using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


/*
 * This class is the central class managing the dialogue system
 */
public class DialogueManager : MonoBehaviour
{
    // Conversation Manager
    [SerializeField] ConversationManager conversationMan;

    // Textboxes for dialogue
    [SerializeField] TextMeshProUGUI nameTextBox;
    [SerializeField] TextMeshProUGUI dialogueTextBox;
    [SerializeField] TextMeshProUGUI languageTextBox;

    // ChoicesUI
    [SerializeField] GameObject choicesUI;

    // List of sentences
    private List<string> sentences;
    // Tracks where the dialogue manager is in the list of sentences
    private int sentenceTracker = -1;

    // Current dialogue coroutine
    IEnumerator currDialogue;
    // Bool to track if text is being typed currently
    bool typing = false;
    // Strings to track name and sentence currently spoken
    private string currSpeakerName = "";
    private string currLanguageSpoken = "";
    private string currSentenceSpoken = "";

    // Speed at which characters are displayed
    [SerializeField] float CharacterDisplayDelaySeconds = .05f;
    // TextEventInvokerScript that is currently in use
    private TextEventInvoker currTextEventInvoker;
    // Tracks what lines in the script the dialogue should jump to when players make a choice
    private int[] choiceLineJumpIndices = new int[3];
    // Choice textboxes
    [SerializeField] private TMP_Text[] choiceTextboxes;

    // dialoguebox animator
    [SerializeField] private Animator dialogueAnim;

    void Awake()
    {
        sentences = new List<string>();
        choicesUI.SetActive(false);
    }

    public void StartDialogue(DialogueTextScript dialogue)
    {
        sentences.Clear();
        foreach(string sentence in dialogue.sentences)
        {
            sentences.Add(sentence);
        }
        sentenceTracker = -1;

        nameTextBox.text = dialogue.speakerName;
        dialogueAnim.SetBool("Open", true);

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (choicesUI.activeSelf) { return; }
        if (typing)
        {
            StopCoroutine(currDialogue);
            dialogueTextBox.text = currSentenceSpoken;
            typing = false;
            return;
        }else if (currDialogue != null) { StopCoroutine(currDialogue); }

        if (sentenceTracker == -5 || sentenceTracker + 1 >= sentences.Count) // -5 signifies end of dialogue or a situation where the dialogue box should close
        {
            EndDialogue();
            return;
        }

        string sentence = sentences[++sentenceTracker];
        
        // Parse speaker name and dialogue content
        string[] currSentenceSplit = sentence.Split('#');
        currSpeakerName = currSentenceSplit[0];
        currSentenceSpoken = currSentenceSplit[1];

        nameTextBox.text = currSpeakerName;
        currDialogue = TypeSentence(currSentenceSpoken);
        if (currTextEventInvoker != null)
        {
            currTextEventInvoker.NewSentenceCleanup();
        }
        StartCoroutine(currDialogue);
    }

    IEnumerator TypeSentence(string sentence)
    {
        typing = true;
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
        typing = false;
        StopCoroutine(currDialogue);
        currDialogue = null;
    }

    public void SetNewTextEventInvoker(TextEventInvoker textEventInvoker)
    {
        currTextEventInvoker = textEventInvoker;
    }

    void EndDialogue()
    {
        dialogueAnim.SetBool("Open", false);
        //Handle End of Dialogue
    }

    //Sets up choice ui with the texts and configures the lines that the choices should lead to
    public void SetupChoices(string choice1Text, int choice1Line, string choice2Text, int choice2Line, string choice3Text, int choice3Line)
    {
        //Debug.LogFormat($"Dialogue Man: {choice1Text} : {choice1Line} : {choice2Text} : {choice2Line} : {choice3Text} : {choice3Line}");
        choiceTextboxes[0].text = choice1Text;
        choiceLineJumpIndices[0] = choice1Line;
        choiceTextboxes[1].text = choice2Text;
        choiceLineJumpIndices[1] = choice2Line;
        choiceTextboxes[2].text = choice3Text;
        choiceLineJumpIndices[2] = choice3Line;
        conversationMan.UpdateShowChoices(true);
    }

    //Sets up dialogue to flow accordingly to the choice player has selected
    public void SelectChoice(int choiceIndex)
    {
        ShowChoices(false);
        if (choiceLineJumpIndices[choiceIndex] < -1)
        {
            ToggleDialogueOpen(false);
            return;
        }
        sentenceTracker = choiceLineJumpIndices[choiceIndex] - 1;
        DisplayNextSentence();
    }

    public void ShowChoices(bool show)
    {
        choicesUI.SetActive(show);
    }

    public void JumpToLine(int line)
    {
        Debug.LogFormat($"Jumping to line{line}");
        sentenceTracker = line - 1;
    }

    public void ToggleDialogueOpen(bool show)
    {
        dialogueAnim.SetBool("Open", show);
    }

    public void ChangeScripts(DialogueTextScript newScript)
    {
        StartDialogue(newScript);
    }

    public void ChangeLanguage(string language)
    {
        languageTextBox.text = $"SPEAKING {language.ToUpper()}";
    }
}
