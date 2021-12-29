using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTextPerson : Collidable
{
    [System.Serializable]
    public struct Dialogue
    {
        public bool isQuestion;
        public string dialogue;
        public string answer1;
        public string answer2;
    }

    public Dialogue[] dialogue;
    public string[] messages;
    public NPCDialogue npcDialogue;

    protected override void Start()
    {
        base.Start();
    }
    protected override void OnCollide(Collider2D coll)
    {
        var returnKey = Input.GetKeyUp(KeyCode.Return);

        if (!npcDialogue.active && returnKey)
            npcDialogue.Show(this.name, dialogue);
    }
}
