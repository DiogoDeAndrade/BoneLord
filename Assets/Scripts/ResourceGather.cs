using UnityEngine;

public class ResourceGather : MonoBehaviour
{
    [SerializeField] private float     searchRadius = 80.0f;
    [SerializeField] private float     gatherRadius = 20.0f;
    [SerializeField] private float     mineCooldown = 5.0f;

    private Character       character;
    private Controllable    controllable;

    private float           wanderCooldown = 5.0f;
    private Item            tool;
    private ResourceNode    targetNode;
    private float           mineTimer;
    private LayerMask       nodeLayer;

    void Start()
    {
        character = GetComponent<Character>();
        controllable = GetComponent<Controllable>();
        nodeLayer = LayerMask.GetMask("Nodes");

        mineTimer = mineCooldown;
    }

    void Update()
    {
        if (!character.isMoving)
        {
            targetNode = null;

            ResourceNode    closestNode = null;
            float           closestDist = float.MaxValue;

            var colliders = Physics2D.OverlapCircleAll(transform.position, searchRadius, nodeLayer);
            foreach (var collider in colliders)
            {
                // Check if it is an resource node
                var node = collider.GetComponent<ResourceNode>();
                if (node != null)
                {
                    if (node.isCompatible(tool))
                    {
                        float d = Vector3.Distance(node.transform.position, transform.position);
                        if (d < closestDist)
                        {
                            closestDist = d;
                            closestNode = node;
                        }
                    }
                }
            }

            if (closestNode != null)
            {
                // Move towards this item
                if (closestDist > gatherRadius)
                {
                    Vector3 targetPos = closestNode.transform.position - (closestNode.transform.position - transform.position).normalized * gatherRadius * 0.5f;

                    character.MoveTo(targetPos);
                }

                targetNode = closestNode;
            }

            if (targetNode == null)
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

        if (targetNode)
        {
            if (Vector3.Distance(transform.position, targetNode.transform.position) < gatherRadius)
            {
                character.UseTool(true);

                mineTimer -= Time.deltaTime;
                if (mineTimer < 0)
                {
                    targetNode.DropResource();
                    mineTimer = mineCooldown;
                }
            }
            else
            {
                character.UseTool(false);
            }
        }
        else
        {
            character.UseTool(false);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.0f, 0.5f, 0.0f, 1.0f);
        Gizmos.DrawWireSphere(transform.position, gatherRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }

    internal void SetTool(Item tool)
    {
        this.tool = tool;
    }
}
