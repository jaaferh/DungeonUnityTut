using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake() 
    {
        // Destroys GameManager instance if one exists already
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this; // Sets the current instance to the first GameManager it finds in a scene
        SceneManager.sceneLoaded += LoadState; // SceneManager calls all the functions in sceneLoaded on scene change. Add LoadState() here to call it on scene change
        DontDestroyOnLoad(gameObject); // preserves the GameManager instance on scene load (scene change)

        // PlayerPrefs.DeleteAll(); -- deletes saved keys (could use this when you want to reset saved data)
    }

    // Resources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponprices;
    public List<int> xpTable;

    // References
    public Player player;
    // public Weapon weapon

    // Logic
    public int pesos;
    public int experience;

    // Save State
    /*
    Int preferredSkin
    Int pesos
    Int experience
    int weaponLevel
    */
    public void SaveState() 
    {
        string s = "";

        // 0|10|15|2 -- example of SaveState
        s += "0" + "|"; // preferredSkin
        s += pesos.ToString() + "|"; // pesos
        s += experience.ToString() + "|"; // experience
        s += "1"; // weaponLevel

        PlayerPrefs.SetString("SaveState", s); // Saves the properties we need
    }

    public void LoadState(Scene s, LoadSceneMode mode)
    {
        if (!PlayerPrefs.HasKey("SaveState"))
            return; // nothing to load

        string[] data = PlayerPrefs.GetString("SaveState").Split('|'); 
        
        // change player skin
        pesos = int.Parse(data[1]);
        experience = int.Parse(data[2]);
        // change weapon level
        
        Debug.Log("LoadState");
    }
}
