using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipCharacter : Tooltip
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] RectTransform   healthContainer;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] Image           healthBar;
    [SerializeField] Gradient        healthGradient;
    [SerializeField] DisplayBuffs    displayBuffs;

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
            if (maxHP == 0)
            {
                healthContainer.gameObject.SetActive(false);
                displayBuffs.SetBuffs(null);
            }
            else
            {
                healthText.text = $"{hp}/{maxHP}";
                healthRectTransform.localScale = new Vector3(hpPercentage, 1.0f, 1.0f);
                healthContainer.gameObject.SetActive(true);
                healthBar.color = healthGradient.Evaluate(hpPercentage);
                displayBuffs.SetBuffs(character.buffs);
            }

            lastUpdated = Time.time;
            Open();
        }
        else
        {
            Close();
        }
    }
}
