using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 * This class handles the tooltip window whenever the LinkHandlerScript notifies that the mouse has clicked on or hovered over a link in the text
 * Place this on the Canvas and provide the tooltip textbox
 */
public class TooltipHandler : MonoBehaviour
{
    [SerializeField] TooltipDataObject[] tooltipObjects;
    [SerializeField] TextMeshProUGUI tooltipText;

    private void OnEnable()
    {
        LinkHandlerScript.OnClickedOnLinkEvent += GetTooltipInfo;
        LinkHandlerScript.OnHoverOverLinkEvent += GetTooltipInfo;
        LinkHandlerScript.OnCleanupTooltip += ClearTooltip;
    }

    private void OnDisable()
    {
        LinkHandlerScript.OnClickedOnLinkEvent -= GetTooltipInfo;
        LinkHandlerScript.OnHoverOverLinkEvent -= GetTooltipInfo;
        LinkHandlerScript.OnCleanupTooltip -= ClearTooltip;
    }

    private void GetTooltipInfo(string keyword)
    {
        foreach (var tooltip in tooltipObjects)
        {
            if (tooltip.CheckMatch(keyword))
            {
                tooltipText.text = tooltip.text;
                return;
            }
        }

        Debug.Log($"Keyword: {keyword} not found.");
    }

    private void ClearTooltip()
    {
        tooltipText.text = "";
    }
}
