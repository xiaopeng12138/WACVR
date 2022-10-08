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
        ConfigManager.onConfigChanged += ApplyConfig;
        ConfigManager.EnsureInitialization();
        ApplyConfig();
    }
    private void ApplyConfig() 
    {
        windowTexture.captureMode = (CaptureMode)ConfigManager.config.CaptureMode - 1;

        var fps = Enum.GetName(typeof(Config.captureFPS), ConfigManager.config.CaptureFPS);
        windowTexture.captureFrameRate = int.Parse(fps.Remove(0, 3));

        if (ConfigManager.config.CaptureDesktop)
        {
            windowTexture.type = WindowTextureType.Desktop;
            windowTexture.desktopIndex = ConfigManager.config.CaptureDesktopNumber;
        }
        else
            windowTexture.type = WindowTextureType.Window;
    }
}
