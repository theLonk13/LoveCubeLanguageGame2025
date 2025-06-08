using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;

public class TextEventInvoker : MonoBehaviour
{
    [SerializeField] private TMP_Text textbox;
    public static event Action<string> LinkFound;

    private int eventsTriggered = 0;

    private void OnEnable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(CheckForLink);
    }
    private void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(CheckForLink);
    }
    private void Awake()
    {
        textbox = GetComponent<TMP_Text>();
        NotifyDialogueManager();

    }
    private void CheckForLink(UnityEngine.Object obj)
    {
        //Debug.Log("Checking for link");
        if (obj != textbox) return;

        int numLinks = textbox.textInfo.linkCount;
        if (numLinks <= 0) return;
        //Debug.LogFormat($"{numLinks} link(s) found");

        
        for (int linkIndex = 0; linkIndex < numLinks; linkIndex++)
        {
            TMP_LinkInfo linkInfo = textbox.textInfo.linkInfo[linkIndex];

            //Debug.LogFormat($"{textbox.text.Length} character(s) shown, {linkInfo.linkTextfirstCharacterIndex}");
            if(eventsTriggered <= linkIndex)
            {
                //Debug.LogFormat($"Attempting to trigger event number {eventsTriggered}");
                LinkFound?.Invoke(linkInfo.GetLinkID());
                eventsTriggered++;
                break;
            }
        }
    }

    // Notifies the DialogueManager that this is now the TextEventInvoker in use
    private void NotifyDialogueManager()
    {
        DialogueManager dialogueMan = GameObject.FindFirstObjectByType<DialogueManager>();
        if (dialogueMan != null)
        {
            dialogueMan.SetNewTextEventInvoker(this);
        }
    }

    // Cleanup any variables for a new sentence; called in the DisplayNextSentence method of DialogueManager
    public void NewSentenceCleanup()
    {
        eventsTriggered = 0;
    }
}
