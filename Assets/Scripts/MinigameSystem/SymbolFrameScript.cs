using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymbolFrameScript : MonoBehaviour
{
    [SerializeField] private Image symbolFrameImage;
    [SerializeField] private Image symbolFrameHighlight;
    private int symbolID = -1;

    public void SetupSymbolFrame(int ID, Color color)
    {
        symbolID = ID;
        symbolFrameImage.color = color;
    }

    public void SetupSymbolFrame(int ID, Sprite sprite)
    {
        symbolID = ID;
        symbolFrameImage.sprite = sprite;
    }

    public void ToggleHighlight(bool highlight)
    {
        symbolFrameHighlight.gameObject.SetActive(highlight);
    }
}
