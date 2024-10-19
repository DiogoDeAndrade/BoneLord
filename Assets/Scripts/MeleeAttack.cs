using System.Collections;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private float      attackRadius = 20.0f;
    [SerializeField] private float      damageDelayTime = 0.1f;
    [SerializeField] private float      attackCooldown = 1.0f;
    [SerializeField] private int        baseAttackPower = 1;
    [SerializeField] private DamageType damageType;
    [SerializeField] private Buff[]     buffsToApply;

    private Animator    animator;
    private Character   character;
    private float       attackTimer;

    void Start()
    {
        character = GetComponent<Character>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0)
        {
            var enemy = character.closestEnemy;
            if (enemy == null) return;

            if (Vector3.Distance(enemy.transform.position, transform.position) < attackRadius)
            {
                StartCoroutine(AttackCR(enemy));
            }
        }
    }

    IEnumerator AttackCR(Character enemy)
    {
        animator.SetTrigger("Attack");
        attackTimer = attackCooldown + damageDelayTime;

        yield return new WaitForSeconds(damageDelayTime);

        if (enemy.DealDamage(GetAttackPower(), damageType))
        {
            if (buffsToApply != null)
            {
                foreach (var buff in buffsToApply)
                {
                    enemy.ApplyBuff(buff);
                }
            }
        }
    }

    float GetAttackPower()
    {
        return baseAttackPower;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
