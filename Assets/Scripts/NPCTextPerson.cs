using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public string key;
        public string answer1;
        public string answer2;
    }

    // References
    public TextAsset npcJson;
    public NPCDialogue npcDialogue;
    public Dialogue[] dialogue;

    // Logic
    private DialoguesReqs[] dialoguesReqs;
    private static Dictionary<string, bool> choicesChosen = new Dictionary<string, bool>();
    private string savePath; 

    protected virtual void Awake()
    {
        savePath = Application.persistentDataPath + "/" + this.name + ".json";
    }

    protected override void Start()
    {
        base.Start();

        // Get NPC Dialogue from Text Asset
        var drColl = (DialogueReqColl)JSONSerializer.Deserialize(typeof(DialogueReqColl), npcJson.text);
        dialoguesReqs = drColl.dialogueReqColl;

        // Get Choices from saved file
        if (File.Exists(savePath))
        {
            string choicesJson = File.ReadAllText(savePath);
            choicesChosen = (Dictionary<string, bool>)JSONSerializer.Deserialize(typeof(Dictionary<string, bool>), choicesJson);
        }
    }
    protected override void OnCollide(Collider2D coll)
    {
        var returnKey = Input.GetKeyUp(KeyCode.Return);

        if (!npcDialogue.active && returnKey)
            GetNPCDialogue();
    }

    private void GetNPCDialogue()
    {
        // bool spoken = dialoguesReqs[0].requirements["spoken"];
        // var filtered = dialoguesReqs.Where(dr => dr.requirements.TryGetValue("spoken", out bool spoken)).First();
        // Filter dialogue list by choicesChosen
        var filteredDialogue = dialoguesReqs.Where(dr => {
            bool matches = false;
            foreach (var req in dr.requirements)
            {
                choicesChosen.TryGetValue(req.Key, out bool chosenValue);
                matches = chosenValue == req.Value;
            };
            return matches;                
        }).First();

        npcDialogue.Show(this.name, filteredDialogue.dialogues);
    }

    public static void AddToChoices(string npcName, string key, bool value, bool isQuestion)
    {
        Debug.Log(npcName);
        Debug.Log(key);
        Debug.Log(value);

        choicesChosen.Add(key, value);
        
        // Save Choices to saved file
        string serializedChoices = JSONSerializer.Serialize(typeof(Dictionary<string, bool>), choicesChosen);
        File.WriteAllText(Application.persistentDataPath + "/" + npcName + ".json", serializedChoices);
    }
}
