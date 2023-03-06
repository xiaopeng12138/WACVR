using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class LightManager : MonoBehaviour
{
    public List<GameObject> Lights = new List<GameObject>();
    List<Material> Materials = new List<Material>();
    [SerializeField]
    private bool isIPCIdle = true;
    [SerializeField]
    private bool useIPCLighting = true;
    //static Texture2D RGBColor2D;
    static byte[] RGBAColors;
    static byte[][] Colors;

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
            //RGBColor2D = new Texture2D(480, 1, TextureFormat.RGBA32, false);
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
            //GetTextureFromBytes(IPCManager.GetLightData());
            RGBAColors = IPCManager.GetLightData();
            Colors = SplitByteArray(RGBAColors, 4);
            CheckIPCState(RGBAColors);
            if (isIPCIdle)
                return;
            UpdateLED();
        }
        else
        {
            isIPCIdle = true;
        }
    }
    private bool CheckIPCState(byte[] data)
    {
        if (data[3] == 0)
        {
            isIPCIdle = true;
            return true;
        }
        else
        {
            isIPCIdle = false;
            return false;
        }
    }

    private void UpdateLED()
    {
        int index = 0;
        for (int i = 0; i < 30; i++)
        {
            for (int ii = 0; ii < 4; ii++)
            {
                
                Materials[119 - i - ii * 30].SetColor("_EmissionColor", 
                    new Color32(Colors[index * 2][0], Colors[index * 2][1], Colors[index * 2][2], 255));
                Materials[119 - i - ii * 30].SetColor("_EmissionColor2", 
                    new Color32(Colors[index * 2 + 1][0], Colors[index * 2 + 1][1], Colors[index * 2 + 1][2], 255));
                Materials[210 + i - ii * 30].SetColor("_EmissionColor", 
                    new Color32(Colors[(index + 120) * 2][0], Colors[(index + 120) * 2][1], Colors[(index + 120) * 2][2], 255));
                Materials[210 + i - ii * 30].SetColor("_EmissionColor2", 
                    new Color32(Colors[(index + 120) * 2 + 1][0], Colors[(index + 120) * 2 + 1][1], Colors[(index + 120) * 2 + 1][2], 255));
                // Materials[119 - i - ii * 30].SetColor("_EmissionColor", RGBColor2D.GetPixel(index * 2, 0));
                // Materials[119 - i - ii * 30].SetColor("_EmissionColor2", RGBColor2D.GetPixel(index * 2 + 1, 0));
                // Materials[210 + i - ii * 30].SetColor("_EmissionColor", RGBColor2D.GetPixel((index + 120) * 2, 0));
                // Materials[210 + i - ii * 30].SetColor("_EmissionColor2", RGBColor2D.GetPixel((index + 120) * 2 + 1, 0));
                index++;
            }
        }
    }
    void GetTextureFromBytes(byte[] bytes)
    {
        if (bytes == null || bytes.Length != 1920)
            return;

        if (CheckIPCState(bytes))
            return;
        var newbytes = new byte[1920];
        newbytes = bytes;
        //RGBColor2D.LoadRawTextureData(newbytes);
        //RGBColor2D.Apply();
        
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
    public static byte[][] SplitByteArray(byte[] byteArray, int chunkSize)
    {
        int numChunks = (byteArray.Length + chunkSize - 1) / chunkSize;
        byte[][] result = new byte[numChunks][];
        for (int i = 0; i < numChunks; i++)
        {
            int offset = i * chunkSize;
            int chunkLength = Math.Min(chunkSize, byteArray.Length - offset);
            byte[] chunk = new byte[chunkLength];
            Buffer.BlockCopy(byteArray, offset, chunk, 0, chunkLength);
            result[i] = chunk;
        }
        return result;
    }    


}
