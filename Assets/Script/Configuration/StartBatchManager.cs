using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using Lavender.Systems;

public class StartBatchManager : MonoBehaviour
{
    private void Start() 
    {
        ConfigManager.EnsureInitialization();
        if (ConfigManager.config.batFileLocation != "")
            StartExternalProcess.Start(ConfigManager.config.batFileLocation);
        //Process.Start(Path.GetFullPath(ConfigManager.config.batFileLocation));
    }
}
