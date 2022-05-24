using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocomotionStatus : MonoBehaviour
{
    private TextMeshPro text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        UpdateText(LocomotionToggle.IsEnabled);
    }

    public void UpdateText(bool status)
    {
        text.text = "LOCOMOTION: " + (status ? "ENABLED" : "DISABLED");
    }
}
