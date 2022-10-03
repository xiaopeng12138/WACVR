using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSettingManager : MonoBehaviour
{
    public List<GameObject> Lights;
    void Start()
    {
        ConfigManager.onConfigChanged += ApplyConfig;
        ConfigManager.EnsureInitialization();
        ApplyConfig();
    }
    void ApplyConfig()
    {
        foreach (var light in Lights)
        {
            light.SetActive(ConfigManager.config.useLight);
        }
    }
}
