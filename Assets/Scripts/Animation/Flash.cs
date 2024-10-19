using UnityEngine;
using System.Collections;

public class Flash : MonoBehaviour
{
    Material    originalMaterial;
    Material    flashMaterial;
    Renderer    mainRenderer;
    Coroutine   flashCR;

    void Start()
    {
        mainRenderer = GetComponent<Renderer>();
        if (mainRenderer)
        {
            originalMaterial = mainRenderer.material;
            flashMaterial = new Material(originalMaterial);
            flashMaterial.shader = Shader.Find("Shader Graphs/FlashShader");
        }
    }

    public void Execute(float duration, Color colorStart, Color colorEnd)
    {
        if (flashCR != null)
        {
            StopCoroutine(flashCR);
            mainRenderer.material = originalMaterial;
        }
        flashCR = StartCoroutine(FlashCR(duration, colorStart, colorEnd));
    }

    IEnumerator FlashCR(float duration, Color colorStart, Color colorEnd)
    {
        mainRenderer.material = flashMaterial;

        float timer = duration;

        while (timer > 0)
        {
            float t = 1.0f - (timer / duration);
            Color c = Color.Lerp(colorStart, colorEnd, t);

            flashMaterial.SetColor("_FlashColor", c);

            timer -= Time.deltaTime;

            yield return null;
        }

        mainRenderer.material = originalMaterial;
    }

}
