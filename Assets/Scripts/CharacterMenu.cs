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
        var weaponLevel = GameManager.instance.weapon.weaponLevel;
        var maxWeaponLvl = GameManager.instance.weapon.weaponLevel == GameManager.instance.weaponPrices.Count;
        weaponSprite.sprite = GameManager.instance.weaponSprites[weaponLevel];
        if (maxWeaponLvl)
            upgradeCostText.text = "MAX";
        else
            upgradeCostText.text = GameManager.instance.weaponPrices[weaponLevel].ToString();

        // Meta
        levelText.text = "NOT IMPLEMENTED";
        hitpointText.text = GameManager.instance.player.hitpoint.ToString();
        pesosText.text = GameManager.instance.pesos.ToString();

        // XP Bar
        xpText.text = "NOT IMPLEMENTED";
        xpBar.localScale = new Vector3(0.5f, 0, 0); // 0.5 for the X scale value
    }
}
