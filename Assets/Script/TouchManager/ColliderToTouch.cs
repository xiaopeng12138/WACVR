using UnityEngine;
using System.IO.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class ColliderToTouch : MonoBehaviour
{
    public LightManager LightManager;
    private int _insideColliderCount = 0;
    private int Area;
    private void Start() 
    {
        Area = Convert.ToInt32(gameObject.name);
    }
    private void OnTriggerEnter(Collider collider)
    {
        _insideColliderCount += 1;
        TouchManager.SetTouch(Area, true);
        LightManager.UpdateFadeLight(Area, true);
    }

    private void OnTriggerExit(Collider collider)
    {
        _insideColliderCount -= 1;
        _insideColliderCount = Mathf.Max(0, _insideColliderCount);
        if (_insideColliderCount == 0)
        {
            TouchManager.SetTouch(Area, false);
            LightManager.UpdateFadeLight(Area, false);
        }
    }
}
