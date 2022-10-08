using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public class PlayerSettingManager : MonoBehaviour
{
    private Transform LHandTransform = null;
    private Transform RHandTransform = null;
    private double height = 0; // meters
    [SerializeField]
    private double upperLimit = 10; // meters 
    [SerializeField]
    private double lowerLimit = -10; // meters 
    void Start()
    {
        LHandTransform = transform.Find("Camera Offset").Find("LeftHand Controller").Find("LHand");
        RHandTransform = transform.Find("Camera Offset").Find("RightHand Controller").Find("RHand");
        ConfigManager.onConfigChanged += ApplyConfig;
        ConfigManager.EnsureInitialization();
        ApplyConfig();
    }
    void ApplyConfig()
    {
        LHandTransform.localPosition = new Vector3(ConfigManager.config.HandPosition[0]/100, 
                                                    ConfigManager.config.HandPosition[1]/100,
                                                    ConfigManager.config.HandPosition[2]/100);
        RHandTransform.localPosition = new Vector3(-ConfigManager.config.HandPosition[0]/100,
                                                    ConfigManager.config.HandPosition[1]/100, 
                                                    ConfigManager.config.HandPosition[2]/100);

        var value = ConfigManager.config.HandSize;
        LHandTransform.localScale = new Vector3(value/100, value/100, value/100);
        RHandTransform.localScale = new Vector3(value/100, value/100, value/100);
        
        height = ConfigManager.config.PlayerHeight;
    }
    void Update() 
    {
        if (height > upperLimit) height = upperLimit;
        if (height < lowerLimit) height = lowerLimit;
        transform.position = new Vector3(transform.position.x, (float)height, transform.position.z);
    }
    private void ResetHeight() 
    { 
        height = 0;
    }
}
