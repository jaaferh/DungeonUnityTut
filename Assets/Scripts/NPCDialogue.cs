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
    private string[] messages;
    private int messageIndex;

    protected virtual void Update()
    {
        var returnKey = Input.GetKeyUp(KeyCode.Return);

        if (active && returnKey)
        {
            bool moreMessages = messageIndex < messages.Length - 1;
            if (moreMessages)
            {
                message.text = messages[messageIndex + 1];
                messageIndex++;
            }
            else
                Hide();
        }
    }

    public void Show(string name, string[] msgs)
    {
        messages = msgs;
        messageIndex = 0;
        active = true;
        GameManager.instance.player.inDialogue = active;

        npcName.text = name;
        message.text = msgs[0];

        npcDialogue.SetActive(active);
    }

    public void Hide()
    {
        active = false;
        GameManager.instance.player.inDialogue = active;
        npcDialogue.SetActive(active);
    }
}
