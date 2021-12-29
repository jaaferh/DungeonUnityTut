using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NPCTextPerson : Collidable
{
    [System.Serializable]
    public struct DialogueReqColl
    {
        public DialoguesReqs[] dialogueReqColl;
    }

    [System.Serializable]
    public struct DialoguesReqs
    {
        public Dialogue[] dialogues;
        public Dictionary<string, bool> requirements;
    }

    [System.Serializable]
    public struct Dialogue
    {
        public bool isQuestion;
        public string dialogue;
        public string answer1;
        public string answer2;
    }

    // References
    public TextAsset npcJson;
    public NPCDialogue npcDialogue;
    public Dialogue[] dialogue;

    // Logic
    [SerializeField]
    private Dictionary<string, bool> choicesChosen = new Dictionary<string, bool>();
    private string savePath; 

    protected virtual void Awake()
    {
        savePath = Application.persistentDataPath + "/" + this.name + ".json";
    }

    protected override void Start()
    {
        base.Start();

        var dlgReqJson2 = JSONSerializer.Deserialize(typeof(DialogueReqColl), npcJson.text);


        // Get Choices from saved file
        string choicesJson = File.ReadAllText(savePath);
        choicesChosen = (Dictionary<string, bool>)JSONSerializer.Deserialize(typeof(Dictionary<string, bool>), choicesJson);

        if (choicesChosen.Keys.Count < 1)
            choicesChosen.Add("spoken", true);
        
        // Save Choices to saved file
        string serializedChoices = JSONSerializer.Serialize(typeof(Dictionary<string, bool>), choicesChosen);
        File.WriteAllText(savePath, serializedChoices);
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
