using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Character : MonoBehaviour
{
    [SerializeField] 
    private Faction _faction = Faction.Environment;
    [SerializeField, MinMaxSlider(1.0f, 60.0f)]
    private Vector2 _emoteCooldown = new Vector2(10.0f, 30.0f);
    [SerializeField]
    private bool    flipHorizontal;
    [SerializeField]
    private float   _moveSpeed = 100.0f;
    [SerializeField]
    private float   _maxHP = 20;
    [SerializeField]
    private string  _displayName;
    [SerializeField]
    private float   enemyDetectionRadius = 0.0f;

    public bool         isPlayer => _faction == Faction.Player;
    public Faction      faction => _faction;
    public float        hp { get; private set; }
    public float        maxHP => _maxHP;
    public string       displayName => _displayName;
    public Character    closestEnemy { get; private set; }
    public bool         isMoving => targetPos != null;
    public bool         isDead => (hp <= 0);


    float       emoteTimer;
    Animator    animator;
    Flash       flash;
    Vector2?    targetPos = null;
    bool        alert = false;
    Vector2     ctOffset;

    public delegate void OnAlert(bool alertEnable);
    public event OnAlert onAlert;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        flash = GetComponent<Flash>();
        emoteTimer = _emoteCooldown.Random();
        hp = _maxHP;

        PlayerControl.AddCharacter(this);

        var collider = GetComponent<Collider2D>();
        if (collider)
        {
            ctOffset = new Vector2(collider.bounds.center.x, collider.bounds.max.y);
            ctOffset = transform.worldToLocalMatrix * ctOffset.xy0().xyz1();
        }
    }

    private void OnDestroy()
    {
        PlayerControl.RemoveCharacter(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;

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

        // Detect enemy?
        closestEnemy = DetectClosestEnemy();

        if (closestEnemy != null)
        {
            if (!alert)
            {
                alert = true;
                onAlert?.Invoke(true);
            }
            // Modify speed so that this character points towards the enemy instead of the move direction
            speed.x = closestEnemy.transform.position.x - transform.position.x;
        }
        else
        {
            if (alert)
            {
                alert = false;
                onAlert?.Invoke(false);
            }
        }

        if (flipHorizontal) speed.x = -speed.x;
        if ((speed.x < 0) && (transform.right.x > 0)) transform.rotation = Quaternion.Euler(0, 180, 0);
        else if ((speed.x > 0) && (transform.right.x < 0)) transform.rotation = Quaternion.identity;        
    }

    Character DetectClosestEnemy()
    {
        Character   closestCharacter = null;
        float       closestDist = float.MaxValue;

        if (enemyDetectionRadius > 0)
        {
            var colliders = Physics2D.OverlapCircleAll(transform.position, enemyDetectionRadius, 1 << gameObject.layer);
            foreach (var collider in colliders)
            {
                // Check if it is an item
                var character = collider.GetComponent<Character>();
                if (character != null)
                {
                    if ((!character.isDead) && (character.faction.IsEnemy(_faction)))
                    {
                        float d = Vector3.Distance(character.transform.position, transform.position);
                        if (d < closestDist)
                        {
                            closestDist = d;
                            closestCharacter = character;
                        }
                    }
                }
            }
        }

        return closestCharacter;
    }

    public void MoveTo(Vector2 targetPos)
    {
        this.targetPos = targetPos;
    }

    public void Stop()
    {
        targetPos = null;
    }

    public void DealDamage(float damage, DamageType damageType)
    {
        if (hp > 0)
        {
            hp = Mathf.Clamp(hp - damage, 0, maxHP);

            Color color = Globals.GetColor(damageType);
            CombatTextManager.SpawnText(gameObject, ctOffset, -damage, "{0}", color, color.ChangeAlpha(0), 1.0f, 1.0f);

            flash.Execute(0.25f, color, color.ChangeAlpha(0));

            if (hp == 0)
            {
                // Die
                animator.SetTrigger("Die");
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (enemyDetectionRadius > 0)
        {
            Gizmos.color = new Color(0.25f, 0.0f, 0.0f, 1.0f);
            Gizmos.DrawWireSphere(transform.position, enemyDetectionRadius);
        }
    }
}
