using UnityEngine;

public class MoveAnim : MonoBehaviour
{
    public Vector3 speed;

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + speed * Time.deltaTime;        
    }
}
