using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collidable : MonoBehaviour
{
    public ContactFilter2D filter; // filters what you can collide with
    private BoxCollider2D boxCollider; 
    private Collider2D[] hits = new Collider2D[10]; // what did you hit in a frame

    // virtual method allows for overriding
    protected virtual void Start() 
    {
        boxCollider = GetComponent<BoxCollider2D>(); // inits box collider
    }

    protected virtual void Update()
    {
        // Collision work
        boxCollider.OverlapCollider(filter, hits); // returns everything colliding with this boxCollider, filtered by filter
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
                continue;

            // Do something on collide (hit)
            OnCollide(hits[i]);

            // The array is not cleaned up, so we do it ourself
            hits[i] = null;
        }
    }

    protected virtual void OnCollide(Collider2D coll)
    {
        Debug.Log("OnCollide was not implemented in " + this.name);
    }
}
