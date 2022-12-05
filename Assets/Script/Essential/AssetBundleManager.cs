using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AssetBundleManager : MonoBehaviour
{
    public static AssetBundleManager Instance { get; private set; }

    private Dictionary<string, AssetBundle> assetBundles = new Dictionary<string, AssetBundle>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public T LoadAsset<T>(string assetBundleFolder, string assetBundleName) where T : Object
    {
        UnloadAllAssetBundles();
        LoadAssetBundle(assetBundleFolder, assetBundleName);
        if (assetBundles.ContainsKey(assetBundleName))
        {
            string rootAssetPath = assetBundles[assetBundleName].GetAllAssetNames()[0];
            return assetBundles[assetBundleName].LoadAsset<T>(rootAssetPath);
        }
        return null;
    }

    public void LoadAssetBundle(string assetBundleFolder, string assetBundleName)
    {
        if (!assetBundles.ContainsKey(assetBundleName))
        {
            AssetBundle assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, assetBundleFolder, assetBundleName));
            assetBundles.Add(assetBundleName, assetBundle);
        }
    }

    public void UnloadAssetBundle(string assetBundleName)
    {
        if (assetBundles.ContainsKey(assetBundleName))
        {
            assetBundles[assetBundleName].Unload(true);
            assetBundles.Remove(assetBundleName);
        }
    }

    public void UnloadAllAssetBundles()
    {
        foreach (var assetBundle in assetBundles)
        {
            assetBundle.Value.Unload(true);
        }
        assetBundles.Clear();
    }

    public List<FileInfo> GetAssetBundleFiles(string assetBundleFolder)
    {
        
        var path = Path.Combine(Application.streamingAssetsPath, assetBundleFolder);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        var dir = new DirectoryInfo(path);
        var files = new List<FileInfo>(dir.GetFiles("*.abf"));
        return files;
    }
    
}
