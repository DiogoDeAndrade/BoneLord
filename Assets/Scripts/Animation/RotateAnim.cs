using UnityEngine;

public class RotateAnim : MonoBehaviour
{
    [SerializeField] private Vector3 rotationVector = Vector3.forward;
    [SerializeField] private float   rotationSpeed = 45.0f;

    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, rotationVector);
    }
}
