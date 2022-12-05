using System.Collections;
using System;
using UnityEngine;
using WindowsInput.Native;
using TMPro;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

public class ConfigManager : MonoBehaviour
{
    public static Config config;
    private static bool hasInitialized = false;
    public static event Action onConfigChanged;
    private static float saverTimer = 0;
    private static bool isSavingConfig = false;
    private float saverDelay = 1.5f;

    public List<GameObject> Tabs;
    [SerializeField]
    public static List<ConfigPanelComponent> ConfigPanelComponents;
    void Awake()
    {
        onConfigChanged += EnsureInitialization;
        onConfigChanged += SaveFileWait;
        
    }
    void Start()
    {
        EnsureInitialization();
        AddListenerToWidget(ConfigPanelComponents);
        onConfigChanged?.Invoke();
    }
    public static void EnsureInitialization() 
    {
        if (hasInitialized) 
            return;
        LoadFile();
        ConfigPanelComponents = GetConfigPanelComponentsStatic();
        UpdateConfigPanelFromConfig(ref ConfigPanelComponents);
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
            SaveFileWait();
            Debug.Log("Config file created");
        }
    }
    public static string GetFileName() 
    {
        return Application.dataPath + "/../config.json";
    }
    public static void SaveFileWait() 
    {
        isSavingConfig = true;
        saverTimer = 0;
        //Debug.Log("Saving config file");
    }
    public void saveFile() 
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
                saveFile();
            }
        }
    }

    void OnDestroy()
    {
        saveFile();
    }

    public static GameObject GetConfigPanelWidget(string configKeyName)
    {
        EnsureInitialization();
        foreach (var item in ConfigPanelComponents)
        {
            if (item.ConfigKeyName == configKeyName)
            {
                return item.Widget;
            }
        }
        return null;
    }

    private List<ConfigPanelComponent> GetConfigPanelComponents()
    {
        var _configPanelComponents = new List<ConfigPanelComponent>();
        foreach (var tab in Tabs)
        {
            var _configPanelComponentsInTab = tab.GetComponentsInChildren<ConfigPanelComponent>();
            _configPanelComponents.AddRange(_configPanelComponentsInTab);
        }
        return _configPanelComponents;
    }

    private static List<ConfigPanelComponent> GetConfigPanelComponentsStatic()
    {
        var _configPanelComponents = new List<ConfigPanelComponent>();
        _configPanelComponents = GameObject.FindObjectOfType<ConfigManager>().GetConfigPanelComponents();
        return _configPanelComponents;
    }

    private void AddListenerToWidget(List<ConfigPanelComponent> _configPanelComponents)
    {
        foreach (var configPanelComponent in _configPanelComponents)
        {
            var widget = configPanelComponent.Widget;
            var dropdown = widget.GetComponent<TMP_Dropdown>();
            var toggle = widget.GetComponent<Toggle>();
            var slider = widget.GetComponent<Slider>();
            var valueManager = widget.GetComponent<ValueManager>();
            if (dropdown != null) // add listener to dropdown to update config by key name
            {
                dropdown.onValueChanged.AddListener((int value) =>
                {
                    var field = config.GetType().GetField(configPanelComponent.ConfigKeyName);
                    field.SetValue(config, value);
                    onConfigChanged?.Invoke();
                });
            }
            else if (toggle != null)
            {
                toggle.onValueChanged.AddListener((bool value) =>
                {
                    var field = config.GetType().GetField(configPanelComponent.ConfigKeyName);
                    field.SetValue(config, value);
                    onConfigChanged?.Invoke();
                });
            }
            else if (slider != null)
            {
                slider.onValueChanged.AddListener((float value) =>
                {
                    var field = config.GetType().GetField(configPanelComponent.ConfigKeyName);
                    field.SetValue(config, value);
                    onConfigChanged?.Invoke();
                });
            }
            else if (valueManager != null)
            {
                valueManager.onValueChanged.AddListener(delegate
                {
                    var field = config.GetType().GetField(configPanelComponent.ConfigKeyName);
                    field.SetValue(config, valueManager.value);
                    onConfigChanged?.Invoke();
                });
            }
        }
    }

    private static void UpdateConfigPanelFromConfig(ref List<ConfigPanelComponent> _configPanelComponents)
    {
        foreach (var configPanelComponent in _configPanelComponents)
        {
            Debug.Log(configPanelComponent.ConfigKeyName);
            var componentObject = configPanelComponent.Widget;
            if (componentObject.GetComponent<TMP_Dropdown>() != null)
            {
                var dropdown = componentObject.GetComponent<TMP_Dropdown>();
                var field = config.GetType().GetField(configPanelComponent.ConfigKeyName);
                dropdown.value = (int)field.GetValue(config);
            }
            else if (componentObject.GetComponent<Toggle>() != null)
            {
                var toggle = componentObject.GetComponent<Toggle>();
                var field = config.GetType().GetField(configPanelComponent.ConfigKeyName);
                toggle.isOn = (bool)field.GetValue(config);
            }
            else if (componentObject.GetComponent<Slider>() != null)
            {
                var slider = componentObject.GetComponent<Slider>();
                var field = config.GetType().GetField(configPanelComponent.ConfigKeyName);
                slider.value = (float)field.GetValue(config);
            }
            else if (componentObject.GetComponent<ValueManager>() != null)
            {
                var valueManager = componentObject.GetComponent<ValueManager>();
                var field = config.GetType().GetField(configPanelComponent.ConfigKeyName);
                valueManager.value = (float)field.GetValue(config);
            }
        }
    }
}