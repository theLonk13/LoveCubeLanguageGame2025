using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewArtifactSet", menuName = "Artifact Set")]
public class ArtifactSet : ScriptableObject
{
    public ArtifactData[] artifacts;
}
