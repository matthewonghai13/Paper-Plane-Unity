using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class GameOverWindow : MonoBehaviour
{
    private Text FinalScoreText;
    private Text HighScoreText;
    private static GameOverWindow instance;
    private string placementId = "GameOver";
    private string gameId = "3433506";
    private bool testMode = false;

    private void Awake() {
        instance = this;
        FinalScoreText = transform.Find("FinalScoreText").GetComponent<Text>();
        HighScoreText = transform.Find("HighScoreText").GetComponent<Text>();

        // init ads
        Advertisement.Initialize(gameId, testMode);
        //StartCoroutine(ShowBannerWhenReady());

        // retry button
        Button btn = transform.Find("RetryButton").GetComponent<Button>();
        btn.onClick.AddListener(RestartLevel);
        Hide();
    }

    IEnumerator ShowBannerWhenReady() {
        while (!Advertisement.IsReady(placementId)) {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.Show(placementId);
    }


    public static GameOverWindow GetInstance() {
        return instance;
    }

    // handles calling back to level
    public void RestartLevel() {
        Debug.Log("restart level!");
        // remove ad


        // restart level
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    // called by level when the game is over
    public void DisplayGameOverScreen() {
        int thisScore = Level.GetInstance().getFloorsPassed();
        FinalScoreText.text = thisScore.ToString();
        // set hs
        if (!PlayerPrefs.HasKey("hs") || thisScore > PlayerPrefs.GetInt("hs")) {
            // set new hs
            HighScoreText.text = "New High Score! ";
            // save new hs
            PlayerPrefs.SetInt("hs", thisScore);
            PlayerPrefs.Save();
        } else {
            HighScoreText.text = "";
        }
        Show();
    }

    // used for showing and hiding the game over menu
    private void Hide() {
        bool destroy = false;
        Advertisement.Banner.Hide(destroy);
        gameObject.SetActive(false);
    }

    private void Show() {
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Show(placementId);
        gameObject.SetActive(true);
    }
}
