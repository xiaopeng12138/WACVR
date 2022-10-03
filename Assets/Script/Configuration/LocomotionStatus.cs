using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocomotionStatus : MonoBehaviour
{
    private TextMeshPro text;
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        UpdateText(LocomotionToggle.IsEnabled);
    }

    public void UpdateText(bool status)
    {
        text.text = "Locomotion: " + (status ? "Enabled" : "Disabled");
    }
}
