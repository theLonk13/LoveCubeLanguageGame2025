using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LocationSceneManager : MonoBehaviour
{   
    public enum Location { Library };
    [Header("Location variables")]
    [SerializeField] private Location location;

    [Header("Dialogue variables")]
    [SerializeField] DialogueManager dialogueMan;
    [Tooltip("Dialogue for when the player visits this location for the first time")]
    [SerializeField] DialogueTextScript firstVisitDialogue;
    [Tooltip("Dialogue for when the player visits this location, NOT on the first time")]
    [SerializeField] DialogueTextScript startingDialogue;
    [Tooltip("Main Library for location related dialogue")]
    [SerializeField] LocationDialogueLibrary locDialogueLibrary;

    bool firstVisit = true;

    // Start is called before the first frame update
    void Start()
    {
        //Test invoke the start dialogue
        Invoke("PlayStartingLocationDialogue", 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayStartingLocationDialogue()
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

    public void PlayArtifactLocationDialogue(int artifactID)
    {
        ArtifactsManager artifactMan = ArtifactsManager.Instance;
        if (artifactMan != null)
        {
            int researchLv = artifactMan.FindArtifact(artifactID).currResearchLevel;
            if(locDialogueLibrary != null)
            {
                try
                {
                    DialogueTextScript newDialogue = locDialogueLibrary.artifactLibraries[artifactID - 1].dialogues[researchLv];
                    dialogueMan.ChangeScripts(newDialogue);
                }catch(Exception e)
                {
                    Debug.Log($"Error looking up dialogue of artifact ID {artifactID} with research level {researchLv}: {e.Message}");
                }
            }
        }
    }
}
