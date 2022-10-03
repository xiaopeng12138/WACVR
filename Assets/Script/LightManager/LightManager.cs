using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Security.Principal;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public List<GameObject> Lights = new List<GameObject>();
    List<Material> Materials = new List<Material>();
    public static bool isIPCIdle = false;
    public static bool IsUseIPC = true;
    static Texture2D RGBColor2D;

    public static MemoryMappedFile sharedBuffer;
    public static MemoryMappedViewAccessor sharedBufferAccessor;

    private IEnumerator[] coroutines = new IEnumerator[240];
    public float FadeDuration = 0.5f;

    private void Start() 
    {
        ConfigManager.EnsureInitialization();
        ConfigManager.onConfigChanged += UpdateConfig;
        UpdateConfig();

        for (int i = 0; i < Lights.Count; i++)
            Materials.Add(Lights[i].GetComponent<Renderer>().material);
        
        if (IsUseIPC)
        {
            InitializeIPC("Local\\WACVR_SHARED_BUFFER", 2164);
            RGBColor2D = new Texture2D(480, 1, TextureFormat.RGBA32, false);
            //RGBColor2D.filterMode = FilterMode.Point; //for debugging
            //GetComponent<Renderer>().material.mainTexture = RGBColor2D; //for debugging
        }
    }
    private void Update() 
    {
        if (sharedBuffer != null)
            GetTextureFromBytes(GetBytesFromMemory());
        else
            return;
        if (IsUseIPC)
            CheckIPCState();
        if (!isIPCIdle)
            UpdateLED();
    }
    void UpdateConfig()
    {
        IsUseIPC = ConfigManager.config.useIPCLighting;
    }
    private void CheckIPCState()
    {
        if (RGBColor2D.GetPixel(0 , 0).a == 1)
            isIPCIdle = false;
        else
            isIPCIdle = true;
    }
    private void InitializeIPC(string sharedMemoryName, int sharedMemorySize)
    {
        MemoryMappedFileSecurity CustomSecurity = new MemoryMappedFileSecurity();
        SecurityIdentifier sid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
        var acct = sid.Translate(typeof(NTAccount)) as NTAccount;
        CustomSecurity.AddAccessRule(new System.Security.AccessControl.AccessRule<MemoryMappedFileRights>(acct.ToString(), MemoryMappedFileRights.FullControl, System.Security.AccessControl.AccessControlType.Allow));
        sharedBuffer = MemoryMappedFile.CreateOrOpen(sharedMemoryName, sharedMemorySize, MemoryMappedFileAccess.ReadWrite, MemoryMappedFileOptions.None, CustomSecurity, System.IO.HandleInheritability.Inheritable);
        sharedBufferAccessor = sharedBuffer.CreateViewAccessor();
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
        RGBColor2D.LoadRawTextureData(bytes);
        RGBColor2D.Apply();
    }
    byte[] GetBytesFromMemory()
    {
        byte[] bytes = new byte[1920];
        sharedBufferAccessor.ReadArray<byte>(244, bytes, 0, 1920);
        return bytes;
    }
    public void UpdateLightFade(int Area, bool State)
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
