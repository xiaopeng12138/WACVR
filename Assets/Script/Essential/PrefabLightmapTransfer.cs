using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PrefabLightmapData))]
public class PrefabLightmapTransfer : MonoBehaviour
{
    public GameObject targetPrefabParent;
    private PrefabLightmapData sourceLightmapData;
    private PrefabLightmapData lightmapData;
    void Awake()
    {
        sourceLightmapData = GetComponent<PrefabLightmapData>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
