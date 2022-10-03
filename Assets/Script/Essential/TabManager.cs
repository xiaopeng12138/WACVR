using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabManager : MonoBehaviour
{
    public GameObject Tab1Object;
    public GameObject Tab2Object;
    void Start()
    {
        OnFirstTabClick();
    }
    public void OnFirstTabClick()
    {
        Tab1Object.SetActive(true);
        Tab2Object.SetActive(false);
    }
    public void OnSecondTabClick()
    {
        Tab1Object.SetActive(false);
        Tab2Object.SetActive(true);
    }
}
