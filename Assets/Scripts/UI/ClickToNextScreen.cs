using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickToNextScreen : MonoBehaviour
{
    [Scene]
    public string nextScreen;

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(nextScreen);
        }
    }
}
