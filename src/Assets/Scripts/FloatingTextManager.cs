using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextManager : MonoBehaviour
{
    public GameObject textContainer; // The outer GameObject of all FloatingText objects to group them all
    public GameObject textPrefab; // The Gam

    private List<FloatingText> floatingTexts = new List<FloatingText>();

    public void Show(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        FloatingText floatingText = GetFloatingText();

        floatingText.txt.text = msg;
        floatingText.txt.fontSize = fontSize;
        floatingText.txt.color = color;

        floatingText.go.transform.position = Camera.main.WorldToScreenPoint(position); // transfer world space to screen space so we can use it in the UI
        floatingText.motion = motion;
        floatingText.duration = duration;

        floatingText.Show();
    }

    private FloatingText GetFloatingText()
    {
        FloatingText txt = floatingTexts.Find(t => !t.active); // find inactive text 

        // create new FloatingText if none in the list is inactive and add to list. this is so that multiple text instances can appear at once
        if (txt == null)
        {
            txt = new FloatingText();

            txt.go = Instantiate(textPrefab); // clones textPrefab into the FloatingText GameObject
            txt.go.transform.SetParent(textContainer.transform);
            txt.txt = txt.go.GetComponent<Text>(); // get text from the FloatingText GameObject

            floatingTexts.Add(txt);
        }

        return txt;
    }

    private void Update()
    {
        foreach (FloatingText txt in floatingTexts)
            txt.UpdateFloatingText();
    }
}
