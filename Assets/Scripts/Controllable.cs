using UnityEngine;

public class Controllable : MonoBehaviour
{
    [SerializeField] private GameObject selectorIcon;

    private Character character;
    
    public bool      canSelect = true;
    public Vector2   lastMoveOrderPos;

    public bool isDead => character.isDead;

    private void Start()
    {
        character = GetComponent<Character>();

        lastMoveOrderPos = transform.position;
        selectorIcon?.SetActive(false);
    }

    public bool CanSelect()
    {
        return canSelect && !character.isDead;
    }

    public void Select()
    {
        selectorIcon?.SetActive(true);
    }

    public void Deselect()
    {
        selectorIcon?.SetActive(false);
    }
    
    public void MoveTo(Vector2 targetPos)
    {
        lastMoveOrderPos = targetPos;
        character.MoveTo(targetPos);
    }
}
