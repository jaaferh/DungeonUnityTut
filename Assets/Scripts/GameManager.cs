using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Resources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;

    // References
    public Player player;
    public Weapon weapon;
    public FloatingTextManager floatingTextManager;
    public RectTransform hitpointBar;
    public GameObject hud;
    public GameObject menu;

    // Logic
    public int pesos;
    public int experience;

    private void Awake() 
    {
        // Destroys GameManager instance and all other DontDestroyOnLoad objects if they already exist
        if (instance != null)
        {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(hud);
            Destroy(menu);
            return;
        }

        instance = this; // Sets the current instance to the first GameManager it finds in a scene
        SceneManager.sceneLoaded += LoadState; // SceneManager calls all the functions in sceneLoaded on scene change. Add LoadState() here to call it on scene change
        SceneManager.sceneLoaded += OnSceneLoaded;

        // PlayerPrefs.DeleteAll(); -- deletes saved keys (could use this when you want to reset saved data)
    }

    // Floating Text
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    // Hitpoint Bar
    public void OnHitPointChange()
    {
        float ratio = (float) player.hitpoint / (float) player.maxHitpoint;
        hitpointBar.localScale = new Vector3(1, ratio, 1); // scale change on y axis;
    }

    // Upgrade Weapon
    public bool TryUpgradeWeapon()
    {
        // is the weapon max level?
        if (weaponPrices.Count <= weapon.weaponLevel)
            return false;

        if (pesos >= weaponPrices[weapon.weaponLevel])
        {
            pesos -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            return true;
        }

        return false;
    }

    // Experience System
    public int GetCurrentLevel()
    {
        int r = 0;
        int add = 0;

        while (experience >= add)
        {
            add += xpTable[r];
            r++;

            if (r == xpTable.Count) // Max Level
                return r;
        }

        return r;
    }

    public int GetXpToLevel(int level)
    {
        int r = 0;
        int xp = 0;

        while (r < level)
        {
            xp += xpTable[r];
            r++;
        }

        return xp;
    }

    public void GrantXp(int xp)
    {
        int currLevel = GetCurrentLevel();
        experience += xp;

        if (currLevel < GetCurrentLevel())
            OnLevelUp();
    }

    public void OnLevelUp()
    {
        player.OnLevelUp();
        instance.OnHitPointChange();
    }

    // On Scene Load
    public void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }

    // Save State
    public void SaveState() 
    {
        string s = "";

        // 0|10|15|2 -- example of SaveState
        s += "0" + "|"; // preferredSkin
        s += pesos.ToString() + "|"; // pesos
        s += experience.ToString() + "|"; // experience
        s += weapon.weaponLevel.ToString(); // weaponLevel

        PlayerPrefs.SetString("SaveState", s); // Saves the properties we need
    }

    public void LoadState(Scene s, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= LoadState;

        if (!PlayerPrefs.HasKey("SaveState"))
            return; // nothing to load

        string[] data = PlayerPrefs.GetString("SaveState").Split('|'); 

        // changeplayerskin
        pesos = int.Parse(data[1]);

        // Experience
        experience = int.Parse(data[2]);
        if (GetCurrentLevel() != 1)
            player.SetLevel(GetCurrentLevel());

        // Weapon Level
        int weaponLvl = int.Parse(data[3]);
        weapon.SetWeaponLevel(weaponLvl);
    }
}
