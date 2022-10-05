using System.Collections;
using System;
using UnityEngine;
using WindowsInput.Native;
using TMPro;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class ConfigManager : MonoBehaviour
{
    public static Config config;
    private static bool hasInitialized = false;
    Config oldConfig;
    public static event Action onConfigChanged;
    private static float saverTimer = 0;
    private static bool isSavingConfig = false;
    private float saverDelay = 1.5f;
    void Awake()
    {
        onConfigChanged += EnsureInitialization;
        onConfigChanged += SaveFile;
    }
    void Start()
    {
        EnsureInitialization();
        FindConfigPanelWidget();
        UpdateConfigPanel();
        AddListenerToWidget();
        onConfigChanged?.Invoke();
    }
    public static void EnsureInitialization() 
    {
        if (hasInitialized) 
            return;
        LoadFile();
        hasInitialized = true;
    }
    private static void LoadFile() 
    {
        Debug.Log("Loading config file");
        if (File.Exists(GetFileName()))
        {
            Debug.Log("Config file exists");
            config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(GetFileName()));
        }
        else 
        {
            Debug.Log("Config file does not exist");
            config = new Config();
            SaveFile();
            Debug.Log("Config file created");
        }
    }
    public static string GetFileName() 
    {
        return Application.dataPath + "/../config.json";
    }
    public static void SaveFile() 
    {
        isSavingConfig = true;
        saverTimer = 0;
        Debug.Log("Saving config file");
    }
    public void saveFileWait() 
    {
        File.WriteAllText(GetFileName(), JsonConvert.SerializeObject(config, Formatting.Indented));
        Debug.Log("Config file saved");
    }

    void Update()
    {
        if (isSavingConfig)
        {
            saverTimer += Time.deltaTime;
            if (saverTimer >= saverDelay)
            {
                isSavingConfig = false;
                saverTimer = 0;
                saveFileWait();
            }
        }
    }

    private TMP_Dropdown CaptureModeDropdown;
    private TMP_Dropdown CaptureFPSDropdown;
    private Toggle CaptureDesktopToggle;
    private TMP_Dropdown SpectatorModeDropdown;
    private TMP_Dropdown SpectatorFPSDropdown;
    private Slider SpectatorFOVSlider;
    private Slider HandSizeSlider;
    private Slider HandXSlider;
    private Slider HandYSlider;
    private Slider HandZSlider;
    private TMP_Dropdown SkyboxDropdown;
    private ValueManager PlayerHeightManager;
    private Slider HapticDurationSlider;
    private Slider HapticAmplitudeSlider;
    private TMP_Dropdown TouchSampleRateDropdown;
    private TMP_Dropdown HandStabilizationModeDropdown;
    private Slider HandStabilVelocitySlider;
    private Slider HandStabilDistanceSlider;
    private Slider HandStabilSmoothSlider;
    private Toggle isIPCLightingToggle;
    private Toggle isIPCTouchToggle;
    private TMP_Dropdown TestKeyDropdown;
    private TMP_Dropdown ServiceKeyDropdown;
    private TMP_Dropdown CoinKeyDropdown;
    private TMP_Dropdown CustomKeyDropdown;

    void FindConfigPanelWidget()
    {
        CaptureModeDropdown = transform.Find("Tab1").Find("CaptureMode").Find("Dropdown").GetComponent<TMP_Dropdown>();
        CaptureFPSDropdown = transform.Find("Tab1").Find("CaptureFPS").Find("Dropdown").GetComponent<TMP_Dropdown>();
        CaptureDesktopToggle = transform.Find("Tab1").Find("CaptureDesktop").Find("Toggle").GetComponent<Toggle>();
        SpectatorModeDropdown = transform.Find("Tab1").Find("SpectatorMode").Find("Dropdown").GetComponent<TMP_Dropdown>();
        SpectatorFPSDropdown = transform.Find("Tab1").Find("SpectatorFPS").Find("Dropdown").GetComponent<TMP_Dropdown>();
        SpectatorFOVSlider = transform.Find("Tab1").Find("SpectatorFOV").Find("Slider").GetComponent<Slider>();
        HandSizeSlider = transform.Find("Tab1").Find("HandSize").Find("Slider").GetComponent<Slider>();
        HandXSlider = transform.Find("Tab1").Find("HandX").Find("Slider").GetComponent<Slider>();
        HandYSlider = transform.Find("Tab1").Find("HandY").Find("Slider").GetComponent<Slider>();
        HandZSlider = transform.Find("Tab1").Find("HandZ").Find("Slider").GetComponent<Slider>();
        SkyboxDropdown = transform.Find("Tab1").Find("Skybox").Find("Dropdown").GetComponent<TMP_Dropdown>();
        PlayerHeightManager = transform.Find("Tab1").Find("PlayerHeight").Find("Value").GetComponent<ValueManager>();
        HapticDurationSlider = transform.Find("Tab2").Find("HapticDuration").Find("Slider").GetComponent<Slider>();
        HapticAmplitudeSlider = transform.Find("Tab2").Find("HapticAmplitude").Find("Slider").GetComponent<Slider>();
        TouchSampleRateDropdown = transform.Find("Tab2").Find("TouchSampleRate").Find("Dropdown").GetComponent<TMP_Dropdown>();
        HandStabilizationModeDropdown = transform.Find("Tab2").Find("HandStabilization").Find("Dropdown").GetComponent<TMP_Dropdown>();
        //HandStabilVelocitySlider = transform.Find("Tab2").Find("HandStabilVelocity").Find("Slider").GetComponent<Slider>();
        //HandStabilDistanceSlider = transform.Find("Tab2").Find("HandStabilDistance").Find("Slider").GetComponent<Slider>();
        //HandStabilSmoothSlider = transform.Find("Tab2").Find("HandStabilSmooth").Find("Slider").GetComponent<Slider>();
        isIPCLightingToggle = transform.Find("Tab2").Find("UseIPCLighting").Find("Toggle").GetComponent<Toggle>();
        isIPCTouchToggle = transform.Find("Tab2").Find("UseIPCTouch").Find("Toggle").GetComponent<Toggle>();
        TestKeyDropdown = transform.Find("Tab2").Find("TestKeyBind").Find("Dropdown").GetComponent<TMP_Dropdown>();
        ServiceKeyDropdown = transform.Find("Tab2").Find("ServiceKeyBind").Find("Dropdown").GetComponent<TMP_Dropdown>();
        CoinKeyDropdown = transform.Find("Tab2").Find("CoinKeyBind").Find("Dropdown").GetComponent<TMP_Dropdown>();
        CustomKeyDropdown = transform.Find("Tab2").Find("CustomKeyBind").Find("Dropdown").GetComponent<TMP_Dropdown>();
    }
    void AddListenerToWidget()
    {
        CaptureModeDropdown.onValueChanged.AddListener(onIntChanged);
        CaptureFPSDropdown.onValueChanged.AddListener(onIntChanged);
        CaptureDesktopToggle.onValueChanged.AddListener(onBoolChanged);
        SpectatorModeDropdown.onValueChanged.AddListener(onIntChanged);
        SpectatorFPSDropdown.onValueChanged.AddListener(onIntChanged);
        SpectatorFOVSlider.onValueChanged.AddListener(onFloatChanged);
        HandSizeSlider.onValueChanged.AddListener(onFloatChanged);
        HandXSlider.onValueChanged.AddListener(onFloatChanged);
        HandYSlider.onValueChanged.AddListener(onFloatChanged);
        HandZSlider.onValueChanged.AddListener(onFloatChanged);
        SkyboxDropdown.onValueChanged.AddListener(onIntChanged);
        PlayerHeightManager.onValueChanged.AddListener(onValueChanged);
        HapticDurationSlider.onValueChanged.AddListener(onFloatChanged);
        HapticAmplitudeSlider.onValueChanged.AddListener(onFloatChanged);
        TouchSampleRateDropdown.onValueChanged.AddListener(onIntChanged);
        HandStabilizationModeDropdown.onValueChanged.AddListener(onIntChanged);
        //HandStabilVelocitySlider.onValueChanged.AddListener(onFloatChanged);
        //HandStabilDistanceSlider.onValueChanged.AddListener(onFloatChanged);
        //HandStabilSmoothSlider.onValueChanged.AddListener(onFloatChanged);
        isIPCLightingToggle.onValueChanged.AddListener(onBoolChanged);
        isIPCTouchToggle.onValueChanged.AddListener(onBoolChanged);
        TestKeyDropdown.onValueChanged.AddListener(onIntChanged);
        ServiceKeyDropdown.onValueChanged.AddListener(onIntChanged);
        CoinKeyDropdown.onValueChanged.AddListener(onIntChanged);
        CustomKeyDropdown.onValueChanged.AddListener(onIntChanged);
    }
    void onValueChanged()
    {
        config.PlayerHeight = PlayerHeightManager.Value;
        onConfigChanged?.Invoke();
    }
    void onIntChanged(int value)
    {
        config.CaptureMode = (Config.captureMode)CaptureModeDropdown.value;
        config.CaptureFPS = (Config.captureFPS)CaptureFPSDropdown.value;
        config.SpectatorMode = (Config.spectatorMode)SpectatorModeDropdown.value;
        config.SpectatorFPS = (Config.spectatorFPS)SpectatorFPSDropdown.value;
        config.Skybox = SkyboxDropdown.value;
        config.TouchSampleRate = (Config.touchSampleRate)TouchSampleRateDropdown.value;
        config.HandStabilizationMode = (Config.handStabilization)HandStabilizationModeDropdown.value;
        config.TestKey = (VirtualKeyCode)TestKeyDropdown.value;
        config.ServiceKey = (VirtualKeyCode)ServiceKeyDropdown.value;
        config.CoinKey = (VirtualKeyCode)CoinKeyDropdown.value;
        config.CustomKey = (VirtualKeyCode)CustomKeyDropdown.value;
        onConfigChanged?.Invoke();
    }

    void onFloatChanged(float value)
    {
        config.SpectatorFOV = SpectatorFOVSlider.value;
        config.HandSize = HandSizeSlider.value;
        config.HandPosition[0] = HandXSlider.value;
        config.HandPosition[1] = HandYSlider.value;
        config.HandPosition[2] = HandZSlider.value;
        config.HapticDuration = HapticDurationSlider.value;
        config.HapticAmplitude = HapticAmplitudeSlider.value;
        //config.HandStabilVelocity = HandStabilVelocitySlider.value;
        //config.HandStabilDistance = HandStabilDistanceSlider.value;
        //config.HandStabilSmooth = HandStabilSmoothSlider.value;
        onConfigChanged?.Invoke();
    }
    
    void onBoolChanged(bool value)
    {
        config.useIPCLighting = isIPCLightingToggle.isOn;
        config.useIPCTouch = isIPCTouchToggle.isOn;
        onConfigChanged?.Invoke();
    }

    void UpdateConfigPanel()
    {
        CaptureModeDropdown.value = (int)config.CaptureMode;
        CaptureFPSDropdown.value = (int)config.CaptureFPS;
        CaptureDesktopToggle.isOn = config.CaptureDesktop;
        SpectatorModeDropdown.value = (int)config.SpectatorMode;
        SpectatorFPSDropdown.value = (int)config.SpectatorFPS;
        SpectatorFOVSlider.value = config.SpectatorFOV;
        HandSizeSlider.value = config.HandSize;
        HandXSlider.value = config.HandPosition[0];
        HandYSlider.value = config.HandPosition[1];
        HandZSlider.value = config.HandPosition[2];
        SkyboxDropdown.value = config.Skybox;
        PlayerHeightManager.Value = config.PlayerHeight;
        HapticDurationSlider.value = config.HapticDuration;
        HapticAmplitudeSlider.value = config.HapticAmplitude;
        TouchSampleRateDropdown.value = (int)config.TouchSampleRate;
        HandStabilizationModeDropdown.value = (int)config.HandStabilizationMode;
        //HandStabilVelocitySlider.value = HandStabilVelocity;
        //HandStabilDistanceSlider.value = HandStabilDistance;
        //HandStabilSmoothSlider.value = HandStabilSmooth;
        isIPCLightingToggle.isOn = config.useIPCLighting;
        isIPCTouchToggle.isOn = config.useIPCTouch;
        TestKeyDropdown.value = (int)config.TestKey;
        ServiceKeyDropdown.value = (int)config.ServiceKey;
        CoinKeyDropdown.value = (int)config.CoinKey;
        CustomKeyDropdown.value = (int)config.CustomKey;
    }
}