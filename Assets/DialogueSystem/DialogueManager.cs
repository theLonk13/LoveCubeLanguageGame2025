using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


/*
 * This class is the central class managing the dialogue system
 */
public class DialogueManager : MonoBehaviour
{
    // Textboxes for dialogue
    [SerializeField] TextMeshProUGUI nameTextBox;
    [SerializeField] TextMeshProUGUI dialogueTextBox;

    // ChoicesUI
    [SerializeField] GameObject choicesUI;

    // List of sentences
    private List<string> sentences;
    // Tracks where the dialogue manager is in the list of sentences
    private int sentenceTracker = -1;

    // Current dialogue coroutine
    IEnumerator currDialogue;

    // Speed at which characters are displayed
    [SerializeField] float CharacterDisplayDelaySeconds = .05f;
    // TextEventInvokerScript that is currently in use
    private TextEventInvoker currTextEventInvoker;
    void Awake()
    {
        sentences = new List<string>();
    }

    public void StartDialogue(DialogueTextScript dialogue)
    {
        sentences.Clear();
        foreach(string sentence in dialogue.sentences)
        {
            sentences.Add(sentence);
        }
        sentenceTracker = -1;
    }

    public void DisplayNextSentence()
    {
        if(++sentenceTracker >= sentences.Count)
        {
            EndDialogue();
            return;
        }
        
        if(currDialogue != null) { StopCoroutine(currDialogue); }
        string sentence = sentences[sentenceTracker];
        currDialogue = TypeSentence(sentence);
        if (currTextEventInvoker != null)
        {
            currTextEventInvoker.NewSentenceCleanup();
        }
        StartCoroutine(currDialogue);
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueTextBox.text = "";
        string charsToAdd = "";
        bool inTag = false;
        foreach(char letter in sentence)
        {
            if (inTag)
            {
                charsToAdd += letter;
                if (letter == '>')
                {
                    inTag = false;
                    dialogueTextBox.text += charsToAdd;
                    charsToAdd = "";
                    yield return new WaitForSeconds(CharacterDisplayDelaySeconds);
                }
            }else if (!inTag && letter == '<')
            {
                inTag = true;
                charsToAdd += letter;
            }
            else
            {
                dialogueTextBox.text += letter;
                yield return new WaitForSeconds(CharacterDisplayDelaySeconds);
            }
        }
    }

    public void SetNewTextEventInvoker(TextEventInvoker textEventInvoker)
    {
        currTextEventInvoker = textEventInvoker;
    }

    void EndDialogue()
    {
        //Handle End of Dialogue
    }
}
