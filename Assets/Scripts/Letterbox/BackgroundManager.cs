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

    public void setMovement(BGElement image, float startPercent, float endPercent, float duration)
    {
        image.startPercent = startPercent;
        image.endPercent = endPercent;
        image.duration = duration;
    }

    public void setLoop(BGElement image, bool loop)
    {
        image.loop = loop;
    }

    public void setTiled(BGElement image, bool tiled)
    {
        image.tiled = tiled;
    }

    public void setBackground(BGElement image, bool isBackground)
    {
        image.isBackground = isBackground;
    }
}
