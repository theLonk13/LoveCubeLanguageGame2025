using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterboxAnimator : MonoBehaviour
{

    public RectTransform topBar;
    public RectTransform bottomBar;
    public Image topBarVisual;
    public Image bottomBarVisual;

    private Vector2 topStartPos;
    private Vector2 topEndPos;
    private float topDuration;
    private float topTimeElapsed;

    private Vector2 bottomStartPos;
    private Vector2 bottomEndPos;
    private float bottomDuration;
    private float bottomTimeElapsed;

    private bool isTopMoving = false;
    private bool isBottomMoving = false;

    // Color Changing
    private Color topColorStart;
    private Color topColorEnd;
    private float topColorDuration;
    private float topColorTimeElapsed;
    private bool isTopColorChanging = false;

    private Color bottomColorStart;
    private Color bottomColorEnd;
    private float bottomColorDuration;
    private float bottomColorTimeElapsed;
    private bool isBottomColorChanging = false;


    // Debug
    // Start: Percentage of screen bar covers at first
    // End: Percentage of screen bar covers at end
    // Duration: Length of time of the animation
    [SerializeField] private bool testColorChange = false;

    [SerializeField] private Color testUpColor = Color.black;
    [SerializeField] private Color testDownColor = Color.black;
    [SerializeField] private float testUpStart = 0f;
    [SerializeField] private float testUpEnd = 25f;
    [SerializeField] private float testUpDuration = 1f;
    [SerializeField] private float testDownStart = 50f;
    [SerializeField] private float testDownEnd = 25f;
    [SerializeField] private float testDownDuration = 1f;

    public bool useTopMovementCurve = true;
    public bool useBottomMovementCurve = true;
    public AnimationCurve topMovementCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public AnimationCurve bottomMovementCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [SerializeField] private Color testTopColorChange1 = Color.black;
    [SerializeField] private Color testTopColorChange2 = Color.black;
    [SerializeField] private Color testBottomColorChange1 = Color.black;
    [SerializeField] private Color testBottomColorChange2 = Color.black;
    [SerializeField] private float testTopColorDuration = 1f;
    [SerializeField] private float testBottomColorDuration = 1f;



    void Start()
    {
        // Set Default properties for top and bottom bars
        float screenHeight = Screen.height;
        float barHeight = screenHeight * 0.5f;
        if (topBar != null)
        {
            topBar.anchorMin = new Vector2(0, 1);
            topBar.anchorMax = new Vector2(1, 1);
            topBar.pivot = new Vector2(0.5f, 1);
            topBar.sizeDelta = new Vector2(0, barHeight);
            topBar.anchoredPosition = new Vector2(0, 0);
            topBarVisual.color = Color.black;
        }
        if (bottomBar != null)
        {
            bottomBar.anchorMin = new Vector2(0, 0);
            bottomBar.anchorMax = new Vector2(1, 0);
            bottomBar.pivot = new Vector2(0.5f, 1);
            bottomBar.sizeDelta = new Vector2(0, barHeight);
            bottomBar.anchoredPosition = new Vector2(0, 0);
            bottomBarVisual.color = Color.black;
        }

        //Testing
        AnimateTopBar(testUpStart, testUpEnd, testUpDuration, topMovementCurve);
        setTopColor(testUpColor);
        AnimateBottomBar(testDownStart, testDownEnd, testDownDuration);
        setBottomColor(testDownColor);
        if(testColorChange)
        {
            animateTopColorChange(testTopColorChange1, testTopColorChange2, testTopColorDuration);
            animateBottomColorChange(testBottomColorChange1, testBottomColorChange2, testBottomColorDuration);
        }
    }
    void Update()
    {
        if (isTopMoving)
        {
            topTimeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(topTimeElapsed / topDuration);
            if(useTopMovementCurve) t = topMovementCurve.Evaluate(t);
            topBar.anchoredPosition = Vector2.Lerp(topStartPos, topEndPos, t);

            if (t >= 1f) isTopMoving = false;
        }

        if (isBottomMoving)
        {
            bottomTimeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(bottomTimeElapsed / bottomDuration);
            if (useBottomMovementCurve) t = topMovementCurve.Evaluate(t);
            bottomBar.anchoredPosition = Vector2.Lerp(bottomStartPos, bottomEndPos, t);

            if (t >= 1f) isBottomMoving = false;
        }

        if (isTopColorChanging)
        {
            topColorTimeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(topColorTimeElapsed / topColorDuration);
            topBarVisual.color = Color.Lerp(topColorStart, topColorEnd, t);
            if(t>= 1f) isTopColorChanging = false;
        }

        if (isBottomColorChanging)
        {
            bottomColorTimeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(bottomColorTimeElapsed / bottomColorDuration);
            bottomBarVisual.color = Color.Lerp(bottomColorStart, bottomColorEnd, t);
            if (t >= 1f) isBottomColorChanging = false;
        }
    }

    public void setTopBar(float percent)
    {
        float screenHeight = Screen.height;
        float barHeight = screenHeight * 0.5f;
        float y = screenHeight * (1f - (percent / 100f)) - barHeight;

        topBar.anchoredPosition = new Vector2(0, y);
    }

    public void setBottomBar(float percent)
    {
        float screenHeight = Screen.height;
        float barHeight = screenHeight * 0.5f;
        float y = screenHeight * (percent / 100f);

        bottomBar.anchoredPosition = new Vector2(0, y);
    }

    public void AnimateTopBar(float startPercent, float endPercent, float duration, AnimationCurve curve = null)
    {
        float screenHeight = Screen.height;
        float barHeight = screenHeight * 0.5f;
        float startY = screenHeight * (1f - (startPercent / 100f)) - barHeight;
        float endY = screenHeight * (1f - (endPercent / 100f)) - barHeight;

        topStartPos = new Vector2(0, startY);
        topEndPos = new Vector2(0, endY);
        topDuration = duration;
        topTimeElapsed = 0f;
        isTopMoving = true;

        if (curve != null) useTopMovementCurve = true;
        topMovementCurve = curve;
    }

    public void AnimateBottomBar(float startPercent, float endPercent, float duration, AnimationCurve curve = null)
    {
        float screenHeight = Screen.height;
        float startY = screenHeight * (startPercent / 100f);
        float endY = screenHeight * (endPercent / 100f);

        bottomStartPos = new Vector2(0, startY);
        bottomEndPos = new Vector2(0, endY);
        bottomDuration = duration;
        bottomTimeElapsed = 0f;
        isBottomMoving = true;

        if (curve != null) useBottomMovementCurve = true;
        bottomMovementCurve = curve;
    }

    public void setTopColor(Color color)
    {
        topBarVisual.color = color;
    }

    public void setBottomColor(Color color)
    {
        bottomBarVisual.color = color;
    }

    public void animateTopColorChange(Color color1, Color color2, float duration)
    {
        topColorStart = color1;
        topColorEnd = color2;
        topColorDuration = duration;
        topColorTimeElapsed = 0f;
        isTopColorChanging = true;
    }

    public void animateBottomColorChange(Color color1, Color color2, float duration)
    {
        bottomColorStart = color1;
        bottomColorEnd = color2;
        bottomColorDuration = duration;
        bottomColorTimeElapsed = 0f;
        isBottomColorChanging = true;
    }

    public void setZTopLayer(int layer)
    {

    }

    public void setZBottomLayer(int layer)
    {

    }
}
