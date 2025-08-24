using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskSceneManager : MonoBehaviour
{
    private ArtifactsManager artifactMan;
    private Artifact[] artifacts;

    [SerializeField] GameObject[] artifactLocations;

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
            Debug.Log(artifactLocations[i]);
            Debug.Log(artifacts[i]);
            artifacts[i].MoveToScreenLocation(artifactLocations[i]);
        }
    }

    public void debug_PlaceArtifacts()
    {
        PlaceArtifacts();
    }
}
