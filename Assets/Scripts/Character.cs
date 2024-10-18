using NaughtyAttributes;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] 
    private Faction _faction = Faction.Environment;
    [SerializeField, MinMaxSlider(1.0f, 60.0f)]
    private Vector2 _emoteCooldown = new Vector2(10.0f, 30.0f);
    [SerializeField]
    private float   _moveSpeed = 200.0f;


    public bool isPlayer => _faction == Faction.Player;

    float       emoteTimer;
    Animator    animator;
    Vector2?    targetPos = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        emoteTimer = _emoteCooldown.Random();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 prevPos = transform.position;

        if (targetPos.HasValue)
        {
            Vector2 currentPos = transform.position;

            if (Vector2.Distance(currentPos, targetPos.Value) < 1e-3)
            {
                targetPos = null;
            }
            else
            {
                currentPos = Vector2.MoveTowards(currentPos, targetPos.Value, _moveSpeed * Time.deltaTime);

                transform.position = currentPos.xyz(transform.position.z);
            }
        }
        else
        {
            emoteTimer -= Time.deltaTime;
            if (emoteTimer < 0)
            {
                emoteTimer = _emoteCooldown.Random();
                animator.SetTrigger("Emote");
            }
        }

        Vector3 deltaPos = transform.position - prevPos;
        Vector3 speed = deltaPos / Time.deltaTime;

        animator.SetFloat("AbsVelocityX", Mathf.Abs(speed.x));

        if ((speed.x < 0) && (transform.right.x > 0)) transform.rotation = Quaternion.Euler(0, 180, 0);
        else if ((speed.x > 0) && (transform.right.x < 0)) transform.rotation = Quaternion.identity;
    }

    public void MoveTo(Vector2 targetPos)
    {
        this.targetPos = targetPos;
    }

    public void Stop()
    {
        targetPos = null;
    }
}
