using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class CanvasCameraSwitcher : MonoBehaviour
{
    public Camera Camera;
    private Canvas Canvas;
    void Start()
    {
        Canvas = GetComponent<Canvas>();
    }

    public void EnableCanvasCamera(bool value)
    {
        if (value)
        {
            Canvas.worldCamera = Camera;
        }
        else
        {
            Canvas.worldCamera = null;
        }
    }
}
