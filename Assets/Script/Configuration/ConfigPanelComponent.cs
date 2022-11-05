using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ConfigPanelComponent : MonoBehaviour
{
    [SerializeField]
    public string ConfigKeyName
    {
        get
        {
            return gameObject.name.Replace(" ", "");
        }
    }
    private string configKeyName;
    [SerializeField]
    public GameObject Widget
    {
        get
        {
            var dropdown = gameObject.GetComponentInChildren<TMP_Dropdown>();
            var toggle = gameObject.GetComponentInChildren<Toggle>();
            var slider = gameObject.GetComponentInChildren<Slider>();
            var value = gameObject.GetComponentInChildren<ValueManager>();
            if (dropdown != null)
                return dropdown.gameObject;
            else if (toggle != null)
                return toggle.gameObject;
            else if (slider != null)
                return slider.gameObject;
            else if (value != null)
                return value.gameObject;
            else
                return null;
        }
    }
}
