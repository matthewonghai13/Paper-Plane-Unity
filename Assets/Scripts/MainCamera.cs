using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{

    private Camera mainCam;
    private Camera pixelCam;
    private float initialSize;
    private const float desiredAspect = 2f / 3f;
    private float desiredSize;

    // Start is called before the first frame update
    void Start() {
        QualitySettings.vSyncCount = 1; // set vsync to 1:1 panel refresh rate
        mainCam = GetComponent<Camera>();
        initialSize = Camera.main.orthographicSize;

        if(mainCam.aspect < 1.6f) {
            // respect width
            mainCam.orthographicSize = initialSize * (desiredAspect / mainCam.aspect);
        } else {
            mainCam.orthographicSize = initialSize;
        }
        desiredSize = mainCam.orthographicSize;
    }

    private void Update() {
        //mainCam.orthographicSize = desiredSize;
    }
}
