using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    private Transform lookAt; // the player
    public float boundX = 0.15f;
    public float boundY = 0.05f;

    private void Start()
    {
        lookAt = GameObject.Find("Player").transform;
    }

    // LateUpdate() is called after Update() and FixedUpdate()
    private void LateUpdate() {
        Vector3 delta = Vector3.zero; // Translation of camera

        // Calculate camera translation for X axis
        float deltaX = lookAt.position.x - transform.position.x; // player pos - camera.pos
        if (deltaX > boundX || deltaX < -boundX) // if distance between player and camera is more than boundX (right) || less than -boundX (left)
        {
            if (transform.position.x < lookAt.position.x) // right
            {
                delta.x = deltaX - boundX; // move by the amount overflowing past boundX
            }
            else // left
            {
                delta.x = deltaX + boundX; // move by the amount overflowing past boundX (negative val)
            }
        }

        // Calculate camera translation for Y axis
        float deltaY = lookAt.position.y - transform.position.y; // player pos - camera.pos
        if (deltaY > boundY || deltaY < -boundY) // if distance between player and camera is more than boundY (right) || less than -boundY (left)
        {
            if (transform.position.y < lookAt.position.y) // right
            {
                delta.y = deltaY - boundY; // move by the amount overflowing past boundY
            }
            else // left
            {
                delta.y = deltaY + boundY; // move by the amount overflowing past boundY (negative val)
            }
        }

        transform.position += new Vector3(delta.x, delta.y, 0); // transform camera using vector3 translation
    }
}
