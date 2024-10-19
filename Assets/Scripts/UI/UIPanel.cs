using System;
using UnityEngine;

[RequireComponent (typeof(CanvasGroup))]
public class UIPanel : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.5f;

    CanvasGroup canvasGroup;

    float canvasInc;

    protected virtual void Start()
    {
        canvasGroup= GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0.0f;
    }

    protected virtual void Update()
    {
        if (canvasInc != 0.0f)
        {
            canvasGroup.alpha = Mathf.Clamp01(canvasGroup.alpha + canvasInc * Time.deltaTime / fadeDuration);
            if (((canvasInc < 0) && (canvasGroup.alpha == 0.0f)) ||
                ((canvasInc > 0) && (canvasGroup.alpha == 1.0f)))
            {
                canvasInc = 0.0f;

                canvasGroup.blocksRaycasts = (canvasGroup.alpha > 0.0f);
            }
        }
    }

    public void ToggleDisplay()
    {
        if (isOpen)
        {
            Close();
        }
        else
        {
            Open();
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

    public virtual void UpdateTooltip(TooltipManager tooltipManager)
    {
        
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
