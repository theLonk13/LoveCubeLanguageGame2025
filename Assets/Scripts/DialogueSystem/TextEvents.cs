using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using System;
using UnityEditor;

// this script identifies and executes events based on link tags found in text box
public class TextEvents : MonoBehaviour
{
    [SerializeField] ConversationManager conversationMan;
    [SerializeField] DialogueManager dialogueMan;
    [SerializeField] MinigameManager minigameMan;
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
            case "Choice":
                ParseAndSendChoiceData(textEvent.GetLinkText());
                break;
            case "JumpToLine":
                HandleJumpToLine(textEvent.GetLinkText());
                break;
            case "OpenDialogue":
                dialogueMan.ToggleDialogueOpen(true);
                dialogueMan.DisplayNextSentence();
                break;
            case "CloseDialogue":
                dialogueMan.ToggleDialogueOpen(false);
                break;
            case "Captcha":
                ParseAndSendCaptchaData(textEvent.GetLinkText());
                break;
            case "Letterbox":
                ParseAndSendLetterboxData(textEvent.GetLinkText());
                break;
            case "ChangeScript":
                ParseAndChangeDialogueScript(textEvent.GetLinkText());
                break;
            default:
                Debug.Log("default event triggered");
                break;
        }
    }

    // Choices are formatted <;[choice 1 text]; [choice 1 line]; [choice 2 text]; [choice 2 line]; [choice 3 text]; [choice 3 line];>  *********SUBJECT TO CHANGE**********
    // texts are strings; line #s are ints
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

    // Parse and send captcha minigame data
    // Captcha data should be formatted <;[number of symbols required];[modifier];[modifier param] (optional - depends on the modifier);[repeat modifier + modifier param until done];>
    // # of symbols is int; modifier IDs are int; modifier params are floats
    private void ParseAndSendCaptchaData(string linkText)
    {
        string[] splitStrings = linkText.Split(';');
        int numSymbols = int.Parse(splitStrings[1]);
        
        string[] modifierData = new string[splitStrings.Length - 3];
        for (int i = 2; i < splitStrings.Length - 1; i++)
        {
            modifierData[i - 2] = splitStrings[i];
            //Debug.LogFormat($"Adding modifier param element: {modifierData[i - 2]}");
        }
        conversationMan.StartMinigame(numSymbols, modifierData);
    }

    // Letterbox data should be formatted <;[topStart];[topEnd];[topDuration];[botStart];[botEnd];[botDuration];[topColor];[botColor];>
    // Colors are in the format r:g:b
    // All values are floats
    // Use <size=0%> tag to make the letterbox data invisible mid sentence
    private void ParseAndSendLetterboxData(string linkText)
    {
        string[] splitStrings = linkText.Split(';');

        float topStart = 0f;
        float topEnd = 0f;
        float topDuration = 0f;
        float botStart = 0f;
        float botEnd = 0f;
        float botDuration = 0f;
        Color topColor = Color.black;
        Color botColor = Color.black;

        try
        {
            topStart = float.Parse(splitStrings[1]);
            topEnd = float.Parse(splitStrings[2]);
            topDuration = float.Parse(splitStrings[3]);
            botStart = float.Parse(splitStrings[4]);
            botEnd = float.Parse(splitStrings[5]);
            botDuration = float.Parse(splitStrings[6]);
        }
        catch (Exception e)
        {
            Debug.LogFormat($"Error parsing letterbox float data: {e.Message}");
        }

        try
        {
            if (splitStrings[7] != "")
            {
                string[] topColorSeparate = splitStrings[7].Split(':');
                topColor = new Color(float.Parse(topColorSeparate[0]), float.Parse(topColorSeparate[1]), float.Parse(topColorSeparate[2]));
            }
            if(splitStrings[8] != "")
            {
                string[] botColorSeparate = splitStrings[8].Split(':');
                botColor = new Color(float.Parse(botColorSeparate[0]), float.Parse(botColorSeparate[1]), float.Parse(botColorSeparate[2]));
            }
        }
        catch(Exception e)
        {
            Debug.LogFormat($"Error parsing letterbox color data: {e.Message}");
        }

        conversationMan.AnimateLetterbox(topStart, topEnd, topDuration, botStart, botEnd, botDuration, topColor, botColor);
    }

    // Find and change Dialogue objects for a new script
    private void ParseAndChangeDialogueScript(string linkText)
    {
        //Search for assets by name
        string[] assets = AssetDatabase.FindAssets(linkText + " t:DialogueTextScript", new[] {"Assets/DialogueSystem/DialogueScripts/"});
        //Debug.LogFormat($"Found {assets.Length} DialogueTextScript using search term {linkText}");

        //Convert GUIDS to asset path and load asset
        string assetPath = AssetDatabase.GUIDToAssetPath(assets[0]);
        DialogueTextScript newScript = AssetDatabase.LoadAssetAtPath<DialogueTextScript>(assetPath);
        if(newScript == null)
        {
            //Debug.LogFormat($"newScript is null.");
            return;
        }
        dialogueMan.ChangeScripts(newScript);
    }
}
