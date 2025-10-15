using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Object for holding data that should be displayed in the tabs UI
[CreateAssetMenu(fileName = "NewInfoDataObject", menuName = "InfoDataObject")]
public class InfoDataObject : ScriptableObject
{
    public string name;
    public Sprite art;
    [TextArea(3, 10)]
    public string info;
}
