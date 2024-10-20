using UnityEngine;

public class RighteousFury : MonoBehaviour
{
    [SerializeField] private Hypertag bonelordTag;
    [SerializeField] private float    stepSize = 50.0f;
    [SerializeField] private float    cooldown = 5.0f;

    Character boneLord;
    Character character;
    float     timer = 0.0f;

    void Start()
    {
        boneLord = gameObject.FindObjectOfTypeWithHypertag<Character>(bonelordTag);
        character = GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0.0f)
        {
            if ((boneLord) && (!boneLord.isDead) && (!character.isMoving))
            {
                Vector3 toBonelord = (boneLord.transform.position - transform.position).normalized;
                character.MoveTo(transform.position + toBonelord * stepSize);
                timer = cooldown;
            }
        }
    }
}
