using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetryButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // handles calling back to level
    public void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}
