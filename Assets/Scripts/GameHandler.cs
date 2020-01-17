using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class GameHandler : MonoBehaviour {
    

    private void Start() {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
    }
}
