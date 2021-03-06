﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour
{
    private Text FinalScoreText;
    private Text HighScoreText;
    private static GameOverWindow instance;

    private void Awake() {
        instance = this;
        FinalScoreText = transform.Find("FinalScoreText").GetComponent<Text>();
        HighScoreText = transform.Find("HighScoreText").GetComponent<Text>();


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
    public void DisplayGameOverScreen() {
        int thisScore = Level.GetInstance().getFloorsPassed();
        FinalScoreText.text = thisScore.ToString();
        if (!PlayerPrefs.HasKey("hs") || thisScore > PlayerPrefs.GetInt("hs")) {
            // set new hs
            HighScoreText.text = "New High Score: " + thisScore.ToString();
            // save new hs
            PlayerPrefs.SetInt("hs", thisScore);
        } else {
            HighScoreText.text = "";
        }
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
