using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable
{
    public Sprite emptyChest; // store the empty chest as a var so that the original sprite is replaced on collect
    public int pesosAmount = 5;
    protected override void OnCollect()
    {
        if (!collected)
        {
            collected = true;
            GetComponent<SpriteRenderer>().sprite = emptyChest; // get current sprite and change it to empty chest
            Debug.Log("Grant " + pesosAmount + " Pesos");
        }
    }
}
