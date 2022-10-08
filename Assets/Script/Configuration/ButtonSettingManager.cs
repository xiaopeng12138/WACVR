using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindowsInput.Native;
using System;

public class ButtonSettingManager : MonoBehaviour
{
    public ButtonType buttonType;
    private PanelButton panelButton;
    void Start()
    {
        panelButton = GetComponent<PanelButton>();
        ConfigManager.onConfigChanged += ApplyConfig;
        ConfigManager.EnsureInitialization();
        ApplyConfig();
    }
    void ApplyConfig()
    {
        switch (buttonType)
        {
            case ButtonType.Test:
                panelButton.key = ConfigManager.config.TestKey;
                break;
            case ButtonType.Service:
                panelButton.key = ConfigManager.config.ServiceKey;
                break;
            case ButtonType.Coin:
                panelButton.key = ConfigManager.config.CoinKey;
                break;
            case ButtonType.Custom:
                panelButton.key = ConfigManager.config.CustomKey;
                break;
        }
    }

    public enum ButtonType
    {
        Test = 0,
        Service = 1,
        Coin = 2,
        Custom = 3,
    }

}
