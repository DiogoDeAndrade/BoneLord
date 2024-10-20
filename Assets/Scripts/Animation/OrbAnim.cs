using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OrbAnim : MonoBehaviour
{
    [SerializeField] private float      riseHeight = 20.0f;
    [SerializeField] private float      riseTime = 1.0f;
    [SerializeField] private float      waitTime = 2.0f;
    [SerializeField] private float      attractionFactor = 0.25f;
    [SerializeField] private Vector2    initialVelocity;
    [SerializeField] private Hypertag   bonelordTag;

    Light2D     light;
    Character   boneLord;

    void Start()
    {
        boneLord = gameObject.FindObjectOfTypeWithHypertag<Character>(bonelordTag);

        light = GetComponentInChildren<Light2D>();

        StartCoroutine(AnimateCR());
    }

    IEnumerator AnimateCR()
    {   
        float elapsed = 0.0f;
        while (elapsed < riseTime)
        {
            float t = (elapsed / riseTime);
            light.intensity = t;
            Vector3 currentPos = transform.position;
            currentPos.y += Time.deltaTime * riseHeight / riseTime;
            transform.position = currentPos;
            elapsed += Time.deltaTime;
            
            yield return null;
        }

        yield return new WaitForSeconds(waitTime);

        Vector2 velocity = initialVelocity;
        float   speed = velocity.magnitude;

        while (Vector3.Distance(boneLord.transform.position, transform.position) > 10.0f)
        {
            velocity = (velocity.xy0() + (boneLord.transform.position - transform.position).normalized * speed * attractionFactor).normalized * speed;

            transform.position = transform.position + velocity.xy0() * Time.deltaTime;

            attractionFactor += Time.deltaTime * 0.01f;

            yield return null;
        }

        Inventory inventory = boneLord.GetComponent<Inventory>();
        inventory?.ChangeSouls(1);
        Destroy(gameObject);
    }
}
