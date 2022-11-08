using TMPro;
using UnityEngine;
using uWindowCapture;
using UnityEngine.UI;
using System;

public class CaptureSettingManager : MonoBehaviour
{
    public UwcWindowTexture windowTexture;
    private void Start()
    {
        windowTexture = GetComponent<UwcWindowTexture>();
        var modeWidget = ConfigManager.GetConfigPanelWidget("CaptureMode");
        var fpsWidget = ConfigManager.GetConfigPanelWidget("CaptureFPS");
        var desktopWidget = ConfigManager.GetConfigPanelWidget("CaptureDesktop");
        var desktopIndexWidget = ConfigManager.GetConfigPanelWidget("DesktopIndex");

        var modeDropdown = modeWidget.GetComponent<TMP_Dropdown>();
        var fpsDropdown = fpsWidget.GetComponent<TMP_Dropdown>();
        var desktopToggle = desktopWidget.GetComponent<Toggle>();
        var desktopIndexDropdown = desktopIndexWidget.GetComponent<TMP_Dropdown>();

        modeDropdown.onValueChanged.AddListener((int value) => {
            windowTexture.captureMode = (CaptureMode)value - 1;
            Debug.Log("CaptureFPS: " + windowTexture.captureMode);
        });

        fpsDropdown.onValueChanged.AddListener((int value) => {
            var fps = Enum.GetName(typeof(CEnum.FPS), value);
            windowTexture.captureFrameRate = int.Parse(fps.Remove(0, 3));
            Debug.Log("CaptureFPS: " + windowTexture.captureFrameRate);
        });

        desktopToggle.onValueChanged.AddListener((bool value) => {
            if (value)
            {

                windowTexture.type = WindowTextureType.Desktop;
                desktopIndexDropdown.interactable = true;
            }
            else
            {
                windowTexture.type = WindowTextureType.Window;
                desktopIndexDropdown.interactable = false;
            }
            Debug.Log("CaptureTarget: " + windowTexture.type);
        });

        desktopIndexDropdown.onValueChanged.AddListener((int value) => {
            windowTexture.desktopIndex = value;
            Debug.Log("DesktopIndex: " + windowTexture.desktopIndex);
        });
        modeDropdown.onValueChanged?.Invoke(modeDropdown.value);
        fpsDropdown.onValueChanged?.Invoke(fpsDropdown.value);
        desktopToggle.onValueChanged?.Invoke(desktopToggle.isOn);
        desktopIndexDropdown.onValueChanged?.Invoke(desktopIndexDropdown.value);
    }
}
