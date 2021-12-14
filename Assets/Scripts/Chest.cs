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
            // transform.position refers to the position of this object in the gamespace
            GameManager.instance.ShowText("+" + pesosAmount + " Pesos", 25, Color.yellow, transform.position, Vector3.up * 25, 1.5f);
        }
    }
}
