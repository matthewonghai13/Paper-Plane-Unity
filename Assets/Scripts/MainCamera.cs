using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{

    private Camera mainCam;
    private float initialSize;
    private const float desiredAspect = 2f / 3f;

    // Start is called before the first frame update
    void Start() {
        mainCam = GetComponent<Camera>();
        initialSize = Camera.main.orthographicSize;

        if(mainCam.aspect < 1.6f) {
            // respect width
            mainCam.orthographicSize = initialSize * (desiredAspect / mainCam.aspect);
        } else {
            mainCam.orthographicSize = initialSize;
        }
    }
}
