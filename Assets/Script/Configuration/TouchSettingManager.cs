using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TouchSettingManager : MonoBehaviour
{
    void Start()
    {
        ConfigManager.onConfigChanged += ApplyConfig;
        ConfigManager.EnsureInitialization();
        ApplyConfig();
    }
    public void ApplyConfig()
    {
        string fpsString = Enum.GetName(typeof(Config.captureFPS), ConfigManager.config.CaptureFPS);
        Time.fixedDeltaTime = 1 / int.Parse(fpsString.Remove(0, 3));
    }
}
