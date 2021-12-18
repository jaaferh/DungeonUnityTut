using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : Fighter
{
    protected BoxCollider2D boxCollider;
    protected Vector3 moveDelta;
    protected RaycastHit2D hit;
    private Vector3 originalSize;
    private float ySpeed = 0f;
    private float xSpeed = 0f;
    public float maxSpeed = 2.0f;
    public float xAccel = 1f;
    public float yAccel = 1f;
    public float reAccelFactor = 3f;

    protected virtual void Start() 
    {
        originalSize = transform.localScale;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void UpdateMotor(Vector3 input)
    {
        CalculateSpeed(input);

        // Reset moveDelta
        moveDelta = new Vector3(xSpeed, ySpeed, 0);

        // Swap sprite direction, whether youre going right or left
        if (input.x > 0)
            transform.localScale = originalSize;
        else if (input.x < 0)
            transform.localScale = new Vector3(originalSize.x * -1, originalSize.y, originalSize.z);

        // Add push vector, if any
        moveDelta += pushDirection;

        // Reduce push force every frame, based on recovery speed by linear interpolation
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);

        // Can move in this direction by casting a box there first. If box == null, we can move
        // y axis
        hit = Physics2D.BoxCast(
            transform.position, 
            boxCollider.size, 
            0, 
            new Vector2(0, moveDelta.y), 
            Mathf.Abs(moveDelta.y * Time.deltaTime), 
            LayerMask.GetMask("Actor", "Blocking")
        );
        if (hit.collider == null)
        {
            // Make player move
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }
        else
        {
            // Drop Y Speed to 0
            ySpeed = 0;
        }

        // x axis
        hit = Physics2D.BoxCast(
            transform.position, 
            boxCollider.size, 
            0, 
            new Vector2(moveDelta.x, 0), 
            Mathf.Abs(moveDelta.x * Time.deltaTime), 
            LayerMask.GetMask("Actor", "Blocking")
        );
        if (hit.collider == null)
        {
            // Make player move
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }
        else
        {
            // Drop X Speed to 0
            xSpeed = 0;
        }
    }

    private void CalculateSpeed(Vector3 input)
    {
        if (input.x > 0)
        {
            xSpeed += xAccel * Time.deltaTime;
            xSpeed = xSpeed > maxSpeed ? maxSpeed : xSpeed;
        }
        else if (input.x < 0)
        {
            xSpeed -= xAccel * Time.deltaTime;
            xSpeed = xSpeed < -maxSpeed ? -maxSpeed : xSpeed;
        }
        else // Input 0
        {
            // Decelerate x for Right (input +1)
            if (xSpeed > 0)
            {
                xSpeed -= xAccel * Time.deltaTime * reAccelFactor; // Decrease xSpeed till it's 0. reAccelFactor allows for faster direction switching
                xSpeed = xSpeed < 0 ? 0 : xSpeed; // Less than 0 is clamped to 0
            }
            // Decelerate x for Left (input -1)
            else if (xSpeed < 0)
            {
                xSpeed += xAccel * Time.deltaTime * reAccelFactor; // Increase -xSpeed till it's 0
                xSpeed = xSpeed > 0 ? 0 : xSpeed;  // More than 0 is clamped to 0
            }
        }

        if (input.y > 0)
        {
            ySpeed += yAccel * Time.deltaTime;
            ySpeed = ySpeed > maxSpeed ? maxSpeed : ySpeed;
        }
        else if (input.y < 0)
        {
            ySpeed -= yAccel * Time.deltaTime;
            ySpeed = ySpeed < -maxSpeed ? -maxSpeed : ySpeed;
        }
        else // Input 0
        {
            // Decelerate y for Right (input +1)
            if (ySpeed > 0)
            {
                ySpeed -= yAccel * Time.deltaTime * reAccelFactor;
                ySpeed = ySpeed < 0 ? 0 : ySpeed;
            }
            // Decelerate y for Left (input -1)
            else if (ySpeed < 0)
            {
                ySpeed += yAccel * Time.deltaTime * reAccelFactor;
                ySpeed = ySpeed > 0 ? 0 : ySpeed;
            }
        }
    }
}
