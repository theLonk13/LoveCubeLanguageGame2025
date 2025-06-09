using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using System;

// this script identifies and executes events based on link tags found in text box
public class TextEvents : MonoBehaviour
{
    [SerializeField] DialogueManager dialogueMan;
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
            case "Choice":
                ParseAndSendChoiceData(textEvent.GetLinkText());
                break;
            case "JumpToLine":
                HandleJumpToLine(textEvent.GetLinkText());
                break;
            case "CloseDialogue":
                break;
            default:
                Debug.Log("default event triggered");
                break;
        }
    }

    // Choices are formatted <;[choice 1 text]; [choice 1 line]; [choice 2 text]; [choice 2 line]; [choice 3 text]; [choice 3 line];>  *********SUBJECT TO CHANGE**********
    // Remember to put the closing semi colon
    private void ParseAndSendChoiceData(string linkText)
    {
        string[] splitStrings = linkText.Split(';');
        Debug.Log(splitStrings.ToString());
        //if (splitStrings.Length < 6) { return; }

        string choice1Text = "";
        string choice2Text = "";
        string choice3Text = "";
        int choice1Line = -5;
        int choice2Line = -5;
        int choice3Line = -5;
        try
        {
            choice1Text = splitStrings[1];
            choice1Line = int.Parse(splitStrings[2]);
            choice2Text = splitStrings[3];
            choice2Line = int.Parse(splitStrings[4]);
            choice3Text = splitStrings[5];
            choice3Line = int.Parse(splitStrings[6]);
        }
        catch (Exception e)
        {
            Debug.LogFormat($"Error during choice parsing: {e.Message}");
            
        }
        Debug.LogFormat($"Text Events: {choice1Text} : {choice1Line} : {choice2Text} : {choice2Line} : {choice3Text} : {choice3Line}");
        dialogueMan.SetupChoices(choice1Text, choice1Line, choice2Text, choice2Line, choice3Text, choice3Line);
    }

    // Handles a line jump in script
    // Should be a singular integer
    private void HandleJumpToLine(string linkText)
    {
        int lineJump = int.Parse(linkText);
        dialogueMan.JumpToLine(lineJump);
    }
}
