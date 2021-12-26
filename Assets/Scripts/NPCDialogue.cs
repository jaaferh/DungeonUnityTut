using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialogue : MonoBehaviour
{
    // References
    public GameObject npcDialogue;
    public Text npcName;
    public Text message;
    public Animator blinkAnim;
    private IEnumerator coroutine;

    // Logic
    public bool active = false;
    private string[] messages;
    private int messageIndex;
    private bool messageDone = false;


    public void Show(string name, string[] msgs)
    {
        messages = msgs;
        messageIndex = 0;
        active = true;

        GameManager.instance.player.inDialogue = active; // Player freezes when inDialogue = true
        npcName.text = name;

        npcDialogue.SetActive(active);
        UpdateText();
    }

    public void Hide()
    {
        active = false;
        GameManager.instance.player.inDialogue = active;
        npcDialogue.SetActive(active);
    }

    protected virtual void Update()
    {
        var returnKey = Input.GetKeyUp(KeyCode.Return);
        if (!active || !returnKey)
            return;

        if (messageDone)
        {
            bool moreMessages = messageIndex < messages.Length - 1; // Do more messages exist?
            if (moreMessages)
            {
                messageIndex++;
                UpdateText();
            }
            else
                Hide();
        }
        else
        {
            StopCoroutine(coroutine);
            message.text = messages[messageIndex]; // complete message
            SetMessageDone(true);
        }
    }

    private void UpdateText()
    {
        SetMessageDone(false);

        string thisMessage = messages[messageIndex];
        coroutine = DisplayText(thisMessage); // save coroutine
        StartCoroutine(coroutine);
    }

    private IEnumerator DisplayText(string msg)
    {
        message.text = string.Empty;
        foreach (char c in msg)
        {
            message.text += c;
            yield return new WaitForSeconds(0.025f);
        }
        SetMessageDone(true);
    }

    private void SetMessageDone(bool done)
    {
        messageDone = done;
        blinkAnim.SetBool("MessageDone", done);
    }
}
