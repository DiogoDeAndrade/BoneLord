using UnityEngine;
using UnityEngine.UI;

public class BuffDisplayElem : MonoBehaviour
{
    [SerializeField] Image  fader;
    [SerializeField] Image  icon;

    public void SetBuff(Buff.Instance instance)
    {
        if (instance == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            icon.sprite = instance.type.image;
            fader.fillAmount = instance.elapsedTimePercentage;
        }
    }
}
