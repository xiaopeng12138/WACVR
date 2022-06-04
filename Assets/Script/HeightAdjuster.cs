using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.Networking;
using System.Runtime.InteropServices;
using System;

public class HeightAdjuster : MonoBehaviour
{
    [SerializeField]
    private double height = 0; // meters
    [SerializeField]
    private double upperLimit = 10; // meters 
    [SerializeField]
    private double lowerLimit = -10; // meters 

    [Space]
    [SerializeField]
    private double adjustSpeed = 0.1; // meters per second

    [Header("Components")]
    [SerializeField]
    private PanelButton incrementButton;
    [SerializeField]
    private PanelButton decrementButton;
    [SerializeField]
    private PanelButton resetButton;
    [SerializeField]
    private TextMeshPro counterTxt;
    [SerializeField]
    private Transform XROrigin;

    private bool incrementing = false;
    private bool decrementing = false;

    void Start()
    {
        if (JsonConfiguration.HasKey("Height")) height = JsonConfiguration.GetDouble("Height");
        else SaveHeight();

        incrementButton.ButtonPressed += StartIncrementing;
        incrementButton.ButtonReleased += StopIncrementing;

        decrementButton.ButtonPressed += StartDecrementing;
        decrementButton.ButtonReleased += StopDecrementing;

        resetButton.ButtonPressed += ResetHeight;
    }

    void Update() 
    {
        if (incrementing) height += Time.deltaTime * adjustSpeed;
        if (decrementing) height -= Time.deltaTime * adjustSpeed;

        if (height > upperLimit) height = upperLimit;
        if (height < lowerLimit) height = lowerLimit;

        counterTxt.text = String.Format("{0:F2}m", height);
        XROrigin.position = new Vector3(XROrigin.position.x, (float) -height, XROrigin.position.z);
    }

    private void StartIncrementing() { incrementing = true; }
    private void StartDecrementing() { decrementing = true; }

    private void StopIncrementing() 
    { 
        incrementing = false; 
        SaveHeight();
    }
    private void StopDecrementing() 
    { 
        decrementing = false;
        SaveHeight();
    }

    private void ResetHeight() 
    { 
        height = 0;
        SaveHeight();
    }

    private void SaveHeight() {
        JsonConfiguration.SetDouble("Height", height);
    }
}
