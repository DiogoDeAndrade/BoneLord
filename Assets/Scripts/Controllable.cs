using UnityEngine;

public class Controllable : MonoBehaviour
{
    [SerializeField] private float      selectionRadius = 20.0f;
    [SerializeField] private Vector2    selectionOffset = Vector2.zero;

    public Color selectColor = Color.yellow;

    Color originalColor;
    Character character;

    private void Start()
    {
        PlayerControl.AddControllable(this);

        character = GetComponent<Character>();

        originalColor = GetComponent<SpriteRenderer>().color;        
    }

    private void OnDestroy()
    {
        PlayerControl.RemoveControllable(this);
    }

    public bool Overlap(Vector2 pos)
    {
        return (Vector2.Distance(transform.position.xy() + selectionOffset, pos) < selectionRadius);
    }

    public void Select()
    {
        GetComponent<SpriteRenderer>().color = selectColor;
    }

    public void Deselect()
    {
        GetComponent<SpriteRenderer>().color = originalColor;
    }
    
    public void MoveTo(Vector2 targetPos)
    {
        character.MoveTo(targetPos);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + selectionOffset.xy0(), selectionRadius);
    }
}
