using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RayManager : MonoBehaviour
{
    public bool RaySwitch = true;
    public Transform CabinetTransform;
    public float Distance = 0.5f;
    XRRayInteractor interactor;
    XRInteractorLineVisual lineVisual;
    LineRenderer lineRenderer;
    void Start()
    {
        interactor = GetComponent<XRRayInteractor>();
        lineVisual = GetComponent<XRInteractorLineVisual>();
        lineRenderer = lineVisual.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        var controllerDistance = CabinetTransform.position.z - gameObject.transform.position.z;
        controllerDistance = Mathf.Abs(controllerDistance);
        Debug.Log(controllerDistance);
        if (controllerDistance < Distance || !RaySwitch)
        {
            interactor.enabled = false;
            lineRenderer.enabled = false;
            lineVisual.enabled = false;
        }
        else
        {
            interactor.enabled = true;
            lineRenderer.enabled = true;
            lineVisual.enabled = true;
        }
    }
}
