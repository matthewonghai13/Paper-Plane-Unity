using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour
{
    private Text FinalScoreText;
    private static GameOverWindow instance;

    private void Awake() {
        instance = this;
        FinalScoreText = transform.Find("FinalScoreText").GetComponent<Text>();

        // TODO: find the button and make it reload the game on click
        Button btn = transform.Find("RetryButton").GetComponent<Button>();
        btn.onClick.AddListener(RestartLevel);
        Hide();
    }

    public static GameOverWindow GetInstance() {
        return instance;
    }

    // handles calling back to level
    private void RestartLevel() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    // called by level when the game is over
    public void DisplayGameOverScreen(){
        FinalScoreText.text = Level.GetInstance().getFloorsPassed().ToString();
        Show();
    }

    // used for showing and hiding the game over menu
    private void Hide() {
        gameObject.SetActive(false);
    }

    private void Show() {
        gameObject.SetActive(true);
    }
}
