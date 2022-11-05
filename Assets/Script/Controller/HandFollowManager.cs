using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandFollowManager : MonoBehaviour
{
    public GameObject Target;
    public Transform Center;
    public CEnum.handStabilization Mode;
    public float VelocityThreshold = 0.1f;
    private Rigidbody TargetRigidbody;
    private Vector3 previousPosition;

    private void Start() 
    {
        TargetRigidbody = Target.GetComponent<Rigidbody>();

        var modeWidget = ConfigManager.GetConfigPanelWidget("HandStabilization");
        var threshWidget = ConfigManager.GetConfigPanelWidget("Threshold");

        var modeDropdown = modeWidget.GetComponent<TMP_Dropdown>();
        var threshSlider = threshWidget.GetComponent<Slider>();

        modeDropdown.onValueChanged.AddListener((int value) => {
            VelocityThreshold = ConfigManager.config.HandStabilVelocity;
            Mode = (CEnum.handStabilization)value;
        });

        threshSlider.onValueChanged.AddListener((float value) => {
            VelocityThreshold = value;
        });

        modeDropdown.onValueChanged?.Invoke(modeDropdown.value);
        threshSlider.onValueChanged?.Invoke(threshSlider.value);
    }
    
    private void VelocityTracking()
    {
        Vector3 velocity = (Target.transform.position - previousPosition) / Time.deltaTime;
        if (velocity.magnitude > VelocityThreshold)
        {
            transform.position = Target.transform.position;
        }
        previousPosition = Target.transform.position;
    }
    private void Update() 
    {
        gameObject.transform.localScale = Target.transform.localScale;
    }

    private void FixedUpdate() 
    {
        switch (Mode)
        {
            case CEnum.handStabilization.Velocity:
                VelocityTracking();
                break;
            case CEnum.handStabilization.None:
                transform.position = Target.transform.position;
                break;
        }
    }
}
