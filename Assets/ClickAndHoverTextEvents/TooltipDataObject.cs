using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TooltipDataObject", menuName = "TooltipDataObject")]
public class TooltipDataObject : ScriptableObject
{
    public string[] keywords;
    public string text;

    public bool CheckMatch(string key)
    {
        foreach (string keyword in keywords)
        {
            if (keyword.Equals(key))
            {
                return true;
            }
        }
        return false;
    }
}
