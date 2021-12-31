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

    // private float timeStarted;

    // protected override void Update() 
    // {
    //     base.Update();
    //     Debug.Log(timeStarted);
    //     Debug.Log(Time.time);
    //     if (Time.time - timeStarted > director.duration)
    //         StopCutscene();
         
    // }
    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player")
        {
            GameManager.instance.player.inDialogue = true;
            director.Play();
            mainCam.enabled = false;
            cutsceneCam.enabled = true;
            Destroy(this);

            // timeStarted = Time.time;

            // Invoke(nameof(StopCutscene), 5.5f);
        }
    }

    private void StopCutscene(PlayableDirector obj)
    {
        cutsceneCam.enabled = false;
        mainCam.enabled = true;
        GameManager.instance.player.inDialogue = false;
        director.Stop();
        Debug.Log("test");
    }
}
