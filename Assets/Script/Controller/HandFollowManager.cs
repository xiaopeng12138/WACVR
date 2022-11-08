using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandFollowManager : MonoBehaviour
{
    public GameObject Target;
    public Transform Center;
    public CEnum.handStabilization Mode;
    public float VelocityThreshold = 0.1f;
    private Rigidbody currentRigidbody;
    private Rigidbody TargetRigidbody;
    private Vector3 previousPosition;


    private void Start() 
    {
        TargetRigidbody = Target.GetComponent<Rigidbody>();
        currentRigidbody = GetComponent<Rigidbody>();

        var modeWidget = ConfigManager.GetConfigPanelWidget("HandTrackingMode");
        var threshWidget = ConfigManager.GetConfigPanelWidget("Threshold");

        var modeDropdown = modeWidget.GetComponent<TMP_Dropdown>();
        var threshSlider = threshWidget.GetComponent<Slider>();

        modeDropdown.onValueChanged.AddListener((int value) => {
            VelocityThreshold = ConfigManager.config.Threshold;
            Mode = (CEnum.handStabilization)value;
            switch (Mode)
            {
                case CEnum.handStabilization.None:
                    currentRigidbody.isKinematic = true;
                    break;
                case CEnum.handStabilization.Physics:
                    currentRigidbody.isKinematic = false;
                    break;
                case CEnum.handStabilization.Velocity:
                    currentRigidbody.isKinematic = true;
                    break;
            }
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
            //PhysicsMove(Target.transform);
        }
        previousPosition = Target.transform.position;
    }

    private void PhysicsMove(Transform targetTransform)
    {
        currentRigidbody.velocity = (targetTransform.position - transform.position) / Time.fixedDeltaTime;
        
        Quaternion rotationDelta = targetTransform.rotation * Quaternion.Inverse(transform.rotation);
        rotationDelta.ToAngleAxis(out float angle, out Vector3 axis);

        Vector3 rotationDeltaInDegrees = angle * axis;
        currentRigidbody.angularVelocity = rotationDeltaInDegrees * Mathf.Deg2Rad / Time.fixedDeltaTime;
    }

    private void DistanceSnap()
    {
        transform.position = Target.transform.position;
        transform.rotation = Target.transform.rotation;
    }
    
    private void Update() 
    {
        gameObject.transform.localScale = Target.transform.localScale;
    }

    private void FixedUpdate() 
    {
        switch (Mode)
        {
            case CEnum.handStabilization.None:
                transform.position = Target.transform.position;
                break;
            case CEnum.handStabilization.Physics:
                //transform.position = Target.transform.position;
                PhysicsMove(Target.transform);
                break;
            case CEnum.handStabilization.Velocity:
                VelocityTracking();
                break;
        }
    }
}
