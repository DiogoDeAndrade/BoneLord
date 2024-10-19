using UnityEngine;

[RequireComponent (typeof(CanvasGroup))]
public class UIPanel : MonoBehaviour
{
    CanvasGroup canvasGroup;

    float canvasInc;

    void Start()
    {
        canvasGroup= GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0.0f;
    }

    private void Update()
    {
        if (canvasInc != 0.0f)
        {
            canvasGroup.alpha = Mathf.Clamp01(canvasGroup.alpha + canvasInc * Time.deltaTime * 2.0f);
            if (((canvasInc < 0) && (canvasGroup.alpha == 0.0f)) ||
                ((canvasInc > 0) && (canvasGroup.alpha == 1.0f)))
            {
                canvasInc = 0.0f;
            }
        }
    }

    public virtual void Open()
    {
        canvasInc = 1.0f;
    }

    public virtual void Close()
    {
        canvasInc = -1.0f;
    }

    public bool isOpen
    {
        get
        {
            if (canvasInc < 0) return false;
            if (canvasInc > 0) return true;

            return canvasGroup.alpha > 0;
        }
    }
}
