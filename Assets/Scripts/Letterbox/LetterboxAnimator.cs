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

    // Debug
    // Start: Percentage of screen bar covers at first
    // End: Percentage of screen bar covers at end
    // Duration: Length of time of the animation
    [SerializeField] private Color testUpColor = Color.black;
    [SerializeField] private Color testDownColor = Color.black;
    [SerializeField] private float testUpStart = 0f;
    [SerializeField] private float testUpEnd = 25f;
    [SerializeField] private float testUpDuration = 1f;
    [SerializeField] private float testDownStart = 50f;
    [SerializeField] private float testDownEnd = 25f;
    [SerializeField] private float testDownDuration = 1f;

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
        AnimateTopBar(testUpStart, testUpEnd, testUpDuration);
        topBarVisual.color = testUpColor;
        AnimateBottomBar(testDownStart, testDownEnd, testDownDuration);
        bottomBarVisual.color = testDownColor;
    }
    void Update()
    {
        if (isTopMoving)
        {
            topTimeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(topTimeElapsed / topDuration);
            Vector2 newPos = Vector2.Lerp(topStartPos, topEndPos, t);
            topBar.anchoredPosition = newPos;

            if (t >= 1f) isTopMoving = false;
        }

        if (isBottomMoving)
        {
            bottomTimeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(bottomTimeElapsed / bottomDuration);
            Vector2 newPos = Vector2.Lerp(bottomStartPos, bottomEndPos, t);
            bottomBar.anchoredPosition = newPos;

            if (t >= 1f) isBottomMoving = false;
        }
    }

    public void AnimateTopBar(float startPercent, float endPercent, float duration)
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
    }

    public void AnimateBottomBar(float startPercent, float endPercent, float duration)
    {
        float screenHeight = Screen.height;
        float startY = screenHeight * (startPercent / 100f);
        float endY = screenHeight * (endPercent / 100f);

        bottomStartPos = new Vector2(0, startY);
        bottomEndPos = new Vector2(0, endY);
        bottomDuration = duration;
        bottomTimeElapsed = 0f;
        isBottomMoving = true;
    }
}
