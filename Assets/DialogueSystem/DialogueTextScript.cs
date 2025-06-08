using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class is a Scriptable object that contains data about a dialogue conversation
 */
[CreateAssetMenu(fileName = "New DialogueTextScript", menuName = "DialogueTextScript")]
public class DialogueTextScript : ScriptableObject
{
    public string speakerName;
    [TextArea(3, 10)]
    public string[] sentences;
}
