using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.IO.Ports;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class TouchManager : MonoBehaviour
{
    public static event Action touchDidChange;
    static bool useIPCTouch = true;
    void Start()
    {
        var widget = ConfigManager.GetConfigPanelWidget("UseIPCTouch");
        var toggle = widget.GetComponent<Toggle>();
        toggle.onValueChanged.AddListener((value) => {
            useIPCTouch = value;
            Debug.Log("UseIPCTouch: " + value);
        });
        toggle.onValueChanged.Invoke(toggle.isOn);
    }

    IEnumerator TouchTest(bool State) //this is a touch test code
    {
        for (int i = 0; i < 240; i++)
        {
            SetTouch(i, true);
            Debug.Log(i);
            yield return new WaitForSeconds(0.05f);
            SetTouch(i, false);
            yield return new WaitForSeconds(0.05f);
        }
    }
    public static void SetTouch(int Area, bool State) //set touch data 1-240
    {
        Debug.Log("SetTouch: " + Area + " " + State + " " + useIPCTouch);
        if (useIPCTouch)
            IPCManager.SetTouch(Area, State); //send touch data to IPC
        else
            SerialManager.SetTouch(Area, State); //send touch data to Serial
        
        touchDidChange?.Invoke();
    }
}