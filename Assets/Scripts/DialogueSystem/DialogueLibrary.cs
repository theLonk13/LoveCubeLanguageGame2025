using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Artifact Dialogue Library", menuName = "ArtifactDialogueLibrary")]
public class DialogueLibrary : ScriptableObject
{
    public DialogueTextScript[] dialogues;
}
