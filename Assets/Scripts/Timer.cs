using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float timeRemaining = 90f; // Set the timer duration in seconds
    [SerializeField] private GameEnding gameEnding;
    private bool gameOverTriggered = false;
    public GameObject timerImage;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else if (!gameOverTriggered)
        {
            timeRemaining = 0;
            timerText.text = "00:00";
            timerImage.SetActive(false);
            gameEnding.CaughtPlayer();
            gameOverTriggered = true;
        }
    }
}
