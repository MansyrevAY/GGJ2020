﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinAnimationEvent : MonoBehaviour
{
    public void LoadStart()
    {
        SceneManager.LoadScene(0);
    }
}
