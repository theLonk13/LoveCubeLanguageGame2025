using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BackgroundManager : MonoBehaviour
{
    public int Scene_ID;
    public GameObject Background;
    public float speed;

    private RectTransform rectTransform;
    private List<GameObject> backgrounds = new List<GameObject>();
    private int bgNumber = 3;
    private float backgroundWidth;
    private int direction = 1;

    [SerializeField] private bool debugMode;
    public GameObject debugMark;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = Background.GetComponent<RectTransform>();
        backgroundWidth = rectTransform.rect.width;

        direction = speed > 0 ? -1 : 1;

        for (int i = 0; i < bgNumber; i++)
        {
            GameObject bg = Instantiate(rectTransform.gameObject, Background.transform);
            RectTransform bgRect = bg.GetComponent<RectTransform>();
            bgRect.anchoredPosition = new Vector3(direction * rectTransform.rect.width * i, 0);
            backgrounds.Add(bg);
            bg.transform.SetSiblingIndex(0);
        }

        if (debugMode)
        {
            debugMark.SetActive(true);
            debugMark.transform.SetParent(backgrounds[0].transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float move = speed * Time.deltaTime;

        foreach(GameObject bg in backgrounds)
        {
            bg.transform.position += new Vector3(move, 0);
        }

        GameObject firstBG = backgrounds[0];

        if (debugMode)
        {
            Debug.Log("Pos: " + backgrounds[0].transform.position.x + ", " + backgroundWidth);
            Debug.Log(firstBG.transform.position.x < -backgroundWidth);
        }

        if(direction == -1)
        {
            if (firstBG.transform.position.x > backgroundWidth)
            {
                AddNewBG(direction);
                backgrounds.RemoveAt(0);
                Destroy(firstBG);
            }
        } else if (direction == 1)
        {
            if (firstBG.transform.position.x < -backgroundWidth * 3/4)
            {
                AddNewBG(direction);
                backgrounds.RemoveAt(0);
                Destroy(firstBG);
            }
        }
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

    //Background:
    private void AddNewLeft()
    {
        if (backgrounds.Count < bgNumber)
        {
            GameObject bg = Instantiate(rectTransform.gameObject, Background.transform);
            RectTransform bgRect = bg.GetComponent<RectTransform>();
            bgRect.anchoredPosition = new Vector3(-rectTransform.rect.width * backgrounds.Count, 0);
            backgrounds.Add(bg);
        }
    }

    private void AddNewBG(int direction)
    {
        if (backgrounds.Count < bgNumber)
        {
            GameObject bg = Instantiate(rectTransform.gameObject, Background.transform);
            RectTransform bgRect = bg.GetComponent<RectTransform>();
            bgRect.anchoredPosition = new Vector3(direction * rectTransform.rect.width * backgrounds.Count, 0);
            backgrounds.Add(bg);
        }
    }
}
