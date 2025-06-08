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

    private void Awake()
    {
        dialogueMan = GameObject.FindFirstObjectByType<DialogueManager>();
    }
}
