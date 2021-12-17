using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : Collidable
{
    public string[] sceneNames; // The list of scenes to teleport to
    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player")
        {
            Debug.Log("Player Collide @@@@@@@@@@@@@");
            // Teleport the player
            GameManager.instance.SaveState(); // Save the game before scene change
            string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];
            // string sceneName = sceneNames[1];
            SceneManager.LoadScene(sceneName);
        }
    }
}
