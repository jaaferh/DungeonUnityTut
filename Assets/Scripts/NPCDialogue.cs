using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialogue : MonoBehaviour
{
    public bool active = false;
    public GameObject npcDialogue;
    public Text npcName;
    public Text message;

    protected virtual void Update()
    {
        var returnKey = Input.GetKeyUp(KeyCode.Return);

        if (active && returnKey)
            Hide();
    }

    public void Show(string name, string msg)
    {
        npcName.text = name;
        message.text = msg;
        active = true;
        npcDialogue.SetActive(active);
    }

    public void Hide()
    {
        active = false;
        npcDialogue.SetActive(active);
    }
}
