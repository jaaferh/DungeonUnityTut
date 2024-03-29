using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : Collidable
{
    public PlayableDirector director;
    public Camera mainCam;
    public Camera cutsceneCam;

    private void Awake() 
    {
        director.stopped += StopCutscene;
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player")
        {
            GameManager.instance.player.inDialogue = true;
            director.Play();
            mainCam.enabled = false;
            cutsceneCam.enabled = true;
            GameManager.instance.hud.SetActive(false);
            Destroy(this);
        }
    }

    private void StopCutscene(PlayableDirector obj)
    {
        cutsceneCam.enabled = false;
        mainCam.enabled = true;
        GameManager.instance.player.inDialogue = false;
        GameManager.instance.hud.SetActive(true);
        director.Stop();
    }
}
