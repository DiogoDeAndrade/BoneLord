using UnityEngine;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    static PlayerControl Instance;

    private Controllable        currentSelection;
    private List<Controllable>  controllables;

    void Awake()
    {
        Instance = this;
        controllables = new();
    }

    private void Start()
    {
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Clicked the mouse
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

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
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (currentSelection != null)
            {
                currentSelection.MoveTo(ray.origin);
            }
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

    static public void AddControllable(Controllable controllable)
    {
        Instance.controllables.Add(controllable);
    }

    static public void RemoveControllable(Controllable controllable)
    {
        Instance.controllables.Remove(controllable);
    }
}
