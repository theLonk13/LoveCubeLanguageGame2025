using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class BGElement : MonoBehaviour
{
    public float startPercent = 0f;
    public float endPercent = 100f;
    public float duration = 1f;
    public AnimationCurve movementCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public bool loop = true;

    public bool tiled = false; // Duplicates image to the left and right for backgrounds
    public bool useCurve = false;
    public bool isBackground = false;
    public int layerIndex = 0;
    public bool hasShadow = false;
    public Color shadowColor = Color.black;
    public Vector2 shadowOffset = new Vector2(1, 1);

    private RectTransform rectTransform;
    private Vector2 originalPos;
    private Vector2 startPos;
    private Vector2 endPos;
    private float timeElapsed = 0f;
    private float screenSize = Screen.width;
    private float startVal;
    private float endVal;
    private RectTransform leftRect;
    private RectTransform rightRect;
    private float width;
    GameObject shadow;

    private List<RectTransform> backgrounds = new List<RectTransform>();
    public float bgSpeed = 10f;


    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        loadImage();
    }

    void Update()
    {

        timeElapsed += Time.deltaTime;
        float t = Mathf.Clamp01(timeElapsed / duration);
        if (useCurve) t = movementCurve.Evaluate(t);
        rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
        /*
        if (!isBackground)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
        } else
        {
            
            rectTransform.anchoredPosition += new Vector2(bgSpeed * Time.deltaTime, 0);
            foreach (RectTransform bg in backgrounds)
            {
                bg.anchoredPosition += new Vector2(bgSpeed * Time.deltaTime, 0);
            }

            RectTransform last = backgrounds[backgrounds.Count - 1];
            if((last.anchoredPosition.x < Screen.width) || (last.anchoredPosition.x + last.rect.width > 0))
            {
                createRightImage();
            }

            if(backgrounds.Count > 2)
            {
                RectTransform first = backgrounds[0];
                if((first.anchoredPosition.x > Screen.width) || (first.anchoredPosition.x + first.rect.width < 0))
                {
                    Destroy(first.gameObject);
                    backgrounds.RemoveAt(0);
                }
            }
            
        }
        */

        if (tiled)
        {
            leftRect.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x - width, rectTransform.anchoredPosition.y);
            rightRect.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x + width, rectTransform.anchoredPosition.y);
        }

        if(t >= 1f && loop)
        {
            //Debug.Log("Loop A");
            timeElapsed = 0f;
            rectTransform.anchoredPosition = originalPos;
        }

        /*
        if(t >= 1f && loop && isBackground)
        {
            Debug.Log("Loop B");
            //timeElapsed = 0f;
            createRightImage();
        }
        */

        if(hasShadow)
        {
            shadow.transform.localPosition = transform.localPosition;
        }

    }

    public void loadImage()
    {
        if (hasShadow)
        {
            addShadow(shadowColor, shadowOffset);
        }
        originalPos = rectTransform.anchoredPosition;

        if (isBackground)
        {
            resize(rectTransform);
            //createRightImage();
        }

        startVal = screenSize * (startPercent / 100f);
        endVal = screenSize * (endPercent / 100f);
        startPos = new Vector2(startVal, 0);
        endPos = new Vector2(endVal, 0);

        width = rectTransform.rect.width;
        setLayer(layerIndex);
        if (tiled)
        {
            createLoopImages();
        }
    }

    private void createLoopImages()
    {
        GameObject left = Instantiate(gameObject, transform.parent);
        GameObject right = Instantiate(gameObject, transform.parent);

        Destroy(left.GetComponent<BGElement>());
        Destroy(right.GetComponent<BGElement>());

        leftRect = left.GetComponent<RectTransform>();
        rightRect = right.GetComponent<RectTransform>();

        leftRect.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x - width, rectTransform.anchoredPosition.y);
        rightRect.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x + width, rectTransform.anchoredPosition.y);

        left.transform.SetSiblingIndex(transform.GetSiblingIndex());
        right.transform.SetSiblingIndex(transform.GetSiblingIndex());
    }

    private void createRightImage()
    {
        GameObject right = Instantiate(gameObject, transform.parent);
        Destroy(right.GetComponent<BGElement>());

        rightRect = right.GetComponent<RectTransform>();
        backgrounds.Add(rightRect);
        rightRect.anchoredPosition = new Vector2(originalPos.x + width, originalPos.y);

    }

    private void resize(RectTransform rectTransform)
    {
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }

    private void setLayer(int index)
    {
        rectTransform.transform.SetSiblingIndex(index);
    }

    private void addShadow(Color color, Vector3 offset)
    {
        shadow = Instantiate(gameObject, transform);
        shadow.transform.SetParent(null);
        Destroy(shadow.GetComponent<BGElement>());
        if (shadow.GetComponent<Image>() == null) return; // Image Empty Error
        Image shadowImage = shadow.GetComponent<Image>();
        shadowImage.color = color;

        shadow.transform.position = transform.position + offset;
        shadow.transform.SetParent(transform.parent);
        transform.SetParent(shadow.transform);
    }

    public void SetMovement(float startPercent, float endPercent, float duration, AnimationCurve curve = null)
    {
        this.startPercent = startPercent;
        this.endPercent = endPercent;
        this.duration = duration;
        if(curve != null)
        {
            this.movementCurve = curve;
            this.useCurve = true;
        } else
        {
            this.useCurve = false;
        }
    }

    public void SetLoop(bool loop)
    {
        this.loop = loop;
    }

    public void SetIfBackground(bool ifBackground)
    {
        isBackground = ifBackground;
    }

    public void SetLayer(int layer)
    {
        layerIndex = layer;
    }

    public void AddShadow(bool hasShadow)
    {
        this.hasShadow = hasShadow;
    }

    public void ShadowSettings(Color shadowColor, Vector2 shadowOffset)
    {
        this.shadowColor = shadowColor;
        this.shadowOffset = shadowOffset;
    }

    public void SetTiled(bool tiled)
    {
        this.tiled = tiled;
    }

    public void DebugVar()
    {
        Debug.Log("Loop: " + loop);
    }
}
