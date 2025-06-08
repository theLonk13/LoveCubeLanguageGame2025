using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// this script identifies and executes events based on link tags found in text box
public class TextEvents : MonoBehaviour
{
    private void Awake()
    {
        TextEventInvoker.LinkFound += CheckEventType;
    }

    public void CheckEventType(TMP_LinkInfo textEvent)
    {
        switch (textEvent.GetLinkID())
        {
            case "testEvent1":
                Debug.Log("test1 event triggered");
                break;
            case "testEvent2":
                Debug.Log("test2 event triggered");
                break;
            default:
                Debug.Log("default event triggered");
                break;
        }
    }
}
