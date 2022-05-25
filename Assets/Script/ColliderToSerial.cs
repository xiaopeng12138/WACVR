using UnityEngine;
using System.IO.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class ColliderToSerial : MonoBehaviour
{
    private Renderer cr;
    private int _insideColliderCount = 0;
    public static event Action touchDidChange;

    private void Start()
    {
        cr = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _insideColliderCount += 1;
        Serial.SetTouch(Convert.ToInt32(gameObject.name), true);
        touchDidChange?.Invoke();
        cr.material.color = new Color(1f, 1f, 1f, 1f);
    }

    private void OnTriggerExit(Collider other)
    {
        _insideColliderCount -= 1;
        _insideColliderCount = Mathf.Max(0, _insideColliderCount);

        if (_insideColliderCount == 0)
        {
            Serial.SetTouch(Convert.ToInt32(gameObject.name), false);
            touchDidChange?.Invoke();
            cr.material.color = new Color(0f, 0f, 0f, 0f);
        }
    }
}
