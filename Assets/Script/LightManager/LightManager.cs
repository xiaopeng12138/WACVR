using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public List<GameObject> Lights = new List<GameObject>();
    public List<Material> Materials = new List<Material>();
    public float FadeDuration = 0.5f;
    private IEnumerator[] coroutines = new IEnumerator[240];
    
    private void Start() 
    {
        for (int i = 0; i < Lights.Count; i++)
        {
            Materials[i] = Lights[i].GetComponent<Renderer>().material;
        }
    }
    public void UpdateLight(int Area, bool State)
    {
        Area -= 1;
        
        if (State)
        {
            Materials[Area].SetColor("_EmissionColor", new Color(1f, 1f, 1f, 1f));
        }
        else
        {
            if (coroutines[Area] != null)
                StopCoroutine(coroutines[Area]);
            coroutines[Area] = FadeOut(Area, Materials[Area]);
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
