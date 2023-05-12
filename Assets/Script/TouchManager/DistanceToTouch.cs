using UnityEngine;
using System.IO.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class DistanceToTouch : MonoBehaviour
{
    public LightManager LightManager;
    public Transform[] Hands;
    private int _insideColliderCount = 0;
    private int Area;
    private void Start() 
    {
        Area = Convert.ToInt32(gameObject.name);
    }
    private void Enter()
    {
        _insideColliderCount += 1;
        TouchManager.SetTouch(Area, true);
        LightManager.UpdateFadeLight(Area, true);
    }

    private void Exit()
    {
        _insideColliderCount -= 1;
        _insideColliderCount = Mathf.Max(0, _insideColliderCount);
        if (_insideColliderCount == 0)
        {
            TouchManager.SetTouch(Area, false);
            LightManager.UpdateFadeLight(Area, false);
        }
    }

    private void Update()
    {
        foreach (var hand in Hands)
        {
            var distance = Vector3.Distance(hand.position, transform.position);
            if (distance < 0.1f)
            {
                //Enter();
                //Debug.Log("Enter");
                return;
            }
        }
    }
}
