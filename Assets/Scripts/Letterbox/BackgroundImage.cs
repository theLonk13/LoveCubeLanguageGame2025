using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundImage : MonoBehaviour
{
    public float speed = 0f;
    public bool loop = true;

    //Note to self: maybe use background manager, rewrite

    private RectTransform rectTransform;
    private List<RectTransform> backgrounds = new List<RectTransform>();
    // Start is called before the first frame update
    void Start()
    {
        /*
        rectTransform = GetComponent<RectTransform>();

        if(speed > 0f )
        {
            RectTransform left = Instantiate(rectTransform, transform);
            //RectTransform canvas;
            //left.SetParent(canvas);
            left.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x - rectTransform.rect.width, 0);
            Destroy(left.GetComponent<BackgroundImage>());
            backgrounds.Add(left);
        }
        */
        /*
        RectTransform first = Instantiate(rectTransform, transform);
        first.anchoredPosition = Vector2.zero;
        Destroy(first.GetComponent<BackgroundImage>());
        backgrounds.Add(first);

        RectTransform second = Instantiate(rectTransform, transform);
        second.anchoredPosition = Vector2.zero;
        Destroy(second.GetComponent<BackgroundImage>());
        backgrounds.Add(second);
        */
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        float move = speed * Time.deltaTime;
        rectTransform.anchoredPosition += new Vector2(move, 0);
        Debug.Log("Pos: " + backgrounds[backgrounds.Count - 1].position);
        

        /*
        Debug.Log("Pos: " + rectTransform.anchoredPosition.x);
        Debug.Log("Width: " + rectTransform.rect.width);
        Debug.Log("If: " + rectTransform.anchoredPosition.x % rectTransform.rect.width);
        if(rectTransform.anchoredPosition.x % rectTransform.rect.width == rectTransform.anchoredPosition.x)
        {
            Debug.Log("Add Left");
            //AddNewLeft();
        }
        */
        /*
        if (rectTransform.rect.width % backgrounds[backgrounds.Count - 1].position.x < 5)
        {
            Debug.Log("Add Left");
            //AddNewLeft();
        }
        if (backgrounds[0].position.x > 4000)
        {
            //Destroy(backgrounds[0]);
        }
        */
        /*
        if (backgrounds[backgrounds.Count - 1].anchoredPosition.x == 0)
        {

        }
        */
        /*
        foreach (RectTransform bg in backgrounds)
        {
            bg.anchoredPosition += new Vector2(move, 0);
        }

        RectTransform last = backgrounds[backgrounds.Count - 1];
        if ((last.anchoredPosition.x < Screen.width) || (last.anchoredPosition.x + last.rect.width > 0))
        {
            //AddNewImage();
        }
        */
        /*
        if (backgrounds.Count > 2)
        {
            RectTransform first = backgrounds[0];
            if ((first.anchoredPosition.x > Screen.width) || (first.anchoredPosition.x + first.rect.width < 0))
            {
                Destroy(first.gameObject);
                backgrounds.RemoveAt(0);
            }
        }
        */
    }

    private void AddNewImage()
    {
        /*
        RectTransform last = backgrounds[backgrounds.Count - 1];
        float offset = last.rect.width;
        Vector2 newPos = last.anchoredPosition + new Vector2(offset, 0);

        RectTransform newBG = Instantiate(rectTransform, transform);
        newBG.anchoredPosition = newPos;
        backgrounds.Add(newBG);
        */
    }

    private void AddNewLeft()
    {
        if (backgrounds.Count < 10)
        {
            RectTransform left = Instantiate(rectTransform, transform);
            float lastX = backgrounds[backgrounds.Count - 1].position.x;
            left.anchoredPosition = new Vector2(lastX - rectTransform.rect.width, 0);
            Destroy(left.GetComponent<BackgroundImage>());
            backgrounds.Add(left);
        }
    }
}
