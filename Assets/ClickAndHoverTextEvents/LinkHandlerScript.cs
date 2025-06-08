using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 * This script detects mouse click and hover events over the given textbox and invokes delegates to handle each event
 * Place this on the GameObject that contains the textbox you wish to detect events for
 */
[RequireComponent(typeof(TMP_Text))]
public class LinkHandlerScript : MonoBehaviour, IPointerClickHandler
{

    private TMP_Text textBox;
    private Canvas canvas;
    [SerializeField] Camera cameraToUse;

    [SerializeField] Boolean clickEventsEnabled = false;
    [SerializeField] Boolean hoverEventsEnabled = false;

    public delegate void ClickOnLinkEvent(string keyword);
    public static event ClickOnLinkEvent OnClickedOnLinkEvent;

    public delegate void HoverOverLinkEvent(string keyword);
    public static event HoverOverLinkEvent OnHoverOverLinkEvent;

    public delegate void CleanupTooltip();
    public static event CleanupTooltip OnCleanupTooltip;

    void Awake()
    {
        textBox = GetComponent<TMP_Text>();
        canvas = GetComponentInParent<Canvas>();

        //*
        if(canvas != null || canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            cameraToUse = null;
        }
        else
        {
            cameraToUse = canvas.worldCamera;
        }
        //*/
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!clickEventsEnabled) return;

        Vector3 mousePos = new Vector3(eventData.position.x, eventData.position.y, 0);

        int linkTaggedText = TMP_TextUtilities.FindIntersectingLink(textBox, mousePos, cameraToUse);
        if (linkTaggedText != -1)
        {
            TMP_LinkInfo linkInfo = textBox.textInfo.linkInfo[linkTaggedText];
            OnClickedOnLinkEvent?.Invoke(linkInfo.GetLinkText());
        }
        else
        {
            OnCleanupTooltip.Invoke();
        }
    }

    void CheckMouseHover()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        int linkTaggedText = TMP_TextUtilities.FindIntersectingLink(textBox, mousePos, cameraToUse);
        if (linkTaggedText != -1)
        {
            TMP_LinkInfo linkInfo = textBox.textInfo.linkInfo[linkTaggedText];
            OnHoverOverLinkEvent?.Invoke(linkInfo.GetLinkID());
        }
        else
        {
            OnCleanupTooltip.Invoke();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(hoverEventsEnabled) CheckMouseHover();
    }
}
