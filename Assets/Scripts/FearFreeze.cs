using System.Collections;
using UnityEngine;
using TMPro;
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
        InvokeRepeating("CheckForFearFreeze", 1f, 5f);

        normalImage.enabled = true;
        freezeImage.enabled = false;
        freezeText.enabled = false;
    }

    private void Update()
    {
        if (isFrozen && Input.GetKeyDown(KeyCode.R))
        {
            presses++;
            StartCoroutine(ShakeFreezeImage(0.15f, 5f));

            if (presses >= pressesRequired)
            {
                UnfreezePlayer();
            }
        }
    }

    private void CheckForFearFreeze()
    {
        if (Random.value < 0.7f && !isFrozen)
        {
            FreezePlayer();
        }
    }

    private void FreezePlayer()
    {
        //Set the freeze flag
        isFrozen = true;
        presses = 0;

        normalImage.enabled = false;
        freezeImage.enabled = true;
        freezeText.enabled = true;
        freezeText.text = "You're frozen in fear! Press the R button three times to unfreeze!";
    }

    private void UnfreezePlayer()
    {
        isFrozen = false;

        freezeImage.enabled = false;
        normalImage.enabled = true;
        freezeText.enabled = false;
    }

    IEnumerator ShakeFreezeImage(float duration, float magnitude)
    {
        Vector3 originalPos = freezeImage.rectTransform.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            freezeImage.rectTransform.anchoredPosition = originalPos + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        freezeImage.rectTransform.anchoredPosition = originalPos;
    }
}
