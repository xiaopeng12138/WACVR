using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpectatorManager : MonoBehaviour
{
    CameraSmooth cameraSmooth;
    Camera SpectatorCam;
    public Transform SpectatorFPTarget;
    public Transform SpectatorTPTarget;

    void Start()
    {
        cameraSmooth = GetComponent<CameraSmooth>();
        SpectatorCam = GetComponent<Camera>();
        ConfigManager.onConfigChanged += ApplyConfig;
        ConfigManager.EnsureInitialization();
        ApplyConfig();
        ApplyTPCamTransform();
    }

    // Update is called once per frame
    void ApplyConfig()
    {
        if (SpectatorCam == null || cameraSmooth == null || SpectatorFPTarget == null || SpectatorTPTarget == null) 
            return;
        switch ((int)ConfigManager.config.SpectatorMode)
        {
            case 0:
                if (gameObject.activeSelf)
                    gameObject.SetActive(false);
                break;
            case 1:
                if (!gameObject.activeSelf)
                    gameObject.SetActive(true);
                cameraSmooth.target = SpectatorFPTarget;
                cameraSmooth.smoothSpeed = (float)ConfigManager.config.SpectatorSmooth;
                SpectatorCam.cullingMask |= 1 << LayerMask.NameToLayer("TPBlock"); // Enable TPBlock Layer Mask
                SpectatorCam.cullingMask &=  ~(1 << LayerMask.NameToLayer("FPBlock")); // Disable FPBlock Layer Mask
                break;
            case 2:
                if (!gameObject.activeSelf)
                    gameObject.SetActive(true);
                cameraSmooth.target = SpectatorTPTarget;
                cameraSmooth.smoothSpeed = 1;
                SpectatorCam.cullingMask &=  ~(1 << LayerMask.NameToLayer("TPBlock")); // Disable TPBlock Layer Mask
                SpectatorCam.cullingMask |= 1 << LayerMask.NameToLayer("FPBlock"); // Enable FPBlock Layer Mask
                break;
        }

        SpectatorCam.fieldOfView = (float)ConfigManager.config.SpectatorFOV;

        string fpsString = Enum.GetName(typeof(Config.captureFPS), ConfigManager.config.CaptureFPS);
        Application.targetFrameRate = int.Parse(fpsString.Remove(0, 3));
    }
    void ApplyTPCamTransform()
    {
        if (SpectatorTPTarget == null)
            return;
        SpectatorTPTarget.position = new Vector3(ConfigManager.config.TPCamPosition[0],
                                                ConfigManager.config.TPCamPosition[1],
                                                ConfigManager.config.TPCamPosition[2]);
        SpectatorTPTarget.rotation = Quaternion.Euler(ConfigManager.config.TPCamRotation[0],
                                                     ConfigManager.config.TPCamRotation[1],
                                                     ConfigManager.config.TPCamRotation[2]);
    }
    public void SaveTransform()
    {
        if (SpectatorTPTarget == null)
            return;
        ConfigManager.config.TPCamPosition[0] = SpectatorTPTarget.position.x;
        ConfigManager.config.TPCamPosition[1] = SpectatorTPTarget.position.y;
        ConfigManager.config.TPCamPosition[2] = SpectatorTPTarget.position.z;
        ConfigManager.config.TPCamRotation[0] = SpectatorTPTarget.rotation.eulerAngles.x;
        ConfigManager.config.TPCamRotation[1] = SpectatorTPTarget.rotation.eulerAngles.y;
        ConfigManager.config.TPCamRotation[2] = SpectatorTPTarget.rotation.eulerAngles.z;
        ConfigManager.SaveFile();
    }
}
