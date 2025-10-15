using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Artifact : MonoBehaviour
{
    ArtifactData thisArtifactData;

    [Header("General variables")]
    public string artifactName = "";
    public int artifactID = -1;
    public int researchPoints = 0;
    public Sprite artifactArt;
    [SerializeField] Image image;
    [SerializeField] RectTransform rectTransform;
    DeskSceneManager deskSceneMan;
    [SerializeField] Button thisArtifactButton;

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
        artifactName = thisArtifactData.artifactName;
        artifactArt = thisArtifactData.artifactArt;
        artifactID = thisArtifactData.artifactID;
        startResearchLevel = thisArtifactData.startResearchLevel;
        currResearchLevel = thisArtifactData.currResearchLevel;
        maxResearchLevel = thisArtifactData.maxResearchLevel;
        keywordUnlocks = thisArtifactData.keywordUnlocks;
        recordUnlocks = thisArtifactData.recordUnlocks;
        bonusLocations = thisArtifactData.bonusLocations;

        if (artifactArt != null && image != null)
        {
            image.sprite = artifactArt;
        }
    }

    public void LoadArtifactData(ArtifactData newData)
    {
        thisArtifactData = newData;
        SetupArtifactData();
    }

    public void MoveToScreenLocation(GameObject newLoc)
    {
        this.gameObject.transform.SetParent(newLoc.transform, false);
        rectTransform.localPosition = Vector3.zero;
    }

    public void SetDeskSceneManager(DeskSceneManager deskMan)
    {
        deskSceneMan = deskMan;
    }

    public void SetAsDeskArtifact()
    {
        thisArtifactButton.onClick.RemoveAllListeners();
        thisArtifactButton.onClick.AddListener(DeskOnClick);
    }

    public void DeskOnClick()
    {
        Debug.LogFormat($"DeskOnClick for button {this.ToString()} with ID {artifactID} activated");
        if(deskSceneMan != null)
        {
            deskSceneMan.DisplayExpandedTags($"Test expanded tags for artifact ID {artifactID}");
        }
    }
}
