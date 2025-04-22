using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FearFreeze : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public float freezeDuration = 5f;
    public int pressesRequired = 3;
    public KeyCode unfreezeButton = KeyCode.R;
    public Image normalImage;
    public Image freezeImage;
    public TMP_Text freezeText;

    public bool isFrozen = false;

    private int presses = 3;

    private void Start()
    {
        //Start the fear freeze timer
        InvokeRepeating("CheckForFearFreeze", 1f, 5f);

        normalImage.enabled = true;
        freezeImage.enabled = false;
        freezeText.enabled = false;
   
    }

    private void Update()
    {
        //If the player is frozen and presses the unfreeze button
        if (isFrozen && Input.GetKeyDown(KeyCode.R))
        {

            presses++;
            if (presses>= pressesRequired) 
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

        normalImage.enabled= false;
        freezeImage.enabled = true;
        freezeText.enabled = true;
        freezeText.text = "You're frozen in fear! Press the R button three times to unfreeze!";

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


        freezeImage.enabled= false;
        normalImage.enabled = true;
        freezeText.enabled= false;
        
    }
}
