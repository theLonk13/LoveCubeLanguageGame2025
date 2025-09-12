using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationScene : MonoBehaviour
{
    [SerializeField] DialogueManager DialogueManager;
    [SerializeField] DialogueTextScript locationScript;
    void Start()
    {
        // Test Dialogue
        Debug.Log(locationScript.sentences[0]);
        DialogueManager.StartDialogue(locationScript);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
