using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.Rendering.DebugUI;
using System;
using System.Runtime.CompilerServices;
using UnityEditor;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private Camera              mainCamera;
    [SerializeField] 
    private Hypertag            boneLord;
    [SerializeField]
    private Hypertag            summoningCircleTag;
    [SerializeField]
    private UIPanel[]           panels;
    [SerializeField]
    private TooltipManager      tooltipManager;
    [SerializeField]
    private LayerMask           itemLayer;
    [SerializeField]
    private LayerMask           characterLayer;
    [SerializeField]
    private GameCursor          cursor;

    static PlayerControl Instance;

    private Controllable            currentSelection;
    private List<Character>         characters;
    private Inventory               playerInventory;
    private Character               playerCharacter;
    private Character               summoningCircleCharacter;
    private DisplayInventory        inventoryDisplay;
    private UISummoningCircle  summoningCircle;
    private Sprite                  defaultCursor;

    void Awake()
    {
        Instance = this;
        characters = new();
    }

    private void Start()
    {
        playerInventory = gameObject.FindObjectOfTypeWithHypertag<Inventory>(boneLord);
        playerCharacter = gameObject.FindObjectOfTypeWithHypertag<Character>(boneLord);
        summoningCircleCharacter = gameObject.FindObjectOfTypeWithHypertag<Character>(summoningCircleTag);

        inventoryDisplay = GetPanel<DisplayInventory>();
        inventoryDisplay?.SetInventory(playerInventory);

        summoningCircle = GetPanel<UISummoningCircle>();
        summoningCircle?.SetInventory(playerInventory);

        defaultCursor = cursor.GetImage();
        cursor.SetCamera(mainCamera);
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 mp = Input.mousePosition;

        UIPanel hoverPanel = null;

        foreach (var panel in panels)
        {
            if (!panel.isOpen) continue;

            RectTransform rt = panel.transform as RectTransform;

            if (rt.ScreenPointOverlaps(mp, mainCamera))
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
                else if (hoverCharacter == summoningCircleCharacter)
                {
                    ToggleSummoningCircle();
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
        if (Input.GetKeyDown(KeyCode.S))
        {
            ToggleSummoningCircle();
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
    void ToggleSummoningCircle()
    {
        if (!summoningCircle.isOpen)
        {
            CloseAllPanels();
        }
        summoningCircle.ToggleDisplay();
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

    void CloseAllPanels()
    {
        foreach (var panel in panels)
        {
            panel.Close();
        }
    }

    void _SetCursor(Sprite sprite, float scale = 1.0f)
    {
        if (sprite == null)
        {
            cursor.SetImage(defaultCursor, scale);
        }
        else
        {
            cursor.SetImage(sprite, scale);
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

    static public void SetCursor(Sprite sprite, float scale = 1.0f)
    {
        Instance._SetCursor(sprite, scale);
    }
}
