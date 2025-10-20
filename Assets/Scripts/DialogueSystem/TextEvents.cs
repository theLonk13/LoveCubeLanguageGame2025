using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.UIElements;

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

    private void OnDestroy()
    {
        TextEventInvoker.LinkFound -= CheckEventType;
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
            case "ChangeLanguage":
                dialogueMan.ChangeLanguage(textEvent.GetLinkText());
                break;
            case "Captcha":
                ParseAndSendCaptchaData(textEvent.GetLinkText());
                break;
            case "CaptchaAlign":
                ChangeCaptchaAlignment(textEvent.GetLinkText());
                break;
            case "Letterbox":
                ParseAndSendLetterboxData(textEvent.GetLinkText());
                break;
            case "ChangeScript":
                ParseAndChangeDialogueScript(textEvent.GetLinkText());
                break;
            case "SetupArtifactChoices":
                //use to tell dialogue manager to read artifact list and setup choice ui to display all of the artifacts for player to choose
                Debug.Log("Setting up artifact choices");
                dialogueMan.SetupArtifactChoices();
                break;
            case "ChooseArtifact":
                //use to tell dialogue manager that an artifact has been chosen, and to look up the specific dialogue text script to show
                ParseArtifactChoice(textEvent.GetLinkText());
                break;
            case "TagUnlock":
                ParseTagUnlock(textEvent.GetLinkText());
                break;
            case "TabInfoUnlock":
                //use to unlock a tab info object
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

        //OLD CHOICE SETUP
        /*
        string choice1Text = "";
        string choice2Text = "";
        string choice3Text = "";
        int choice1Line = -5;
        int choice2Line = -5;
        int choice3Line = -5;
        try
        {
            choice1Text = splitStrings[1];
            if(int.TryParse(splitStrings[2], out int result1))
            {
                choice1Line = result1;
            }
            else
            {
                throw new Exception("Choice 1 line failed to parse");
            }
            choice2Text = splitStrings[3];
            if (int.TryParse(splitStrings[4], out int result2))
            {
                choice2Line = result2;
            }
            else
            {
                throw new Exception("Choice 2 line failed to parse");
            }
            choice3Text = splitStrings[5];
            if (int.TryParse(splitStrings[6], out int result3))
            {
                choice3Line = result3;
            }
            else
            {
                throw new Exception("Choice 3 line failed to parse");
            }
        }
        catch (Exception e)
        {
            Debug.LogFormat($"Error during choice parsing: {e.Message}");
            
        }
        Debug.LogFormat($"Text Events: {choice1Text} : {choice1Line} : {choice2Text} : {choice2Line} : {choice3Text} : {choice3Line}");
        dialogueMan.SetupChoices(choice1Text, choice1Line, choice2Text, choice2Line, choice3Text, choice3Line);
        //*/

        //NEW CHOICE SETUP
        string[] choiceTexts = new string[(splitStrings.Length - 2) / 2];
        int[] choiceLines = new int[choiceTexts.Length];
        Debug.Log($"choiceTexts Length : {choiceTexts.Length}\nchoiceLines Length : {choiceLines.Length}");
        for(int i = 1; i < splitStrings.Length - 1; i++)
        {
            if(i%2 == 1) // text element
            {
                choiceTexts[i/2] = splitStrings[i];
            }else if(i%2 == 0) // choice line element
            {
                choiceLines[i/2 - 1] = int.Parse(splitStrings[i]);
            }
        }
        dialogueMan.SetupChoices(choiceTexts, choiceLines);
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
        string reqSymbols = splitStrings[1];
        int botSymbolSet = int.Parse(splitStrings[2]);
        int topSymbolSet = int.Parse(splitStrings[3]);
        
        string[] modifierData = new string[splitStrings.Length - 5];
        for (int i = 4; i < splitStrings.Length - 1; i++)
        {
            modifierData[i - 4] = splitStrings[i];
            //Debug.LogFormat($"Adding modifier param element: {modifierData[i - 2]}");
        }
        conversationMan.StartMinigame(reqSymbols, botSymbolSet, topSymbolSet, modifierData);
    }

    // change the alignment of the captcha minigame
    private void ChangeCaptchaAlignment(string linkText)
    {
        minigameMan.SetCaptchaAlignment(int.Parse(linkText));
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

    private void ParseArtifactChoice(string linkText)
    {
        int artifactID = int.Parse(linkText);
        if(dialogueMan != null)
        {
            dialogueMan.HandleArtifactChoice(artifactID);
        }
    }

    // Parses string formatted <;[artifactID];[unlockedTagID];[disabledTagID];> and unlocks corresponding tag
    private void ParseTagUnlock(string linkText)
    {
        string[] splitStrings = linkText.Split(";");
        int artifactID = int.Parse((splitStrings[1]).Trim());
        int unlockedTagID = int.Parse((splitStrings[2]).Trim());
        int disabledTagID = int.Parse((splitStrings[3]).Trim());
        ArtifactsManager.Instance.TagUnlock(artifactID, unlockedTagID, disabledTagID);
    }
}
