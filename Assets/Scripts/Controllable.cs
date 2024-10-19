using UnityEngine;

public class Controllable : MonoBehaviour
{
    [SerializeField] private float      selectionRadius = 20.0f;
    [SerializeField] private Vector2    selectionOffset = Vector2.zero;
    [SerializeField] private GameObject selectorIcon;

    private Character character;
    
    public bool      canSelect = true;
    public Vector2   lastMoveOrderPos;

    private void Start()
    {
        PlayerControl.AddControllable(this);

        character = GetComponent<Character>();

        lastMoveOrderPos = transform.position;
        selectorIcon?.SetActive(false);
    }

    private void OnDestroy()
    {
        PlayerControl.RemoveControllable(this);
    }

    public bool Overlap(Vector2 pos)
    {
        return (Vector2.Distance(transform.position.xy() + selectionOffset, pos) < selectionRadius);
    }

    public bool CanSelect()
    {
        return canSelect;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + selectionOffset.xy0(), selectionRadius);
    }
}
