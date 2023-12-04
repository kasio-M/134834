using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LandmarkInteraction : MonoBehaviour
{
    public GameObject uiCanvas;
    public Text buildingInfoText;
    public Text questionText;
    public InputField answerInputField;
    public Text scoreText;

   private int score = 0;
    private bool isInsideCollider = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInsideCollider = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInsideCollider = false;
        }
    }

    void Update()
    {
        if (isInsideCollider && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(DisplayBuildingInfoWithDelay());
        }
    }

    IEnumerator DisplayBuildingInfoWithDelay()
    {
        uiCanvas.SetActive(true);

        // Set information about the building.
        buildingInfoText.text = "this is a police station wheresuspected criminals are kept temporary awaiting trial ";

        // Display building information for 10 seconds.
        yield return new WaitForSeconds(10f);

        // After 10 seconds, set questions for the player.
        questionText.text = "who do you find most working at the police station?";
    }

    public void CheckAnswer()
    {
        string playerAnswer = answerInputField.text.ToLower();

        if (playerAnswer.Contains("doctor"))
        {
            // Correct answer feedback.
            Debug.Log("Correct! Doctors are known to work in hospitals.");
            AddPoints(10);
        }
        else
        {
            // Incorrect answer feedback.
            Debug.Log("Incorrect! Please try again.");
        }
    }

    void AddPoints(int points)
    {
        score += points;
        UpdateScoreDisplay();
    }

    void UpdateScoreDisplay()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}