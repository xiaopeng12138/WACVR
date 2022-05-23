using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WindowsInput.Native;

public class ControlPanel : MonoBehaviour
{
    public Transform Button;
    [DllImport("user32.dll")]
    public static extern uint MapVirtualKey(uint uCode, uint uMapType);
    [DllImport("user32.dll")]
    static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
    public VirtualKeyCode key;
    public VirtualKeyCode key2;
    private int _insideColliderCount = 0;
    public bool isToggle;
    public bool isOn;
    private Renderer cr;
    public GameObject camera;
    // Start is called before the first frame update


    void Start()
        {
            cr = GetComponent<Renderer>();
        }


    private void OnTriggerEnter(Collider other)
    {
        if (isToggle == true)
        {
            if(isOn == false)
            {
                isOn = true;
                cr.material.color = new Color(0f, 1f, 0f, 1f);
                _insideColliderCount += 1;
                keybd_event(System.Convert.ToByte(key), (byte)MapVirtualKey((uint)key, 0), 0, UIntPtr.Zero);
            } else if (isOn == true)
            {
                isOn = false;
                cr.material.color = new Color(1f, 0f, 0f, 1f);
                _insideColliderCount += 1;
                keybd_event(System.Convert.ToByte(key2), (byte)MapVirtualKey((uint)key2, 0), 0, UIntPtr.Zero);
            }

        } else if (isToggle == false)
        {
            _insideColliderCount += 1;
            keybd_event(System.Convert.ToByte(key), (byte)MapVirtualKey((uint)key, 0), 0, UIntPtr.Zero);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _insideColliderCount -= 1;
        _insideColliderCount = Mathf.Max(0, _insideColliderCount);
        if (_insideColliderCount == 0)
            keybd_event(System.Convert.ToByte(key), (byte)MapVirtualKey((uint)key, 0), 2, UIntPtr.Zero);
    }

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Home))
        {
            camera.SetActive(true);
        }
        
        if (Input.GetKeyDown(KeyCode.End))
        {
            camera.SetActive(false);
        }

    }
}
