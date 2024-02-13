using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : Singleton<MainMenuManager>
{
    private void Start()
    {
        Application.targetFrameRate = 30;
    }
}