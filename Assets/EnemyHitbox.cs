using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : Collidable
{
    // Damage
    public int damage = 1;
    public float pushForce = 5.0f;

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.CompareTag("Fighter") && coll.name == "Player")
        {
            // Create a new dmaage object before sending it to the player
            Damage dmg = new Damage
            {
                damageAmount = damage,
                origin = transform.position,
                pushForce = pushForce
            };

            coll.SendMessage("ReceiveDamage", dmg); // Calls the Fighter ReceiveDamage(dmg) function
        }
    }
}
