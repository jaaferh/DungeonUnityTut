using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static NPCTextPerson;

public class NPCDialogue : MonoBehaviour
{
    public const float charDelay = 0.025f;

    // References
    public GameObject npcDialogue;
    public NPCTextPerson npcTextPerson;
    public Text npcName;
    public Text message;
    public Animator blinkAnim;
    public Text question;
    public Text answer1;
    public Text answer2;
    private IEnumerator coroutine;
    private Image ans1Sel;
    private Image ans2Sel;

    // Logic
    public bool active = false;
    private Dialogue[] dialogueArr;
    private int messageIndex;
    private bool messageDone = false;


    protected virtual void Start()
    {
        ans1Sel = answer1.GetComponentInChildren<Image>();
        ans2Sel = answer2.GetComponentInChildren<Image>();
    }

    public void Show(string name, Dialogue[] dlg)
    {
        dialogueArr = dlg;
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
        bool isQuestion = dialogueArr[messageIndex].isQuestion;

        // Check up/down keys presses for question selection
        bool upDown = Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow);
        if (isQuestion && upDown)
            ToggleSelect();

        // Check Enter key for next dialogue
        var returnKey = Input.GetKeyUp(KeyCode.Return);
        if (!active || !returnKey)
            return;

        if (messageDone)
        {
            // Return answer for question
            if (dialogueArr[messageIndex].key != string.Empty)
            {
                bool value = isQuestion ? ans1Sel.enabled : true; // true if not a question to indicate encounters done (if its not a question its an encounter)
                npcTextPerson.AddToChoices(dialogueArr[messageIndex].key, value, isQuestion);
                if (isQuestion) // return if question, since a dialogue will always follow. dont want to continue since it'll disable the NPCDialogue GO
                    return;
            }

            bool moreMessages = messageIndex < dialogueArr.Length - 1; // Do more messages exist?
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
            message.text = dialogueArr[messageIndex].dialogue; // complete message

            if (isQuestion)
            {
                question.text = dialogueArr[messageIndex].dialogue; // complete questions & answers
                answer1.text = dialogueArr[messageIndex].answer1;
                answer2.text = dialogueArr[messageIndex].answer2;
                ans1Sel.enabled = true;
            }

            SetMessageDone(true);
        }
    }

    private void UpdateText()
    {
        SetMessageDone(false);

        Dialogue thisDialogue = dialogueArr[messageIndex];
        coroutine = DisplayText(thisDialogue); // save coroutine
        StartCoroutine(coroutine);
    }

    private IEnumerator DisplayText(Dialogue dlg)
    {
        if (!dlg.isQuestion)
        {
            question.gameObject.SetActive(false);
            message.gameObject.SetActive(true);
            message.text = string.Empty; // empty out text

            foreach (char c in dlg.dialogue)
            {
                message.text += c;
                yield return new WaitForSeconds(charDelay);
            }
            SetMessageDone(true);
        }
        else
        {
            message.gameObject.SetActive(false);
            question.gameObject.SetActive(true);
            question.text = answer1.text = answer2.text = string.Empty; // empty out text
            ans1Sel.enabled = ans2Sel.enabled = false; // reset selection by disabling both

            foreach (char c in dlg.dialogue)
            {
                question.text += c;
                yield return new WaitForSeconds(charDelay);
            }

            answer1.text = dlg.answer1;
            answer2.text = dlg.answer2;
            ans1Sel.enabled = true;

            SetMessageDone(true);
        }
    }

    private void SetMessageDone(bool done)
    {
        messageDone = done;
        blinkAnim.SetBool("MessageDone", done); // enable blink animation
    }

    private void ToggleSelect()
    {
        ans1Sel.enabled = !ans1Sel.enabled;
        ans2Sel.enabled = !ans2Sel.enabled;
    }
}
