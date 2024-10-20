using NaughtyAttributes;
using System;
using System.IO;
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
    private float   _enemyDetectionRadius = 0.0f;
    [SerializeField]
    private Transform _toolPivot;
    [SerializeField, MinMaxSlider(0, 4)]
    private Vector2Int dropCount;

    public float        moveSpeed => _moveSpeed;
    public bool         isPlayer => _faction == Faction.Player;
    public Faction      faction => _faction;
    public float        hp { get; private set; }
    public float        maxHP => _maxHP;
    public string       displayName { 
        get { return _displayName; }
        set { _displayName = value; } 
    }
    public Character    closestEnemy { get; private set; }
    public bool         isMoving => targetPos != null;
    public bool         isDead => (hp <= 0);
    public Buffs        buffs => _buffs;
    public float        enemyDetectionRadius => _enemyDetectionRadius;


    float       emoteTimer;
    Animator    animator;
    Animator    animTool;
    Flash       flash;
    Vector2?    targetPos = null;
    bool        alert = false;
    Vector2     ctOffset;
    Buffs       _buffs;
    ItemDef        currentTool;
    GameObject  toolObj;
    DropSystem  dropSystem;

    public delegate void OnAlert(bool alertEnable);
    public event OnAlert onAlert;

    void Awake()
    {
        animator = GetComponent<Animator>();
        flash = GetComponent<Flash>();
        dropSystem = GetComponent<DropSystem>();
        _buffs = new(this);

        if (_toolPivot)
        {
            animTool = _toolPivot.GetComponent<Animator>();
        }
    }

    void Start()
    {
        emoteTimer = _emoteCooldown.Random();
        hp = _maxHP;

        PlayerControl.AddCharacter(this);

        var collider = GetComponent<Collider2D>();
        if (collider)
        {
            ctOffset = new Vector2(collider.bounds.center.x, collider.bounds.max.y);
            ctOffset = transform.worldToLocalMatrix * ctOffset.xy0().xyz1();
        }

        Globals.onTick += OnRPGTick;
    }

    private void OnDestroy()
    {
        PlayerControl.RemoveCharacter(this);

        Globals.onTick -= OnRPGTick;
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
                animator?.SetTrigger("Emote");
            }
        }

        Vector3 deltaPos = transform.position - prevPos;
        Vector3 speed = deltaPos / Time.deltaTime;

        animator?.SetFloat("AbsVelocityX", Mathf.Abs(speed.x));

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

        if (_enemyDetectionRadius > 0)
        {
            var colliders = Physics2D.OverlapCircleAll(transform.position, _enemyDetectionRadius, 1 << gameObject.layer);
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

    public void PlayAttack()
    {
        animator.SetTrigger("Attack");
    }

    public void HoldCast(bool hold)
    {
        animator.SetBool("HoldCast", hold);
    }


    public bool DealDamage(float damage, DamageType damageType)
    {
        if (hp > 0)
        {
            (var actualDamage, var actualDamageType) = buffs.ModifyDamage(-damage, damageType);

            hp = Mathf.Clamp(hp + actualDamage, 0, maxHP);

            Color color = Globals.GetColor(actualDamageType);
            CombatTextManager.SpawnText(gameObject, ctOffset, -actualDamage, "{0}", color, color.ChangeAlpha(0), 1.0f, 1.0f);

            flash.Execute(0.25f, color, color.ChangeAlpha(0));

            if (hp == 0)
            {
                // Die
                animator?.SetTrigger("Die");

                // Drops
                int r = dropCount.Random();
                for (int i = 0; i < r; i++)
                {
                    dropSystem?.Drop();
                }
            }

            return true;
        }

        return false;
    }

    public void ApplyBuff(Buff buff)
    {
        _buffs.Apply(buff);
    }

    private void OnRPGTick()
    {
        _buffs.Tick();
    }
    internal void SetMaxHP(int maxHP)
    {
        _maxHP = maxHP;
    }

    internal void SetTool(ItemDef tool)
    {
        if (_toolPivot == null) return;

        GameObject toolPrefab = Globals.GetPrefab(tool);
        if (toolPrefab)
        {
            toolObj = Instantiate(toolPrefab, _toolPivot);
            currentTool = tool;
        }
    }

    internal void UseTool(bool v)
    {
        animTool?.SetBool("Work", v);
    }

    private void OnDrawGizmosSelected()
    {
        if (_enemyDetectionRadius > 0)
        {
            Gizmos.color = new Color(0.25f, 0.0f, 0.0f, 1.0f);
            Gizmos.DrawWireSphere(transform.position, _enemyDetectionRadius);
        }
    }

}
