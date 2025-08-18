using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : MonoBehaviour
{
    ArtifactData thisArtifactData;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetupArtifactData()
    {
        artifactArt = thisArtifactData.artifactArt;
        artifactID = thisArtifactData.artifactID;
        startResearchLevel = thisArtifactData.startResearchLevel;
        currResearchLevel = thisArtifactData.currResearchLevel;
        maxResearchLevel = thisArtifactData.maxResearchLevel;
        keywordUnlocks = thisArtifactData.keywordUnlocks;
        recordUnlocks = thisArtifactData.recordUnlocks;
        bonusLocations = thisArtifactData.bonusLocations;
    }

    public void LoadArtifactData(ArtifactData newData)
    {
        thisArtifactData = newData;
        SetupArtifactData();
    }
}
