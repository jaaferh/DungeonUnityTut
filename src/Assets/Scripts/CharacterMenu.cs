using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    // Text fields
    public Text levelText, hitpointText, pesosText, upgradeCostText, xpText;

    // Logic
    private int currentCharacterSelection = 0;
    public Image characterSelectionSprite;
    public Image weaponSprite;
    public RectTransform xpBar; // will use this to change the local scale of the xpBar image to represent xp graphically

    // Character selection
    public void OnArrowClick(bool right)
    {
        if (right)
        {
            currentCharacterSelection++;

            // If currCharSelection too much (as defined by GameManager playerSprites list)
            if (currentCharacterSelection == GameManager.instance.playerSprites.Count)
                currentCharacterSelection = 0;

            OnSelectionChange();
        }
        else
        {
            currentCharacterSelection--;

            // If currCharSelection too much (as defined by GameManager playerSprites list)
            if (currentCharacterSelection < 0)
                currentCharacterSelection = GameManager.instance.playerSprites.Count - 1; // zero based counting

            OnSelectionChange();
        }
    }

    private void OnSelectionChange()
    {
        characterSelectionSprite.sprite = GameManager.instance.playerSprites[currentCharacterSelection];
        GameManager.instance.player.SwapSprite(currentCharacterSelection);
    }

    // Weapon Upgrade
    public void OnUpgradeClick()
    {
        if (GameManager.instance.TryUpgradeWeapon())
        {
            UpdateMenu();
        }
    }

    // Update character info
    public void UpdateMenu()
    {
        // Weapon
        int weaponLevel = GameManager.instance.weapon.weaponLevel;
        bool maxWeaponLvl = GameManager.instance.weapon.weaponLevel == GameManager.instance.weaponPrices.Count;

        weaponSprite.sprite = GameManager.instance.weaponSprites[weaponLevel];
        if (maxWeaponLvl)
            upgradeCostText.text = "MAX";
        else
            upgradeCostText.text = GameManager.instance.weaponPrices[weaponLevel].ToString();

        // Meta
        levelText.text = GameManager.instance.GetCurrentLevel().ToString();
        hitpointText.text = GameManager.instance.player.hitpoint.ToString();
        pesosText.text = GameManager.instance.pesos.ToString();

        // XP Bar
        int currLevel = GameManager.instance.GetCurrentLevel();
        bool maxXpLevel = currLevel == GameManager.instance.xpTable.Count;

        if (maxXpLevel)
        {
            xpText.text = GameManager.instance.experience.ToString() + " total experience points"; // Display total XP
            xpBar.localScale = Vector3.one; // Bar full
        }
        else
        {
            int prevLevelXp = GameManager.instance.GetXpToLevel(currLevel - 1);
            int currLevelXp = GameManager.instance.GetXpToLevel(currLevel);

            int diff = currLevelXp - prevLevelXp;
            int currXpIntoLevel = GameManager.instance.experience - prevLevelXp;

            float completionRatio = (float) currXpIntoLevel / (float) diff;
            xpText.text = currXpIntoLevel.ToString() + " / " + diff;
            xpBar.localScale = new Vector3(completionRatio, 1, 1);
        }
    }
}
