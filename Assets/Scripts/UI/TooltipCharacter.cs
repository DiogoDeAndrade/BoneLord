using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipCharacter : Tooltip
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] Image           healthBar;
    [SerializeField] Gradient        healthGradient;

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
            nameText.color = Globals.GetColor(character.faction);

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
}
