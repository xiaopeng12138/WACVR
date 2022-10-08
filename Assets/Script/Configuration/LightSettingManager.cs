using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSettingManager : MonoBehaviour
{
    public GameObject LightManager;
    void Start()
    {
        ConfigManager.onConfigChanged += ApplyConfig;
        ConfigManager.EnsureInitialization();
        ApplyConfig();
    }
    void ApplyConfig()
    {
            LightManager.SetActive(ConfigManager.config.useLight);
    }
}
