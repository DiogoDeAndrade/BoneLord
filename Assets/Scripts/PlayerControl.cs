using UnityEngine;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private Camera              mainCamera;
    [SerializeField] 
    private Hypertag            boneLord;
    [SerializeField]
    private UIPanel[]           panels;
    [SerializeField]
    private TooltipManager      tooltipManager;
    [SerializeField]
    private LayerMask           itemLayer;

    static PlayerControl Instance;

    private Controllable        currentSelection;
    private List<Controllable>  controllables;
    private Inventory           playerInventory;
    private DisplayInventory    inventoryDisplay;

    void Awake()
    {
        Instance = this;
        controllables = new();
    }

    private void Start()
    {
        playerInventory = gameObject.FindObjectOfTypeWithHypertag<Inventory>(boneLord);

        inventoryDisplay = GetPanel<DisplayInventory>();
        inventoryDisplay?.SetInventory(playerInventory);
    }

    void Update()
    {
        Vector2 mp = Input.mousePosition;

        bool onPanels = false;
        foreach (var panel in panels)
        {
            if (!panel.isOpen) continue;

            RectTransform rt = panel.transform as RectTransform;

            if (IsOverUI(mp, rt))
            {
                onPanels = true;
                panel.UpdateTooltip(tooltipManager);

                break;
            }
        }

        if (!onPanels)
        {
            // Clicked the mouse
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // Check if mouse is hovering any pickable
            Pickable pickable = GetPickableAt(ray.origin);
            if (pickable)
            {
                tooltipManager.SetItem(pickable.item);
            }

            if (Input.GetMouseButtonDown(0))
            {
                Controllable controllable = GetControllableAt(ray.origin);
                if (controllable)
                {
                    currentSelection?.Deselect();
                    if (controllable != null)
                    {
                        currentSelection = controllable;
                        currentSelection.Select();
                    }
                }
                else
                {
                    currentSelection?.Deselect();
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                if (currentSelection != null)
                {
                    currentSelection.MoveTo(ray.origin);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!inventoryDisplay.isOpen)
            {
                CloseAllPanels();
            }
            inventoryDisplay.ToggleDisplay();
        }
    }

    Controllable GetControllableAt(Vector2 position)
    {
        foreach (var controllable in controllables)
        {
            if (!controllable.CanSelect()) continue;

            if (controllable.Overlap(position))
            {
                return controllable;
            }
        }

        return null;
    }

    Pickable GetPickableAt(Vector2 position)
    {
        Pickable closestPickable = null;
        float closestDist = float.MaxValue;

        var colliders = Physics2D.OverlapCircleAll(position, 5, itemLayer);
        foreach (var collider in colliders)
        {
            // Check if it is an item
            var item = collider.GetComponent<Pickable>();
            if (item != null)
            {
                float d = Vector3.Distance(item.transform.position, position);
                if (d < closestDist)
                {
                    closestDist = d;
                    closestPickable = item;
                }
            }
        }

        return closestPickable;
    }

    private T GetPanel<T>() where T : UIPanel
    {
        foreach (var panel in panels)
        {
            T p = panel as T;
            if (p != null) return p;
        }

        return null;
    }

    private bool isMouseOnPanels()
    {
        Vector2 mp = Input.mousePosition;

        foreach (var panel in panels)
        {
            if (!panel.isOpen) continue;

            RectTransform rt = panel.transform as RectTransform;

            if (IsOverUI(mp, rt))
            {
                return true;
            }
        }

        return false;
    }

    bool IsOverUI(Vector2 pos, RectTransform rectTransform)
    {
        // Convert the mouse position to world space and then to screen point
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, pos, mainCamera, out Vector2 localPoint))
        {
            // Check if the local point is within the rect bounds
            return rectTransform.rect.Contains(localPoint);
        }

        return false;
    }

    void CloseAllPanels()
    {
        foreach (var panel in panels)
        {
            panel.Close();
        }
    }

    static public void AddControllable(Controllable controllable)
    {
        Instance.controllables.Add(controllable);
    }

    static public void RemoveControllable(Controllable controllable)
    {
        Instance.controllables.Remove(controllable);
    }
}
