using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    [SerializeField] Camera     mainCamera;
    [SerializeField] Vector2    offset;

    TooltipItem         tooltipItem;
    TooltipCharacter    tooltipCharacter;
    RectTransform       parentTransform;
    RectTransform       rectTransform;

    private void Start()
    {
        tooltipItem = GetComponentInChildren<TooltipItem>();
        tooltipCharacter = GetComponentInChildren<TooltipCharacter>();

        parentTransform = transform.parent as RectTransform;
        rectTransform = transform as RectTransform;
    }

    public void SetItem(Item item)
    {
        tooltipItem.SetItem(item);
    }

    public void SetCharacter(Character character)
    {
        tooltipCharacter.SetCharacter(character);
    }

    public void CloseAll()
    {
        tooltipItem.Close();
        tooltipCharacter.Close();
    }

    private void Update()
    {
        // Get the mouse position in screen space
        Vector2 mousePosition = Input.mousePosition;

        // Convert the screen space mouse position to a position within the canvas' world space
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentTransform, mousePosition, mainCamera, out localPoint);

        // Update the position of the RectTransform to follow the mouse
        rectTransform.localPosition = localPoint + offset;
    }
}
