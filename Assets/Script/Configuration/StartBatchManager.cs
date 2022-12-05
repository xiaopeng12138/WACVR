using UnityEngine;
using System.Diagnostics;
using System.IO;
using Lavender.Systems;

public class StartBatchManager : MonoBehaviour
{
    uint pid = 0;
    private void Start() 
    {
        ConfigManager.EnsureInitialization();
        if (ConfigManager.config.batFileLocation != "")
            pid = StartExternalProcess.Start(ConfigManager.config.batFileLocation);
        UnityEngine.Debug.Log("Batch file with PID: " + pid);
        //Process.Start(Path.GetFullPath(ConfigManager.config.batFileLocation));
    }
    private void OnDestroy() 
    {
        if (pid != 0)
        {
            StartExternalProcess.KillProcess(pid);
            UnityEngine.Debug.Log("Batch file with PID: " + pid + " killed");
        }
    }
}
