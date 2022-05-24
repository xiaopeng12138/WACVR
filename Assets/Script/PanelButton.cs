using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WindowsInput.Native;

[RequireComponent(typeof(AudioSource))]
public class PanelButton : MonoBehaviour
{
    [DllImport("user32.dll")]
    public static extern uint MapVirtualKey(uint uCode, uint uMapType);
    [DllImport("user32.dll")]
    static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

    public VirtualKeyCode key;
    public VirtualKeyCode key2;

    public bool isToggle;
    public bool isOn;
    private int _insideColliderCount = 0;

    private Renderer cr;
    public GameObject camera;
    private AudioSource audioSrc;
    private static AudioClip btnSound;

    void Start()
    {
        btnSound = Resources.Load<AudioClip>("Audio/button press");
        cr = GetComponent<Renderer>();

        audioSrc = GetComponent<AudioSource>();
        audioSrc.playOnAwake = false;
        audioSrc.Stop();
        audioSrc.clip = btnSound;

        if (isToggle)
        {
            // initialize toggle state
            OnTriggerEnter(new Collider());
            OnTriggerExit(new Collider());
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        _insideColliderCount += 1;
        audioSrc.Play();
        if (isToggle)
        {
            if(!isOn)
            {
                cr.material.color = Color.green;
                keybd_event(System.Convert.ToByte(key2), (byte)MapVirtualKey((uint)key2, 0), 2, UIntPtr.Zero);
                keybd_event(System.Convert.ToByte(key), (byte)MapVirtualKey((uint)key, 0), 0, UIntPtr.Zero);
                isOn = true;
            }
            else
            {
                cr.material.color = Color.red;
                keybd_event(System.Convert.ToByte(key), (byte)MapVirtualKey((uint)key, 0), 2, UIntPtr.Zero);
                keybd_event(System.Convert.ToByte(key2), (byte)MapVirtualKey((uint)key2, 0), 0, UIntPtr.Zero);
                isOn = false;
            }

        }
        else
        {
            keybd_event(System.Convert.ToByte(key), (byte)MapVirtualKey((uint)key, 0), 0, UIntPtr.Zero);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _insideColliderCount = Mathf.Clamp(_insideColliderCount - 1, 0, _insideColliderCount);

        if (_insideColliderCount == 0)
        {
            keybd_event(System.Convert.ToByte(key), (byte)MapVirtualKey((uint)key, 0), 2, UIntPtr.Zero);
            keybd_event(System.Convert.ToByte(key2), (byte)MapVirtualKey((uint)key2, 0), 2, UIntPtr.Zero);
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Home))
        {
            camera?.SetActive(true);
        }
        
        if (Input.GetKeyDown(KeyCode.End))
        {
            camera?.SetActive(false);
        }

    }
}
