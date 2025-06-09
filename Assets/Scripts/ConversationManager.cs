using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * This script manages the activity of DialogueManager, MinigameManager, and LetterboxManager
 * 
 */
public class ConversationManager : MonoBehaviour
{
    [SerializeField] DialogueManager dialogueMan;
    [SerializeField] MinigameManager minigameMan;
    [SerializeField] LetterboxManager letterboxMan;

    private bool showChoices = false; // tracks whether the choices ui should be currently shown
    private bool showChoicesUpdated = false; // tracks whether the value of showChoices has been changed from its previous value, and needs to be updated

    private void Update()
    {
        if(showChoicesUpdated) UpdateShowChoicesHelper();
    }

    public void UpdateShowChoices(bool showChoices)
    {
        this.showChoices = showChoices;
        showChoicesUpdated = true;
    }
    private void UpdateShowChoicesHelper()
    {
        dialogueMan.ShowChoices(showChoices);
        showChoicesUpdated = false;
    }
}
