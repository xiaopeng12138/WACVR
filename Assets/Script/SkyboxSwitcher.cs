using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkyboxSwitcher : MonoBehaviour
{
    [SerializeField]
    private List<Material> skyboxes;
    [SerializeField]
    private int currentSkyboxIndex; // should start at 0

    [Header("Components")]
    [SerializeField]
    private PanelButton incrementBtn;
    [SerializeField]
    private PanelButton decrementBtn;
    [SerializeField]
    private TextMeshPro counterTxt;
    // Start is called before the first frame update
    void Start()
    {
        incrementBtn.ButtonPressed += IncrementEvent;
        decrementBtn.ButtonPressed += DecrementEvent;
        //skyboxes.Insert(0, Resources.Load<Material>("unity_builtin_extra/Default-Skybox")); // results in plain blue??? not the ubiquitous unity default
        skyboxes.Insert(0, RenderSettings.skybox); // add ubiquitous default skybox (should be current)
        SetSkybox();
    }

    private void IncrementEvent()
    {
        currentSkyboxIndex = (currentSkyboxIndex + 1) % skyboxes.Count;
        SetSkybox();
    }
    private void DecrementEvent()
    {
        if (--currentSkyboxIndex < 0)
            currentSkyboxIndex = skyboxes.Count - 1;
        SetSkybox();
    }

    private void SetSkybox()
    {
        counterTxt.text = (currentSkyboxIndex + 1).ToString();
        if (skyboxes[currentSkyboxIndex] != null)
            RenderSettings.skybox = skyboxes[currentSkyboxIndex];
    }
}
