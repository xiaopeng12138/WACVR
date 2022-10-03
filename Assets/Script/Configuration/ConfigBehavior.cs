using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigBehavior : MonoBehaviour
{
    public static ConfigBehavior instance;
    void Awake() 
    {
        instance = this;
    }
    public static void SaveFile()
    {
        instance.StopCoroutine(ConfigManager.SaveFileWait());
        instance.StartCoroutine(ConfigManager.SaveFileWait());
    }
}
