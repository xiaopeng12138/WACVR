using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;

public class StartBatchManager : MonoBehaviour
{
    private void Start() 
    {
        ConfigManager.EnsureInitialization();
        Process.Start(Path.GetFullPath(ConfigManager.config.batFileLocation));
    }
}
