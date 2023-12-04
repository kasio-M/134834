using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Add this for IEnumerator

public class BuildingInteraction : MonoBehaviour
{
    public GameObject uiCanvas;
    public Text buildingInfoText;
    public Text questionText;
    public InputField answerInputField;
    public Text scoreText;

    private int score = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(DisplayBuildingInfoWithDelay());
        }
    }

    IEnumerator DisplayBuildingInfoWithDelay()
    {
        uiCanvas.SetActive(true);

        // Set information about the building.
        buildingInfoText.text = "This is a hospital. It is known for...";

        // Display building information for 10 seconds.
        yield return new WaitForSeconds(10f);

        // After 10 seconds, set questions for the player.
        questionText.text = "Who is known to work in a hospital?";
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
