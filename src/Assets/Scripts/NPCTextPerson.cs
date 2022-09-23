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

    // Logic
    private DialoguesReqs[] dialoguesReqs;
    private static Dictionary<string, bool> choicesChosen = new Dictionary<string, bool>();
    private string savePath; 

    protected override void Start()
    {
        base.Start();

        savePath = GameManager.instance.dialogueSavePath;

        npcDialogue.npcTextPerson = this;

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
        // This is extremely scuffed but it works
        // Prioritise DialoguesReqs by adding them to a dictionary that contains the number of matches of requirements to choicesChosen
        Dictionary<DialoguesReqs, int> dlgReqsPriority = new Dictionary<DialoguesReqs, int>();
        foreach (DialoguesReqs dr in dialoguesReqs)
        {
            bool matches = dr.requirements.Count > 0 || choicesChosen.Count > 0 ? false : true; // If requirements is empty then matches = true (check becomes redundant)
            int matchCount = 0;

            foreach (var req in dr.requirements)
            {
                bool keyExists = choicesChosen.TryGetValue(req.Key, out bool chosenValue);
                matches = keyExists && chosenValue == req.Value;
                if (matches)
                    matchCount++;
            };

            if (matches) // only add matches to dictionary
                dlgReqsPriority.Add(dr, matchCount); 
        }

        var bestDialogueReqs = dlgReqsPriority.OrderByDescending(k => dlgReqsPriority[k.Key]).First().Key; // order dictionary by match count desc for highest match

        npcDialogue.Show(this.name, bestDialogueReqs.dialogues);
    }

    public void AddToChoices(string key, bool value, bool isQuestion)
    {
        if (!choicesChosen.ContainsKey(key))
            choicesChosen.Add(key, value);
        
        // Save Choices to saved file
        string serializedChoices = JSONSerializer.Serialize(typeof(Dictionary<string, bool>), choicesChosen);
        File.WriteAllText(savePath, serializedChoices);

        if (isQuestion)
            GetNPCDialogue();
    }
}
