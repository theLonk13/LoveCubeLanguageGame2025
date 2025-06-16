using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymbolFrameScript : MonoBehaviour
{
    [SerializeField] private Image symbolFrameImage;
    private int symbolID = -1;

    public void SetupSymbolFrame(int ID, Color color)
    {
        symbolID = ID;
        symbolFrameImage.color = color;
    }
}
