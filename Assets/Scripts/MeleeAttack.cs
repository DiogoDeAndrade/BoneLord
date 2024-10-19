using System;
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
        if (character.isDead) return;

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0)
        {
            if (character.isMoving) return;

            var enemy = character.closestEnemy;
            if (enemy == null) return;

            if (InDistance(enemy))
            {
                StartCoroutine(AttackCR(enemy));
            }
            else if (character.moveSpeed > 0)
            {
                Vector3 pos = enemy.transform.position - (enemy.transform.position - transform.position).normalized * attackRadius * 0.5f;

                character.MoveTo(pos);
            }
        }
    }

    bool InDistance(Character enemy)
    {
        return Vector3.Distance(enemy.transform.position, transform.position) < attackRadius;
    }

    IEnumerator AttackCR(Character enemy)
    {
        character.PlayAttack();
        attackTimer = attackCooldown + damageDelayTime;

        yield return new WaitForSeconds(damageDelayTime);

        // Check if still in distance and not dead (attacker and enemy)
        if ((InDistance(enemy)) && (!character.isDead) && (!enemy.isDead))
        {
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

    internal void Set(float attackRadius = 20.0f, float damageDelayTime = 0.5f, float attackCooldown = 1.0f, int attackPower = 1, DamageType damageType = DamageType.Physical)
    {
        this.attackRadius = attackRadius;
        this.damageDelayTime = damageDelayTime;
        this.attackCooldown = attackCooldown;
        this.baseAttackPower = attackPower;
        this.damageType = damageType;
    }
}
