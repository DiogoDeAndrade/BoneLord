using UnityEngine;

public class Gatherer : MonoBehaviour
{
    [SerializeField] private float     searchRadius;
    [SerializeField] private float     grabRadius;
    [SerializeField] private LayerMask itemLayer;
    [SerializeField] private Hypertag  boneLord;

    private Character       character;
    private Controllable    controllable;
    private Pickable        targetItem;

    private Item[]      inventory;
    private bool        returning;
    private Inventory   playerInventory;
    private float       wanderCooldown = 5.0f;

    void Start()
    {
        character = GetComponent<Character>();
        controllable = GetComponent<Controllable>();
        inventory = new Item[1];
        playerInventory = gameObject.FindObjectOfTypeWithHypertag<Inventory>(boneLord);
    }

    void Update()
    {
        if (!character.isMoving)
        {
            if (returning)
            {
                targetItem = null;
                character.MoveTo(playerInventory.transform.position);
            }
            else
            {
                targetItem = null;

                var colliders = Physics2D.OverlapCircleAll(transform.position, searchRadius, itemLayer);
                foreach (var collider in colliders)
                {
                    // Check if it is an item
                    var item = collider.GetComponent<Pickable>();
                    if (item != null)
                    {
                        // Move towards this item
                        character.MoveTo(item.transform.position);
                        targetItem = item;
                    }
                }

                if (targetItem == null)
                {
                    wanderCooldown -= Time.deltaTime;
                    if (wanderCooldown < 0)
                    {
                        // Wander around
                        Vector2 deltaPos = Random.insideUnitCircle.normalized * Random.Range(20.0f, 40.0f);
                        if (controllable) character.MoveTo(controllable.lastMoveOrderPos + deltaPos);
                        else character.MoveTo(transform.position.xy() + deltaPos);
                        wanderCooldown = 5.0f;
                    }
                }
            }
        }
        else
        {
            if (returning) 
            {
                if (Vector3.Distance(playerInventory.transform.position, transform.position) < grabRadius)
                {
                    if (!playerInventory.isFull)
                    {
                        // Returned
                        returning = false;
                        foreach (var item in inventory)
                        {
                            if (item != null)
                            {
                                playerInventory.Add(item);
                            }
                        }
                        ClearInventory();
                    }
                    else
                    {
                        // Wait to unload
                    }
                }
            }
        }

        if (targetItem != null)
        {
            if (Vector3.Distance(targetItem.transform.position, transform.position) < grabRadius)
            {
                // Grab this item
                Grab(targetItem);
            }
        }
    }

    void Grab(Pickable pickable)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = targetItem.item;
                targetItem = null;
                Destroy(pickable.gameObject);
                returning = IsInventoryFull();
                return;
            }
        }

        returning = true;
    }

    bool IsInventoryFull()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null) return false;
        }

        return true;
    }

    void ClearInventory()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            inventory[i] = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.0f, 0.5f, 0.0f, 1.0f);
        Gizmos.DrawWireSphere(transform.position, grabRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}
