using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mover
{
    private AudioSource deathSfx;

    // Experience
    public int xpValue = 1;

    // Logic
    public float triggerLength = 1; // distance for enemy to start chasing
    public float chaseLength = 5; // how far will enemy chase for
    private bool chasing;
    private bool collidingWithPlayer;
    private Transform playerTransform; // movement of the player
    private Vector3 startingPosition;
    private bool isAlive = true;

    // Hitbox
    public ContactFilter2D filter;
    private BoxCollider2D hitbox;
    private readonly Collider2D[] hits = new Collider2D[10]; // duplicate from Collidable.cs

    // References
    public Transform health;
    public ParticleSystem bloodPart;


    protected override void Start()
    {
        base.Start();
        playerTransform = GameManager.instance.player.transform; 
        startingPosition = transform.position; // current position of this object on launch
        hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>(); // gets first child which is the hitbox in this case
        deathSfx = GetComponent<AudioSource>();
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        if (!isAlive)
            return;

        base.ReceiveDamage(dmg);

        // Change health bar
        float ratio = (float) hitpoint / (float) maxHitpoint;
        health.localScale = new Vector3(ratio, 1, 1); // scale change on y axis;

        // Play blood particles animation
        bloodPart.Play();
    }

    private void FixedUpdate()
    {
        if (!isAlive)
            return;

        // Is the player in range?
        if (Vector3.Distance(playerTransform.position, startingPosition) < chaseLength)
        {
            if (Vector3.Distance(playerTransform.position, startingPosition) < triggerLength)
                chasing = true;

            if (chasing)
            {
                if (!collidingWithPlayer)
                {
                    Vector3 towardsPlayer = (playerTransform.position - transform.position).normalized; // normalises a vector to have a direction from enemy towards player position
                    UpdateMotor(towardsPlayer);
                }
            }
            else
            {
                Vector3 towardsStart = startingPosition - transform.position; // towards start position
                UpdateMotor(towardsStart);
            }
        }
        else
        {
            UpdateMotor(startingPosition - transform.position); // towards start position
            chasing = false;
        }


        // Check for overlaps
        collidingWithPlayer = false;
        boxCollider.OverlapCollider(filter, hits); // returns everything colliding with this boxCollider, filtered by filter
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
                continue;

            // Do something on collide (hit)
            if (hits[i].CompareTag("Fighter") && hits[i].name == "Player")
                collidingWithPlayer = true;

            // The array is not cleaned up, so we do it ourself
            hits[i] = null;
        }
    }

    protected override void Death()
    {
        hitbox.enabled = false;
        isAlive = false;
        deathSfx.Play();

        Destroy(gameObject, 1f); // Wait 1 sec before destroying
        GameManager.instance.GrantXp(xpValue);
        GameManager.instance.ShowText("+" + xpValue + " xp", 30, Color.magenta, transform.position, Vector3.up * 40, 1.0f);
    }
}
