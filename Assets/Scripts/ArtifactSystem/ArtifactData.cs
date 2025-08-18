using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewArtifactData", menuName = "Artifact Data")]
public class ArtifactData : ScriptableObject
{
    [Header("General variables")]
    public int artifactID = -1;
    public int researchPoints = 0;
    public Sprite artifactArt;

    [Header("Research Level Variables")]
    public int startResearchLevel = 0;
    public int currResearchLevel = 0;
    public int maxResearchLevel = 1;
    [Tooltip("Keywords that unlock through research on this artifact. The index of the keyword indicates the research level where it unlocks")]
    public string[] keywordUnlocks = null;
    [Tooltip("Records that unlock through research on this artifact. The index of the record indicates the research level where it unlocks")]
    public string[] recordUnlocks = null;
    [Tooltip("Locations where this artifact gains research bonuses")]
    public string[] bonusLocations = null;
}
