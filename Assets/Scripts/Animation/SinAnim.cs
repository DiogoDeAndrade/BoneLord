using UnityEngine;

public class SinAnim : MonoBehaviour
{
    [SerializeField] private float      amplitude = 4.0f;
    [SerializeField] private float      frequency = 180.0f;
    [SerializeField] private float      initialOffset = 0.0f;
    [SerializeField] private Vector3    displacementVector = Vector3.up;

    Vector3 basePos;

    void Start()
    {
        basePos = transform.localPosition;
    }
    
    void Update()
    {
        transform.localPosition = basePos + displacementVector * amplitude * Mathf.Sin((Time.time * frequency + initialOffset) * Mathf.Deg2Rad);
    }
}
