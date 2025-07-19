using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCaptchaBG : MonoBehaviour
{
    [SerializeField] GameObject testBG;
    [SerializeField] GameObject testBG2;
    [SerializeField] Vector2 setWorldPos = Vector2.zero;
    float sinTimer = 0.0f;
    [SerializeField] float posMultiplier = 1.0f;
    [SerializeField] float timeMultiplier = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 modifiedWorldPos = new Vector2(setWorldPos.x + posMultiplier * Mathf.Sin(sinTimer), setWorldPos.y);
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, modifiedWorldPos);
        Vector2 anchoredPosition = transform.InverseTransformPoint(screenPoint);
        if(testBG != null) testBG.transform.position = anchoredPosition;
        if(testBG2 != null) testBG2.transform.position = anchoredPosition;
        sinTimer += timeMultiplier * Time.deltaTime;
    }
}
