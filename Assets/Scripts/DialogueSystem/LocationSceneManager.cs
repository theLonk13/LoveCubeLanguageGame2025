using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationSceneManager : MonoBehaviour
{
    [Header("Dialogue variables")]
    [SerializeField] DialogueManager dialogueMan;
    [Tooltip("Dialogue for when the player visits this location for the first time")]
    [SerializeField] DialogueTextScript firstVisitDialogue;
    [Tooltip("Dialogue for when the player visits this location, NOT on the first time")]
    [SerializeField] DialogueTextScript startingDialogue;

    bool firstVisit = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayLocationDialogue()
    {
        if (firstVisit && dialogueMan != null && firstVisitDialogue != null)
        {
            dialogueMan.StartDialogue(firstVisitDialogue);
            firstVisit = false;
        }
        else if (dialogueMan != null && startingDialogue != null)
        {
            dialogueMan.StartDialogue(startingDialogue);
        }
    }
}
