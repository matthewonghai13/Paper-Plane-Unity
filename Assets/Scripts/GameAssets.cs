﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{

    private static GameAssets instance;

    public static GameAssets GetInstance(){
        return instance;
    }

    private void Awake(){
        instance = this;
    }

    public Sprite brickSprite;
    public Transform pfBrick;
    public Transform pfbgBrick;
}
