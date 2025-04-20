using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearFreeze : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public float freezeDuration = 5f;
    public int pressesRequired = 3;
    public KeyCode unfreezeButton = KeyCode.R;

    public bool isFrozen = false;

    private int presses = 0;

    private void Start()
    {
        //Start the fear freeze timer
        InvokeRepeating("CheckForFearFreeze", 1f, 5f);
    }

    private void Update()
    {
        //If the player is frozen and presses the unfreeze button
        if (isFrozen && Input.GetKeyDown(unfreezeButton))
        {
            //Increment the number of presses
            presses++;

            //If the player had pressed the button anough times, unfreeze them
            if (presses >= pressesRequired) 
            {
                UnfreezePlayer();
            }
        }
    }

    private void CheckForFearFreeze()
    {
        //Randomly decide whether to freeze the player
        if (Random.value < 0.7f && !isFrozen)
        {
            FreezePlayer();
        }
    }
    private void FreezePlayer()
    {
     //Freeze the player's movement
     playerMovement.enabled= false;

     //Set the frozen flag
     isFrozen = true;

     //Reset the number of presses
     presses= 0;

        //Display a message to the player 
        Debug.Log("You've been frozen by fear! Press the R button three time to unfreeze.");
        //Start the freezer timer
        StartCoroutine(FreezeTimer());
    }
    private IEnumerator FreezerTimer()
    {
        yield return new WaitForSeconds(freezeDuration);
        UnfreezePlayer();
    }

       

    private void UnfreezePlayer()
    {
        //Unfreeze the Player's movement
        playerMovement.enabled= true;

        //Reset the frozen flag
        isFrozen= false;

        //Display a message to the player
        Debug.Log("Chill Out");

        //Cancel any remaining freeze timer
        CancelInvoke("UnfreezePlayerAutomatically");
    }

    private void UnfreezePlayerAutomatically()
    {
        //Unfreeze the player automatically after the freeze duration
        UnfreezePlayer();
    }

    private IEnumerator FreezeTimer()
    {
        yield return new WaitForSeconds(freezeDuration);
        UnfreezePlayerAutomatically();
    }

    private void OnEnable()
    {
        //Start the freeze timer when the player is frozen
        if (isFrozen)
        {
            StartCoroutine (FreezeTimer());
        }
    }

    private void FreezePlayerForDuration()
    {
        //Freeze the player for a set duration 
        StartCoroutine(FreezeTimer());
    }

    private void OnFreeze()
    {
        FreezePlayerForDuration();
    }

}
