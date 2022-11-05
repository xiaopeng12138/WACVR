using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TouchSettingManager : MonoBehaviour
{
    void Start()
    {
        var sampleWidget = ConfigManager.GetConfigPanelWidget("TouchSampleRate");

        var sampleDropdown = sampleWidget.GetComponent<TMP_Dropdown>();

        sampleDropdown.onValueChanged.AddListener((int value) => {
            string fpsString = Enum.GetName(typeof(CEnum.FPS), value);
            Time.fixedDeltaTime = 1 / int.Parse(fpsString.Remove(0, 3));
        });
        
        sampleDropdown.onValueChanged?.Invoke(sampleDropdown.value);
    }
}
