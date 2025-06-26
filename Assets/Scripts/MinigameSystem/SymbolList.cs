using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SymbolList", menuName = "SymbolList")]
public class SymbolList : ScriptableObject
{
    [SerializeField] private List<Sprite> STANDARD;
    [SerializeField] private List<Sprite> STANDARD_HANDWRITTEN;

    public List<Sprite> GetSymbolSet(int setID)
    {
        switch (setID)
        {
            case 0:
                return STANDARD;
            case 1:
                return STANDARD_HANDWRITTEN;
            default:
                return null;
        }
    }
}
