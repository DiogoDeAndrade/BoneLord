using UnityEngine;

public class Coward : MonoBehaviour
{
    Character character;

    void Start()
    {
        character = GetComponent<Character>();
    }

    void Update()
    {
        var closestEnemy = character.closestEnemy;
        if (closestEnemy != null)
        {
            Vector3 runAwayVector = -(closestEnemy.transform.position - transform.position).normalized;

            character.MoveTo(transform.position + runAwayVector * character.enemyDetectionRadius * 2.0f);
        }
    }
}
