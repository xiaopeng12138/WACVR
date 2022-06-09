using UnityEngine;
using uWindowCapture;

public class UwcConfigurator : MonoBehaviour {
    private UwcWindowTexture uwcWindowTexture;

    void Start() {
        uwcWindowTexture = GetComponent<UwcWindowTexture>();
        
        if (JsonConfiguration.HasKey("CaptureMode")) {
            int rawCaptureMode = JsonConfiguration.GetInt("CaptureMode");

            if (rawCaptureMode > 3 || rawCaptureMode < 0) {
                JsonConfiguration.SetInt("CaptureMode", (int) uwcWindowTexture.captureMode);
            } else 
                uwcWindowTexture.captureMode = (CaptureMode) JsonConfiguration.GetInt("CaptureMode");
        } else 
            JsonConfiguration.SetInt("CaptureMode", (int) uwcWindowTexture.captureMode);

        if (JsonConfiguration.HasKey("CaptureFramerate")) 
            uwcWindowTexture.captureFrameRate = JsonConfiguration.GetInt("CaptureFramerate");
        else 
            JsonConfiguration.SetInt("CaptureFramerate", uwcWindowTexture.captureFrameRate);

        if (!JsonConfiguration.HasKey("CaptureDesktopNumber"))
            JsonConfiguration.SetInt("CaptureDesktopNumber", 0);

        if (JsonConfiguration.HasKey("CaptureDesktop") && JsonConfiguration.GetBoolean("CaptureDesktop")) 
            SwitchToDesktopCapture();
        else 
            JsonConfiguration.SetBoolean("CaptureDesktop", false);
    }

    void SwitchToDesktopCapture() {
        uwcWindowTexture.type = WindowTextureType.Desktop;
        uwcWindowTexture.desktopIndex = JsonConfiguration.GetInt("CaptureDesktopNumber");
    }
}
