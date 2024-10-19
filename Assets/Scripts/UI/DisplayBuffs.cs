using UnityEngine;

public class DisplayBuffs : MonoBehaviour
{
    BuffDisplayElem[] elems;

    void Start()
    {
        elems = GetComponentsInChildren<BuffDisplayElem>();

        foreach (var elem in elems)
        {
            elem.SetBuff(null);
        }
    }

    public void SetBuffs(Buffs buffs)
    {
        gameObject.SetActive(buffs.Count > 0);
        for (int i = 0; i < elems.Length; i++)
        {
            var instance = buffs.GetInstance(i);
            elems[i].SetBuff(instance);
        }
    }
}
