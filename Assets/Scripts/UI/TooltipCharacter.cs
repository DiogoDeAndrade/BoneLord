using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipCharacter : Tooltip
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] Image           healthBar;
    [SerializeField] Gradient        healthGradient;
    [SerializeField] Color           playerColor = Color.white;   
    [SerializeField] Color           enemyColor = Color.white;
    [SerializeField] Color           environmentColor = Color.white;

    RectTransform healthRectTransform;

    protected override void Start()
    {
        base.Start();

        healthRectTransform = healthBar.transform as RectTransform;
    }

    public void SetCharacter(Character character)
    {
        if (character != null)
        {
            nameText.text = character.displayName;
            nameText.color = GetColorByFaction(character.faction);

            int hp = Mathf.FloorToInt(character.hp);
            int maxHP = Mathf.FloorToInt(character.maxHP);
            float hpPercentage = (float)hp / (float)maxHP;
            healthText.text = $"{hp}/{maxHP}";
            healthRectTransform.localScale = new Vector3(hpPercentage, 1.0f, 1.0f);
            healthBar.color = healthGradient.Evaluate(hpPercentage);

            lastUpdated = Time.time;
            Open();
        }
        else
        {
            Close();
        }
    }

    Color GetColorByFaction(Faction faction)
    {
        switch (faction)
        {
            case Faction.Player: return playerColor;
            case Faction.Enemy: return enemyColor;
            case Faction.Environment: return environmentColor;
        }

        return Color.white;
    }
}
