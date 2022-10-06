using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using WindowsInput.Native;

public class KeyDropdownManager : MonoBehaviour
{
    TMP_Dropdown Dropdown;
    void Awake()
    {
        Dropdown = GetComponent<TMP_Dropdown>();
        PopulateList();
    }
    void PopulateList()
    {
        string[] enumNames = Enum.GetNames(typeof(VirtualKeyCode));
        List<string> keyNames = new List<string>(enumNames);
        Dropdown.AddOptions(keyNames);
    }
}
