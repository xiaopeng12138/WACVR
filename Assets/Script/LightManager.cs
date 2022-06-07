using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public List<GameObject> Lights = new List<GameObject>();
    public float FadeDuration = 0.5f;
    private IEnumerator[] coroutines = new IEnumerator[240];
    private void Start() 
    {

    }
    public void UpdateLight(int Area, bool State)
    {
        Area -= 1;
        Material mat = Lights[Area].GetComponent<Renderer>().material;
        if (State)
        {
           
            mat.SetColor("_EmissionColor", new Color(1f, 1f, 1f, 1f));
        }
        else
        {
            if (coroutines[Area] != null)
                StopCoroutine(coroutines[Area]);
            coroutines[Area] = FadeOut(Area, mat);
            StartCoroutine(coroutines[Area]);
        }      
    }
    public IEnumerator FadeOut(int Area, Material mat)
    {
        for (float time = 0f; time < FadeDuration; time += Time.deltaTime)
        {
            float p = 1 - time / FadeDuration;
            mat.SetColor("_EmissionColor", new Color(p, p, p, 1f));
            yield return null;
        }
    }
}
