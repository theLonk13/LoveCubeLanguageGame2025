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
    [SerializeField] LetterboxAnimator letterboxMan;

    private bool showChoices = false; // tracks whether the choices ui should be currently shown
    private bool showChoicesUpdated = false; // tracks whether the value of showChoices has been changed from its previous value, and needs to be updated

    private bool activateMinigameFlag = false; // tracks whether the minigame should be open and if it already has been opened
    private string[] captchaParams = { "" };
    private int captchaNumSymbols = -1;

    private void Update()
    {
        if(showChoicesUpdated) UpdateShowChoicesHelper();
        if (activateMinigameFlag) StartMinigameHelper();
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

    public void StartMinigame(int numSymbols, string[] modifierData)
    {
        captchaNumSymbols = numSymbols;
        captchaParams = modifierData;
        activateMinigameFlag = true;
    }

    private void StartMinigameHelper()
    {
        minigameMan.SetupMinigame(captchaNumSymbols, captchaParams);
        dialogueMan.ToggleDialogueOpen(false);
        activateMinigameFlag = false;
    }

    public void FinishMinigame(int result = 1)
    {
        dialogueMan.ToggleDialogueOpen(true);
        dialogueMan.DisplayNextSentence();
    }

    public void AnimateLetterbox(float topStart, float topEnd, float topDuration, float botStart, float botEnd, float botDuration)
    {
        letterboxMan.AnimateTopBar(topStart, topEnd, topDuration);
        letterboxMan.AnimateBottomBar(botStart, botEnd, botDuration);
    }

    public void AnimateLetterbox(float topStart, float topEnd, float topDuration, float botStart, float botEnd, float botDuration, Color topColor, Color botColor)
    {
        letterboxMan.AnimateTopBar(topStart, topEnd, topDuration);
        letterboxMan.AnimateBottomBar(botStart, botEnd, botDuration);
    }
}
