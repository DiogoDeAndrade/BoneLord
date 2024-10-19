using UnityEngine;

public class DisplaySummoningCircle : UIPanel
{
    DisplayInventory displayInventory;

    protected void Awake()
    {
        displayInventory = GetComponentInChildren<DisplayInventory>();
    }

    public void SetInventory(Inventory inventory)
    {
        displayInventory.SetInventory(inventory);
    }

    public override void Open()
    {
        base.Open();

        displayInventory.Open();
    }

    public override void Close()
    {
        base.Close();
        
        displayInventory.Close();
    }
}
