using UnityEngine;
using UnityEditor;
using System.IO;
public class AssetBundleCreator
{
    static string assetBundleDirectory = "Assets/AssetBundles";
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        if (!Directory.Exists(assetBundleDirectory))
            Directory.CreateDirectory(assetBundleDirectory);
        try
        {
            BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.Log(e);
        }
    }
}
