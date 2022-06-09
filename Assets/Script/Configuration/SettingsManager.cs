using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public double DefaultPhysicFPS = 90;
    public double DefaultHandSize = 7;
    public float[] DefaultHandPosition = {1f, 1f, -3f};

    private bool FocusChecked;
    public GameObject Display;
    public GameObject LHand;
    public GameObject RHand;
    UwcConfigurator UwcConfig;

    void Start()
    {
        UwcConfig = Display.GetComponent<UwcConfigurator>();
        UpdateAllConfigs();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5) | !FocusChecked) //Update ConfigFile
        {
            if (Application.isFocused)
            {
                FocusChecked=true;
                UpdateAllConfigs();
            }  
        }
        if (!Application.isFocused)
            FocusChecked=false;
    }
    void UpdateAllConfigs()
    {
        JsonConfiguration.hasInitialized = false;
        UwcConfig.UpdateConfigs();
        UpdatePhysicFPS();
        UpdateHands();
        Debug.Log("Configs Updated");
    }

    void UpdatePhysicFPS()
    {
        if (!JsonConfiguration.HasKey("PhysicFPS")) 
            JsonConfiguration.SetDouble("PhysicFPS", DefaultPhysicFPS); 
        Time.fixedDeltaTime = 1/(float)JsonConfiguration.GetDouble("PhysicFPS");
    }

    static float HandSize;
    static float[] HandPosition;

    void UpdateHands()
    {
        if (!JsonConfiguration.HasKey("HandSize")) 
            JsonConfiguration.SetDouble("HandSize", DefaultHandSize); 
        if (!JsonConfiguration.HasKey("HandPosition")) 
            JsonConfiguration.SetFloatArray("HandPosition", DefaultHandPosition); 
        HandSize = (float)JsonConfiguration.GetDouble("HandSize");
        HandPosition = JsonConfiguration.GetFloatArray("HandPosition");
        LHand.transform.localScale = new Vector3(HandSize/100,HandSize/100,HandSize/100);
        RHand.transform.localScale = new Vector3(HandSize/100,HandSize/100,HandSize/100);
        LHand.transform.localPosition = new Vector3(HandPosition[0]/100,HandPosition[1]/100,HandPosition[2]/100);
        RHand.transform.localPosition = new Vector3(HandPosition[0]/-100,HandPosition[1]/100,HandPosition[2]/100);
    }
}
