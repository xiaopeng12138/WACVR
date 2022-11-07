using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public List<GameObject> Lights = new List<GameObject>();
    List<Material> Materials = new List<Material>();
    [SerializeField]
    private bool isIPCIdle = true;
    [SerializeField]
    private bool useIPCLighting = true;
    static Texture2D RGBColor2D;

    private IEnumerator[] coroutines = new IEnumerator[240];
    public float FadeDuration = 0.5f;

    private void Start() 
    {
        for (int i = 0; i < Lights.Count; i++)
            Materials.Add(Lights[i].GetComponent<Renderer>().material);
        
        var lightWidget = ConfigManager.GetConfigPanelWidget("UseIPCLighting");
        var strengthWidgent = ConfigManager.GetConfigPanelWidget("LightStrength");

        var lightToggle = lightWidget.GetComponent<Toggle>();
        var strengthSlider = strengthWidgent.GetComponent<Slider>();

        lightToggle.onValueChanged.AddListener((bool value) => {
            useIPCLighting = value;
        });

        strengthSlider.onValueChanged.AddListener((float value) => {
            for (int i = 0; i < Materials.Count; i++)
                Materials[i].SetFloat("_EmissionStrength", value);
        });

        lightToggle.onValueChanged.Invoke(useIPCLighting);
        strengthSlider.onValueChanged.Invoke(strengthSlider.value);

        if (useIPCLighting)
        {
            RGBColor2D = new Texture2D(480, 1, TextureFormat.RGBA32, false);
            //RGBColor2D.filterMode = FilterMode.Point; //for debugging
            //GetComponent<Renderer>().material.mainTexture = RGBColor2D; //for debugging
        }
    }
    private void Update() 
    {
        if (!useIPCLighting)
        {
            isIPCIdle = true;
            return;
        }
            
        if (IPCManager.sharedBuffer != null)
        {
            GetTextureFromBytes(IPCManager.GetLightData());
            if (!isIPCIdle)
                UpdateLED();
        }
        else
        {
            isIPCIdle = true;
        }
    }
    private void CheckIPCState(byte[] data)
    {
        if (data[3] == 0)
            isIPCIdle = true;
        else
            isIPCIdle = false;
    }

    private void UpdateLED()
    {
        int index = 0;
        for (int i = 0; i < 30; i++)
        {
            for (int ii = 0; ii < 4; ii++)
            {
                Materials[119 - i - ii * 30].SetColor("_EmissionColor", RGBColor2D.GetPixel(index * 2, 0));
                Materials[119 - i - ii * 30].SetColor("_EmissionColor2", RGBColor2D.GetPixel(index * 2 + 1, 0));
                Materials[210 + i - ii * 30].SetColor("_EmissionColor", RGBColor2D.GetPixel((index + 120) * 2, 0));
                Materials[210 + i - ii * 30].SetColor("_EmissionColor2", RGBColor2D.GetPixel((index + 120) * 2 + 1, 0));
                index++;
            }
        }
    }
    void GetTextureFromBytes(byte[] bytes)
    {
        if (bytes != null && bytes.Length == 1920)
        {
            CheckIPCState(bytes);
            RGBColor2D.LoadRawTextureData(bytes);
            RGBColor2D.Apply();
        }
    }
    public void UpdateFadeLight(int Area, bool State)
    {
        if(!isIPCIdle)
            return;

        Area -= 1;
        if (State)
        {
            Materials[Area].SetColor("_EmissionColor", new Color(1f, 1f, 1f, 1f));
            Materials[Area].SetColor("_EmissionColor2", new Color(1f, 1f, 1f, 1f));
        }
        else
        {
            if (coroutines[Area] != null)
                StopCoroutine(coroutines[Area]);
            coroutines[Area] = FadeOut(Area, Materials[Area]);
            StartCoroutine(coroutines[Area]);
        }      
    }
    public IEnumerator FadeOut(int Area, Material mat)
    {
        for (float time = 0f; time < FadeDuration; time += Time.deltaTime)
        {
            float p = 1 - time / FadeDuration;
            mat.SetColor("_EmissionColor", new Color(p, p, p, 1f));
            mat.SetColor("_EmissionColor2", new Color(p, p, p, 1f));
            yield return null;
        }
    }
}
