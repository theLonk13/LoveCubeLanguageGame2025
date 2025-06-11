using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymbolClickableScript : MonoBehaviour
{
    private int symbolID = -1;
    [SerializeField] private Image symbolClickableImage;
    private MinigameManager minigameMan;

    // reference to TopScreen obj
    private GameObject topScreenParent;

    private void Awake()
    {
        topScreenParent = this.transform.parent.gameObject;
    }
    public void SetupSymbolClickable(MinigameManager newMan, int id, Color color)
    {
        minigameMan = newMan;
        symbolID = id;
        symbolClickableImage.color = color;
        RandomizePosition();
    }

    private void RandomizePosition()
    {
        RectTransform topScreenRect = topScreenParent.GetComponent<RectTransform>();
        float rectWidth = topScreenRect.rect.width;
        float rectHeight = topScreenRect.rect.height;
        this.gameObject.transform.localPosition = new Vector3(Random.Range(-rectWidth/2, rectWidth/2), Random.Range(-rectHeight/2, rectHeight/2), 1);
        while (!minigameMan.CheckSymbolClickableSpacing(this.gameObject.transform))
        {
            this.gameObject.transform.localPosition = new Vector3(Random.Range(-rectWidth / 2, rectWidth / 2), Random.Range(-rectHeight / 2, rectHeight / 2), 1);
        }
        //Debug.LogFormat($"SymbolClickable ID: {symbolID} placed at {gameObject.transform.localPosition}");
    }

    public void SymbolClicked()
    {
        minigameMan.SymbolClicked(symbolID);
    }
}
