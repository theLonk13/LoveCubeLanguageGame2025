using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class BreakThroughScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI breakthroughTextbox;
    [TextArea(3, 10)]
    [SerializeField] string breakthroughText = "";
    [TextArea(3, 10)]
    [SerializeField] string[] correctChoices;

    [SerializeField] WordDialSelectorUI wordDialScript;
    TMP_LinkInfo currLinkInfo;
    string oldLinkText = "";
    string oldWord = "";

    // Start is called before the first frame update
    void Start()
    {
        if(breakthroughTextbox != null)
        {
            breakthroughTextbox.text = breakthroughText;
        }
    }

    private void Awake()
    {
        LinkHandlerScript.OnClickedOnLinkEventLinkInfo += OpenWordDial;
    }

    private void OnDestroy()
    {
        LinkHandlerScript.OnClickedOnLinkEventLinkInfo -= OpenWordDial;
    }

    // </link>[word displayed]</style="Invis">;choice1;choice2;choice3;etc.;</link></style>
    public void OpenWordDial(TMP_LinkInfo linkInfo)
    {
        //Debug.Log("Attempting to process linkInfo in OpenWordDial");
        currLinkInfo = linkInfo;
        oldLinkText = linkInfo.GetLinkText();
        
        string[] splitString = linkInfo.GetLinkText().Split(";");
        oldWord = RemoveTags(splitString[0]);

        Debug.Log($"Old Link Text: {oldLinkText}\nOld Word: {oldWord}");

        string[] wordOptions = new string[splitString.Length - 2];
        for(int i = 1; i < splitString.Length - 1; i++)
        {
            wordOptions[i-1] = splitString[i];
        }

        if(wordDialScript != null)
        {
            //Debug.Log("Attempting to open word dial");
            wordDialScript.OpenWordSelector(oldWord, wordOptions, this);
        }
    }

    public void SelectWord(string newWord)
    {
        string newLinkText = GenerateNewLinkText(newWord);
        oldLinkText = ModifyLinkTextForReplace(oldLinkText);
        newLinkText = ModifyLinkTextForReplace(newLinkText);

        Debug.Log($"Modified Old Link Text: {oldLinkText}\nModified New Link Text: {newLinkText}");
        string newText = breakthroughTextbox.text.Replace(oldLinkText, newLinkText);
        breakthroughTextbox.text = newText;
    }

    string RemoveTags(string input)
    {
        string output = string.Empty;
        string charsToAdd = "";
        bool inTag = false;
        foreach (char letter in input)
        {
            if (inTag)
            {
                charsToAdd += letter;
                if (letter == '>')
                {
                    inTag = false;
                    //output += charsToAdd;
                    //charsToAdd = "";
                }
            }
            else if (!inTag && letter == '<')
            {
                inTag = true;
                //charsToAdd += letter;
            }
            else
            {
                output += letter;
            }
        }
        return output;
    }

    // Custom method for doctoring up link text to be usuable in SelectWord method
    string ModifyLinkTextForReplace(string linkText)
    {
        string[] splitString = linkText.Split(";");
        string[] newSplitString = new string[splitString.Length];
        Debug.Log($"Split string length: {splitString.Length}");
        newSplitString[0] = splitString[0] + "<style=\"Invis\">";
        //newSplitString[1] = "<style=\"Invis\">";
        for(int i = 1; i < splitString.Length; i++)
        {
            newSplitString[i] = splitString[i];
        }

        string output = "";
        
        for(int i = 0; i < newSplitString.Length - 1; i++)
        {
            output += newSplitString[i] + ";";
        }

        return output;
    }

    string GenerateNewLinkText(string newWord)
    {
        string[] oldSplitString = oldLinkText.Split(";");
        oldSplitString[0] = newWord;

        string output = "";

        for (int i = 0; i < oldSplitString.Length - 1; i++)
        {
            output += oldSplitString[i] + ";";
        }

        return output;
    }

    public bool SubmitBreakthrough()
    {
        TMP_LinkInfo linkInfo;
        string[] splitString;
        try
        {
            for(int i = 0; i < correctChoices.Length; i++)
            {
                linkInfo = breakthroughTextbox.textInfo.linkInfo[i];
                splitString = linkInfo.GetLinkText().Split(";");
                if (!splitString[0].Contains(correctChoices[i]))
                {
                    return false;
                }
            }
        }catch(Exception e)
        {

        }


        return true;
    }

    public void debug_CheckBreakthrough()
    {
        Debug.Log($"Checking if breakthrough is correct: {SubmitBreakthrough()}");
    }
}
