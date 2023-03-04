using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

[RequireComponent(typeof(TMP_Dropdown))]
public class EnviromentManager : MonoBehaviour
{
    private TMP_Dropdown Dropdown;
    private bool useEnvironment = true;
    public Transform EnvironmentParent;
    public GameObject CurrentEnvironment;
    public GameObject Cabinet;
    
    private List<FileInfo> enviromentFiles = new List<FileInfo>();
    void Start()
    {
        Dropdown = GetComponent<TMP_Dropdown>();
        AddEnviorments();
        Dropdown.onValueChanged.AddListener((int value) => {
            if (value == 0)
                useEnvironment = false;
            else
            {
                useEnvironment = true;
                SetEnvironment(Dropdown.options[value].text);
            }    
            Debug.Log("Value: " + value);
        });
        Dropdown.onValueChanged?.Invoke(Dropdown.value);
    }
    [ContextMenu("InvokeDropdown")]
    void invokeDropdown()
    {  
        Dropdown.onValueChanged?.Invoke(Dropdown.value);
    }
    void Update()
    {
        if (!useEnvironment)
            Destroy(CurrentEnvironment);
    }

    void AddEnviorments()
    {
        Dropdown.options.Clear();
        Dropdown.options.Add(new TMP_Dropdown.OptionData("None"));

        enviromentFiles = AssetBundleManager.Instance.GetAssetBundleFiles("Enviroments");
        foreach (var file in enviromentFiles)
        {
            Dropdown.options.Add(new TMP_Dropdown.OptionData(file.Name));
        }
    }
    void SetEnvironment(string enviromentName)
    {
        if (CurrentEnvironment != null)
        {
            Destroy(CurrentEnvironment);
        }
        GameObject env = AssetBundleManager.Instance.LoadAsset<GameObject>("Enviroments", enviromentName);
        CurrentEnvironment = Instantiate(env, EnvironmentParent);

    }
}
