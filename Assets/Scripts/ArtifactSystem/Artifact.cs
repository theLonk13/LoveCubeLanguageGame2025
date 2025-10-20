using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// a prefab to be instantiated with an artifact data object to be interacted with
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

    public string[] tagUnlockStrings = null;
    int[] tagUnlocks = null;

    [Header("Research Level Variables - DEPRECATED")]
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
        tagUnlockStrings = thisArtifactData.tagUnlocks;
        tagUnlocks = new int[tagUnlockStrings.Length];

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
            deskSceneMan.DisplayExpandedTags($"Test expanded tags for artifact ID {artifactID}\n\n{GenerateTagText()}");
        }
    }

    // marks an entry in tagUnlocks as 1, indicating the corresponding tag in tagUnlockStrings has been unlocked
    public bool UnlockTag(int unlockedTagID, int disabledTagID = -1)
    {
        if(unlockedTagID >= 0 && unlockedTagID < tagUnlocks.Length)
        {
            tagUnlocks[unlockedTagID] = 1;
            if(disabledTagID >= 0 && disabledTagID < tagUnlocks.Length)
            {
                tagUnlocks[disabledTagID] = 0;
            }
            return true;
        }
        return false;
    }

    private string GenerateTagText()
    {
        string output = "";

        for(int i = 0; i < tagUnlockStrings.Length; i++)
        {
            if (tagUnlocks[i] == 1)
            {
                output += tagUnlockStrings[i];
            }
        }

        return output;
    }
}
