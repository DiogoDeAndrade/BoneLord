using System;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    [SerializeField] ItemDef[] compatibleTools;

    DropSystem dropSystem;

    private void Start()
    {
        dropSystem = GetComponent<DropSystem>();
    }

    internal bool isCompatible(ItemDef tool)
    {
        if (compatibleTools != null)
        {
            foreach (var t in compatibleTools)
            {
                if (t == tool) return true;
            }
        }

        return false;
    }

    public void DropResource()
    {
        if (dropSystem)
        {
            dropSystem.Drop();
        }
    }
}
