using TMPro;
using UnityEngine;

[ExecuteAlways]
public class ComponentAutoName : MonoBehaviour
{
    [Header("!Readme!")]
    [TextArea]
    public string Notes = "This script will automatically set the text and config key name to object name. Pls rename prefab/gameobject name.";
    private TMP_Text text;
    private ConfigPanelComponent configPanelComponent;
    private void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
        configPanelComponent = GetComponent<ConfigPanelComponent>();
        text.text = gameObject.name;
    }
    void Update()
    {
        text.text = gameObject.name;
    }
}
