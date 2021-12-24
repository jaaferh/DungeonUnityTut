using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTextPerson : Collidable
{
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
            npcDialogue.Show(this.name, messages);
    }
}
