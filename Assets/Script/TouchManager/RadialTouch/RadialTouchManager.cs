using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialTouchManager : MonoBehaviour
{
    [SerializeField] private Transform Hand;
    [SerializeField] private Transform TriggerTransform;
    [SerializeField] private Transform FrontCircle;
    [SerializeField] private Transform BackCircle;
    [SerializeField] private Transform Center;
    private float FBScaleDiff = 0f;
    private float TriggerDistance = 1f;
    private float MaxDistance = 2f;
    private float ZPosition = 0f;

    void Start()
    {
        FBScaleDiff = FrontCircle.localScale.x - BackCircle.localScale.x;
    }

    void Update()
    {
        TriggerTransform.position = Hand.position;
            SetCenterPosition();
            SetMaxDistance();
        
    }
    private void SetCenterPosition()
    {
        ZPosition = Mathf.Clamp(TriggerTransform.localPosition.z, -0.5f, 0.5f);
        Center.localPosition = new Vector3(
            Center.localPosition.x,
            Center.localPosition.x,
            ZPosition
            );
        
    }

    private void SetMaxDistance()
    {
        TriggerDistance = 1 -  FBScaleDiff * (ZPosition + 0.5f);
        Center.localScale = new Vector3(
            TriggerDistance,
            TriggerDistance,
            TriggerDistance
            );
    }

    private bool IsInRange(Vector3 position)
    {
        if (position.z < BackCircle.localPosition.z && position.z > FrontCircle.localPosition.z)
            return true;
        else    
            return false;
    }
}
