using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Collidable
{
    // Logic
    protected bool collected;
    protected override void OnCollide(Collider2D coll)
    {
        // Make sure the collectable doesn't call OnCollect() when colliding with a wall or something other than the player
        if (coll.name == "Player")
        {
            OnCollect();
        }
    }    

    protected virtual void OnCollect()
    {
        collected = true;
    }
}
