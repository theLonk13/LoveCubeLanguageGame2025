using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] DialogueManager dialogueManager;
    [SerializeField] DialogueTextScript testScript;
    // Start is called before the first frame update
    void Start()
    {
        if (dialogueManager != null && testScript != null)
        {
            dialogueManager.StartDialogue(testScript);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dialogueManager.DisplayNextSentence();
        }
    }
}