using System.Linq;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using WindowsInput.Native;

public class DropdownPopulateList : MonoBehaviour
{
    TMP_Dropdown Dropdown;
    public CEnum.listType ListType = CEnum.listType.VirtualKeyCode;
    [ExecuteAlways]
    void Awake()
    {
        PopulateList();
    }
    void PopulateList()
    {
        Dropdown = GetComponent<TMP_Dropdown>();
        Dropdown.ClearOptions();
        List<string> keyNames = new List<string>();
        switch (ListType)
        {
            case CEnum.listType.VirtualKeyCode:
                keyNames = Enum.GetNames(typeof(VirtualKeyCode)).ToList();
                break;
            case CEnum.listType.captureMode:
                keyNames = Enum.GetNames(typeof(CEnum.captureMode)).ToList();
                break;
            case CEnum.listType.spectatorMode:
                keyNames = Enum.GetNames(typeof(CEnum.spectatorMode)).ToList();
                break;
            case CEnum.listType.FPS:
                keyNames = Enum.GetNames(typeof(CEnum.FPS)).ToList();
                break;
            case CEnum.listType.handStabilization:
                keyNames = Enum.GetNames(typeof(CEnum.handStabilization)).ToList();
                break;
        }
        Dropdown.AddOptions(keyNames);
    }
    
}
