using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeskSceneManager : MonoBehaviour
{
    private ArtifactsManager artifactMan;
    private Artifact[] artifacts;

    [SerializeField] GameObject[] artifactLocations;

    [SerializeField] Animator expandedTagsAnim;
    [SerializeField] TextMeshProUGUI expandedTagsText;
    bool showExpandedTags = false;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
        //PlaceArtifacts();
    }

    // Update is called once per frame
    void Update()
    {
        if(artifactMan == null) { LoadArtifacts(); }
    }

    public void LoadArtifacts()
    {
        artifactMan = ArtifactsManager.Instance;
        artifacts = artifactMan.GetArtifacts();

        Debug.Log(artifacts.Length);
    }

    void PlaceArtifacts()
    {
        for (int i = 0; i < artifactLocations.Length && i < artifacts.Length; i++)
        {
            //Debug.Log(artifactLocations[i]);
            //Debug.Log(artifacts[i]);
            artifacts[i].MoveToScreenLocation(artifactLocations[i]);
            artifacts[i].SetDeskSceneManager(this);
            artifacts[i].SetAsDeskArtifact();
        }
    }

    public void DisplayExpandedTags(string artifactInfo = null)
    {
        if(artifactInfo == null)
        {
            showExpandedTags = false;
            expandedTagsAnim.SetBool("ShowExpandedTags", false);
            return;
        }

        if(expandedTagsAnim != null)
        {
            showExpandedTags = !showExpandedTags;
            expandedTagsAnim.SetBool("ShowExpandedTags", showExpandedTags);
        }
        if(expandedTagsText != null)
        {
            expandedTagsText.text = artifactInfo;
        }
    }

    public void debug_PlaceArtifacts()
    {
        PlaceArtifacts();
    }
}
