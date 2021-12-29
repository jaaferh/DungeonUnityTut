using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTextPerson : Collidable
{
    [System.Serializable]
    public struct DialoguesReqs
    {
        public Dialogue[] dialogues;
        // public Dictionary<string, bool> requirements;
    }

    [System.Serializable]
    public struct Dialogue
    {
        public bool isQuestion;
        public string dialogue;
        public string answer1;
        public string answer2;
    }

    public TextAsset npcJson;
    public Dialogue[] dialogue;
    public string[] messages;
    public NPCDialogue npcDialogue;

    protected override void Start()
    {
        base.Start();

        DialoguesReqs dlgReqJson = JsonUtility.FromJson<DialoguesReqs>(npcJson.text);
        Debug.Log(dlgReqJson);

        // foreach (Dialogue dlgReq in dlgReqJson.dialogues)
        // {
        //     Debug.Log(dlgReq.dialogue);
        // }
    }
    protected override void OnCollide(Collider2D coll)
    {
        var returnKey = Input.GetKeyUp(KeyCode.Return);

        if (!npcDialogue.active && returnKey)
            npcDialogue.Show(this.name, dialogue);
            // GetNPCDialogueRequirements();
    }

    // private void GetNPCDialogueRequirements()
    // {

    // }
}
