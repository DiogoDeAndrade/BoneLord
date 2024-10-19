using NaughtyAttributes;
using UnityEngine;

public class ZByY : MonoBehaviour
{
    [SerializeField] private float baseY = 0.0f;
    [SerializeField] private float slopeZ = 0.001f;

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.z = (pos.y - baseY) * slopeZ;
        transform.position = pos;
    }

    [Button("Force Update")]
    void ForceUpdate()
    {
        LateUpdate();
    }
}
