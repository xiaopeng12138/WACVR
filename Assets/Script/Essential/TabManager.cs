using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    public Button DefautTabButton;
    public List<GameObject> Tabs = new List<GameObject>();
    
    [ExecuteAlways]
    void Start()
    {
        DefautTabButton.onClick?.Invoke();
    }
    
    public void OnTabClicked(GameObject tab)
    {
        foreach (GameObject t in Tabs)
            t.SetActive(false);
        tab.SetActive(true);
    }
}
