using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WindowsInput.Native;

[RequireComponent(typeof(AudioSource))]
public class PanelThirdPersonButton : MonoBehaviour
{
    public bool isTP;

    private Renderer cr;
    public GameObject tpCamera;
    private AudioSource audioSrc;
    private static AudioClip btnSound;

    void Start()
    {
        btnSound = Resources.Load<AudioClip>("Audio/button press");
        audioSrc = GetComponent<AudioSource>();
        audioSrc.playOnAwake = false;
        audioSrc.clip = btnSound;

        cr = GetComponent<Renderer>();

        SetTP(isTP);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        audioSrc.Play();
        isTP = !isTP;
        SetTP(isTP);
    }

    private void SetTP(bool state)
    {
        isTP = state;
        cr.material.color = state ? Color.green : Color.red;
        tpCamera?.SetActive(state);
    }
}
