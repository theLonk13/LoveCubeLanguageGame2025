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


    // Start is called before the first frame update
    void Start()
    {
        if (hasShadow)
        {
            addShadow(shadowColor, shadowOffset);
        }

        rectTransform = GetComponent<RectTransform>();
        originalPos = rectTransform.anchoredPosition;
        if (isBackground)
        {
            resize(rectTransform);
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

    // Update is called once per frame
    void Update()
    {

        timeElapsed += Time.deltaTime;
        float t = Mathf.Clamp01(timeElapsed / duration);
        if (useCurve) t = movementCurve.Evaluate(t);
        rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);

        if (tiled)
        {
            leftRect.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x - width, rectTransform.anchoredPosition.y);
            rightRect.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x + width, rectTransform.anchoredPosition.y);
        }

        if(t >= 1f && loop)
        {
            timeElapsed = 0f;
            rectTransform.anchoredPosition = originalPos;
        }

        if(hasShadow)
        {
            shadow.transform.localPosition = transform.localPosition;
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

    private void resize(RectTransform rectTransform)
    {
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }

    public void setLayer(int index)
    {
        rectTransform.transform.SetSiblingIndex(index);
    }

    public void addShadow(Color color, Vector3 offset)
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
}
