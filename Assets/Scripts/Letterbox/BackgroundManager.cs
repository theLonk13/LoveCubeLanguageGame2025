using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public int Scene_ID;
    public BGElement image;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setMovement(BGElement image, float startPercent, float endPercent, float duration, bool loop = false, bool tiled = false, bool isBackground = false)
    {
        image.startPercent = startPercent;
        image.endPercent = endPercent;
        image.duration = duration;
        image.loop = loop;
        image.tiled = tiled;
        image.isBackground = isBackground;
    }
}
