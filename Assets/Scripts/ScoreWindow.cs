using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreWindow : MonoBehaviour{
      
    private Text scoreText;
    private Text currentHighScoreText;

    private void Awake(){
        scoreText = transform.Find("ScoreText").GetComponent<Text>();
        currentHighScoreText = transform.Find("CurrentHighScore").GetComponent<Text>();
    }

    private void Update(){
        scoreText.text = Level.GetInstance().getFloorsPassed().ToString();
        if(PlayerPrefs.HasKey("hs")) {
            currentHighScoreText.text = "High Score: " + PlayerPrefs.GetInt("hs").ToString();
        } else {
            currentHighScoreText.text = "High Score: 0";
        }
        
    }
}
