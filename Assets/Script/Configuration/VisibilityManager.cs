using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityManager : MonoBehaviour
{
    public string ConfigKeyName;
    void Start()
    {
        ConfigManager.onConfigChanged += ApplyConfig;
        ConfigManager.EnsureInitialization();
        ApplyConfig();
    }
    void ApplyConfig()
    {
        bool state = (bool)ConfigManager.config.GetType().GetField(ConfigKeyName).GetValue(ConfigManager.config);
        gameObject.SetActive(state);
    }
}
