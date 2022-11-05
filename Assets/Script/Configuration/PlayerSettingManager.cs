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
        LHandTransform = transform.Find("Camera Offset").Find("LeftHand Controller").Find("LHand Virtual");
        RHandTransform = transform.Find("Camera Offset").Find("RightHand Controller").Find("RHand Virtual");

        var sizeWidget = ConfigManager.GetConfigPanelWidget("HandSize");
        var xWidget = ConfigManager.GetConfigPanelWidget("HandX");
        var yWidget = ConfigManager.GetConfigPanelWidget("HandY");
        var zWidget = ConfigManager.GetConfigPanelWidget("HandZ");
        var heightWidget = ConfigManager.GetConfigPanelWidget("PlayerHeight");

        var sizeSlider = sizeWidget.GetComponent<Slider>();
        var xSlider = xWidget.GetComponent<Slider>();
        var ySlider = yWidget.GetComponent<Slider>();
        var zSlider = zWidget.GetComponent<Slider>();
        var heightManager = heightWidget.GetComponent<ValueManager>();

        sizeSlider.onValueChanged.AddListener((float value) => {
            LHandTransform.localScale = new Vector3(value, value, value);
            RHandTransform.localScale = new Vector3(value, value, value);
        });
        xSlider.onValueChanged.AddListener((float value) => {
            LHandTransform.localPosition = new Vector3(value, LHandTransform.localPosition.y, LHandTransform.localPosition.z);
            RHandTransform.localPosition = new Vector3(-value, RHandTransform.localPosition.y, RHandTransform.localPosition.z);
        });
        ySlider.onValueChanged.AddListener((float value) => {
            LHandTransform.localPosition = new Vector3(LHandTransform.localPosition.x, value, LHandTransform.localPosition.z);
            RHandTransform.localPosition = new Vector3(RHandTransform.localPosition.x, value, RHandTransform.localPosition.z);
        });
        zSlider.onValueChanged.AddListener((float value) => {
            LHandTransform.localPosition = new Vector3(LHandTransform.localPosition.x, LHandTransform.localPosition.y, value);
            RHandTransform.localPosition = new Vector3(RHandTransform.localPosition.x, RHandTransform.localPosition.y, value);
        });
        heightManager.onValueChanged.AddListener(delegate {
            height = heightManager.value;
        });
        sizeSlider.onValueChanged.Invoke(sizeSlider.value);
        xSlider.onValueChanged.Invoke(xSlider.value);
        ySlider.onValueChanged.Invoke(ySlider.value);
        zSlider.onValueChanged.Invoke(zSlider.value);
        heightManager.onValueChanged.Invoke();
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
