using UnityEngine;

public class Tooltip : UIPanel
{
    [SerializeField] private float tooltipDuration = 0.5f;

    protected float lastUpdated = 0.0f;

    protected override void Update()
    {
        base.Update();

        if ((Time.time - lastUpdated) > tooltipDuration)
        {
            Close();
        }
    }
}
