using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : Collidable
{
    public string[] sceneNames; // The list of scenes to teleport to
    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player")
        {
            // Teleport the player
            string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];
            SceneManager.LoadScene(sceneName);
        }
    }
}
