using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    Camera mainCamera;

    public void SetCamera(Camera camera)
    {
        mainCamera = camera;
    }

    private void Update()
    {
        // Get the mouse position in screen space
        Vector2 mousePosition = Input.mousePosition;

        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        transform.position = ray.origin;
    }

}
