using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.Rendering.DebugUI;

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
    [SerializeField]
    private LayerMask           characterLayer;

    static PlayerControl Instance;

    private Controllable        currentSelection;
    private List<Character>     characters;
    private Inventory           playerInventory;
    private Character           playerCharacter;
    private DisplayInventory    inventoryDisplay;

    void Awake()
    {
        Instance = this;
        characters = new();
    }

    private void Start()
    {
        playerInventory = gameObject.FindObjectOfTypeWithHypertag<Inventory>(boneLord);
        playerCharacter = gameObject.FindObjectOfTypeWithHypertag<Character>(boneLord);

        inventoryDisplay = GetPanel<DisplayInventory>();
        inventoryDisplay?.SetInventory(playerInventory);
    }

    void Update()
    {
        Vector2 mp = Input.mousePosition;

        UIPanel hoverPanel = null;

        foreach (var panel in panels)
        {
            if (!panel.isOpen) continue;

            RectTransform rt = panel.transform as RectTransform;

            if (IsOverUI(mp, rt))
            {
                hoverPanel = panel;
                break;
            }
        }        

        if (hoverPanel)
        {
            hoverPanel.UpdateTooltip(tooltipManager);
        }
        else
        {
            // Clicked the mouse
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            Character       hoverCharacter = GetCharacterAt(ray.origin);
            Controllable    hoverControllable = null;

            if (hoverCharacter)
            {
                tooltipManager.SetCharacter(hoverCharacter);

                hoverControllable = hoverCharacter.GetComponent<Controllable>();
                if (hoverControllable)
                {
                    if (!hoverControllable.canSelect)
                    {
                        hoverControllable = null;
                    }
                }
            }
            else
            {
                // Check if mouse is hovering any pickable
                Pickable pickable = GetPickableAt(ray.origin);
                if (pickable)
                {
                    tooltipManager.SetItem(pickable.item);
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                currentSelection?.Deselect();
                currentSelection = hoverControllable;
                currentSelection?.Select();

                if (hoverCharacter == playerCharacter)
                {
                    ToggleInventory();
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
            ToggleInventory();
        }

        if ((currentSelection != null) && (currentSelection.isDead))
        {
            currentSelection.Deselect();
            currentSelection = null;
        }
    }

    void ToggleInventory()
    {
        if (!inventoryDisplay.isOpen)
        {
            CloseAllPanels();
        }
        inventoryDisplay.ToggleDisplay();
    }

    Character GetCharacterAt(Vector2 position)
    {
        Character   closestCharacter = null;
        float       closestDist = float.MaxValue;

        var colliders = Physics2D.OverlapCircleAll(position, 5, characterLayer);
        foreach (var collider in colliders)
        {
            // Check if it is an item
            var character = collider.GetComponent<Character>();
            if (character != null)
            {
                float d = Vector3.Distance(character.transform.position, position);
                if (d < closestDist)
                {
                    closestDist = d;
                    closestCharacter = character;
                }
            }
        }

        return closestCharacter;
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

    static public void AddCharacter(Character character)
    {
        Instance.characters.Add(character);
    }

    static public void RemoveCharacter(Character character)
    {
        Instance.characters.Remove(character);
    }
}
