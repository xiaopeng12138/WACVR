using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.Networking;
using System.Runtime.InteropServices;
using System;

public class SkyboxSwitcher : MonoBehaviour
{
    private string skyboxPath;
    public List<FileInfo> imageFiles = new List<FileInfo>();
    public List<Texture2D> textures = new List<Texture2D>();
    public List<System.IntPtr> ptrs = new List<System.IntPtr>();
    public GameObject Room;

    [SerializeField]
    private List<Material> skyboxes;
    [SerializeField]
    private int currentSkyboxIndex = 0;

    private TMP_Dropdown Dropdown;

    void Start()
    {      
        Dropdown = GetComponent<TMP_Dropdown>(); // get the dropdown component

        skyboxes.Insert(0, RenderSettings.skybox); // add ubiquitous default skybox (should be current)

        // check StreamingAssets folder for additional skybox textures
        skyboxPath = Path.Combine(Application.streamingAssetsPath, "SkyboxTextures");
        StartCoroutine(AddSkyboxes());

        Dropdown.value = ConfigManager.config.Skybox; // set the dropdown to the current skybox index
        Dropdown.onValueChanged.AddListener((int value) => { // add listener to dropdown
            if (value == 0) // if the first option is selected, disable the room
            {
                Room.SetActive(true);
            }
            else // otherwise, enable the room and set the skybox
            {
                Room.SetActive(false);
                currentSkyboxIndex = Dropdown.value;
                SetSkybox();
            }
        });
        Dropdown.onValueChanged?.Invoke(Dropdown.value); // invoke the listener to set the skybox on startup
    }

    IEnumerator AddSkyboxes()
    {
        var skyboxDir = new DirectoryInfo(skyboxPath);
        
        imageFiles.AddRange(skyboxDir.GetFiles("*.png"));
        imageFiles.AddRange(skyboxDir.GetFiles("*.jpg"));
        imageFiles.AddRange(skyboxDir.GetFiles("*.jpeg"));
        //List<FileInfo> hdrFiles = new List<FileInfo>();
        //hdrFiles.AddRange(skyboxDir.GetFiles("*.hdr"));
        //hdrFiles.AddRange(skyboxDir.GetFiles("*.hdri"));
        //hdrFiles.AddRange(skyboxDir.GetFiles("*.exr"));

        foreach (var file in imageFiles) // Typical image files
        {
            var uwr = UnityWebRequestTexture.GetTexture(file.ToString());
            yield return uwr.SendWebRequest();
            if (uwr.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogWarning($"Couldn't load skybox texture at {uwr.uri}.");
            }
            else
            {
                var skyboxMat = new Material(Shader.Find("Skybox/Panoramic"));
                skyboxMat.SetFloat("_Rotation", 45f);
                var texture = DownloadHandlerTexture.GetContent(uwr);
                if (texture != null)
                {
                    skyboxMat.SetTexture("_MainTex", texture);
                    skyboxes.Add(skyboxMat);

                    Dropdown.options.Add(new TMP_Dropdown.OptionData(file.Name)); // add the skybox option to the dropdown
                }
            }
        }

        SetSkybox();
        //foreach (var file in hdrFiles) // HDR files -- no way to use by scripting?
        //{
        //    var uwr = UnityWebRequest.Get(file.ToString());
        //    yield return uwr.SendWebRequest();
        //    if (uwr.result == UnityWebRequest.Result.ConnectionError)
        //    {
        //        Debug.Log($"Had trouble loading file {uwr.uri}.");
        //    }
        //    else
        //    {
        //        byte[] data = uwr.downloadHandler.data;
        //        if (data != null)
        //        {
        //            GCHandle pinnedArray = GCHandle.Alloc(data, GCHandleType.Pinned);
        //            IntPtr pointer = pinnedArray.AddrOfPinnedObject();

        //            var cubemap = Cubemap.CreateExternalTexture(2200, TextureFormat.DXT5, false, pointer);
        //            var skyboxMat = new Material(Shader.Find("Skybox/Panoramic"));
        //            skyboxMat.SetTexture("_Tex", cubemap);
        //            skyboxes.Add(skyboxMat);

        //            pinnedArray.Free();
        //        }
        //        // FIXME: convert Texture2D to Cubemap
        //        //textures.Add(texture);
        //        //ptrs.Add(texture.GetNativeTexturePtr());
        //        //texture = textures[textures.Count - 1];
        //        //var cubemap = Cubemap.CreateExternalTexture(texture.width, texture.format, false, texture.GetNativeTexturePtr());
        //        //skyboxMat.SetTexture("_Tex", cubemap);
        //    }
        //}
    }

    private void SetSkybox()
    {
        if (currentSkyboxIndex < skyboxes.Count && skyboxes[currentSkyboxIndex] != null)
            RenderSettings.skybox = skyboxes[currentSkyboxIndex];
    }
}
