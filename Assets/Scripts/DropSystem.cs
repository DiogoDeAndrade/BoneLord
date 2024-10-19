using UnityEngine;

public class DropSystem : MonoBehaviour
{
    [System.Serializable]
    struct DropList
    {
        public Item     item;
        public float    prob;
    }

    [SerializeField] float      noneProbability = 0.0f;
    [SerializeField] DropList[] dropList;
    [SerializeField] Pickable   itemPrefab;

    float accumProb = 0.0f;

    void Start()
    {
        accumProb = noneProbability;

        if (dropList != null)
        {
            foreach (var p in dropList)
            {
                accumProb += p.prob;
            }
        }
    }

    public void Drop()
    {
        float r = Random.Range(0, accumProb);

        foreach (var p in dropList)
        {
            if (r < p.prob)
            {
                Drop(p.item);
                return;
            }
            r -= p.prob;
        }
    }

    void Drop(Item item)
    {
        var newItem = Instantiate(itemPrefab, transform.position + Vector3.down * Random.Range(10.0f, 20.0f) + Vector3.right * Random.Range(-40.0f, 40.0f), Quaternion.identity);
        newItem.SetItem(item);
    }
}
