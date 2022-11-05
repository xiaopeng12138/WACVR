using UnityEngine;
using System;
using TMPro;
using WindowsInput.Native;

public class ButtonSettingManager : MonoBehaviour
{
    public ButtonType buttonType;
    private PanelButton panelButton;
    void Start()
    {
        panelButton = GetComponent<PanelButton>();
        var widget = ConfigManager.GetConfigPanelWidget(Enum.GetName(typeof(ButtonType), buttonType));
        var dropdown = widget.GetComponent<TMP_Dropdown>();
        dropdown.onValueChanged.AddListener((int value) => {
            panelButton.key = (VirtualKeyCode)Enum.GetValues(typeof(VirtualKeyCode)).GetValue(value);
        });
        dropdown.onValueChanged?.Invoke(dropdown.value);
    }

    public enum ButtonType
    {
        TestKeyBind = 0,
        ServiceKeyBind = 1,
        CoinKeyBind = 2,
        CustomKeyBind = 3,
    }

}
