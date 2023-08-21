using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footsteps : MonoBehaviour
{
    public AudioSource footstepsSound;
    public float speedMultiplier = 1.5f;

    private void Start()
    {
        footstepsSound.enabled = true;
    }

    void Update()
    {

        bool isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        if (isMoving)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
   
                footstepsSound.enabled = true;
                footstepsSound.pitch = speedMultiplier;
            }
            else
            {

                footstepsSound.enabled = true;
                footstepsSound.pitch = 0.8f;
            }
        }
        else

            footstepsSound.enabled = false;
        }
    }

