using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;
using System;
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
    }
}
