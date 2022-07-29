using UnityEngine;
using System.IO.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class ColliderToSerial : MonoBehaviour
{
    public GameObject LightManager;
    private LightManager lightManager;
    private int _insideColliderCount = 0;
    public static event Action touchDidChange;
    private int Area;
    private void Start() 
    {
        Area = Convert.ToInt32(gameObject.name);
        lightManager = LightManager.GetComponent<LightManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        _insideColliderCount += 1;
        Serial.SetTouch(Area, true);
        touchDidChange?.Invoke();
        lightManager.UpdateLightFade(Area, true);
    }

    private void OnTriggerExit(Collider other)
    {
        _insideColliderCount -= 1;
        _insideColliderCount = Mathf.Max(0, _insideColliderCount);
        if (_insideColliderCount == 0)
        {
            Serial.SetTouch(Area, false);
            touchDidChange?.Invoke();
            lightManager.UpdateLightFade(Area, false);
        }
    }
}
