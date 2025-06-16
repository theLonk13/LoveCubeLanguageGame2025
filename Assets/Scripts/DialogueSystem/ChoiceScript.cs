using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChoiceScript : MonoBehaviour
{
    private DialogueManager dialogueMan;
    [SerializeField] private TMP_Text textbox;

    private string choiceText; // text to be displayed to player on choice option
    private int choiceLineIndex; // backend line index for dialogue manager to find the correct dialogue branch
    [SerializeField] private int choiceIndex; // number that signifies what choice number this is in the dialogue

    private void Awake()
    {
        dialogueMan = GameObject.FindFirstObjectByType<DialogueManager>();
    }

    private void Update()
    {
        if(textbox.text == "") { this.gameObject.SetActive(false); }
        else { this.gameObject.SetActive(true); }
    }

    public void SelectChoice()
    {
        if (dialogueMan != null)
        {
            dialogueMan.SelectChoice(choiceIndex);
        }
    }
}
