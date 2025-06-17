using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGElement : MonoBehaviour
{
    public int layer;

    [SerializeField] private float startPercent = 0f;
    [SerializeField] private float endPercent = 100f;
    [SerializeField] private float duration = 1f;
    [SerializeField] private AnimationCurve movementCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private bool loop = true;
    [SerializeField] private bool tiled = false; // Duplicates image to the left and right for backgrounds
    [SerializeField] private bool useCurve = false;

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


    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPos = rectTransform.anchoredPosition;

        startVal = screenSize * (startPercent / 100f);
        endVal = screenSize * (endPercent / 100f);
        startPos = new Vector2(startVal, 0);
        endPos = new Vector2(endVal, 0);

        width = rectTransform.rect.width;
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
}
