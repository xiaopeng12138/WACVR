using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandFollowManager : MonoBehaviour
{
    public GameObject Target;
    public Transform Center;
    public Config.handStabilization Mode;
    public float VelocityThreshold = 0.1f;
    private Rigidbody TargetRigidbody;
    private Vector3 previousPosition;

    private void Start() 
    {
        TargetRigidbody = Target.GetComponent<Rigidbody>();
        ConfigManager.onConfigChanged += ApplyConfig;
        ConfigManager.EnsureInitialization();
        ApplyConfig();
    }
    private void ApplyConfig()
    {
        VelocityThreshold = ConfigManager.config.HandStabilVelocity;
        Mode = ConfigManager.config.HandStabilizationMode;
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
            case Config.handStabilization.Velocity:
                VelocityTracking();
                break;
            case Config.handStabilization.None:
                transform.position = Target.transform.position;
                break;
        }
    }
}
