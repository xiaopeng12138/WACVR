using UnityEngine;
using System.IO.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class ColliderToSerial : MonoBehaviour
{
    private int _insideColliderCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        _insideColliderCount += 1;
        Serial.SetTouch(Convert.ToInt32(gameObject.name), true);
    }

    private void OnTriggerExit(Collider other)
    {
        _insideColliderCount -= 1;
        _insideColliderCount = Mathf.Max(0, _insideColliderCount);
        if (_insideColliderCount == 0)
        {
            Serial.SetTouch(Convert.ToInt32(gameObject.name), false);
        }
    }
}
