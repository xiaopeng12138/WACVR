using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WindowsInput.Native;

public class PanelThirdPersonButton : MonoBehaviour
{
    public bool isTP;

    private Renderer cr;
    public GameObject tpCamera;
    public GameObject fpsCamera;
    public AudioSource audioSrc;

    void Start()
    {
        cr = GetComponent<Renderer>();

        if (JsonConfiguration.HasKey("ThirdPerson")) SetTP(JsonConfiguration.GetBoolean("ThirdPerson"));
        else SetTP(isTP);
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
        fpsCamera?.SetActive(!state);

        JsonConfiguration.SetBoolean("ThirdPerson", state);
    }
}
