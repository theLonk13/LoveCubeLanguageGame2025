using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Singleton
public class ArtifactsManager : MonoBehaviour
{
    public static ArtifactsManager Instance;
    Artifact[] artifacts = new Artifact[0];
    [SerializeField] GameObject artifactPrefab;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadArtifactSet(ArtifactSet newSet)
    {
        CleanupArtifacts();

        artifacts = new Artifact[newSet.artifacts.Length];
        for(int i = 0; i < newSet.artifacts.Length; i++)
        {
            GameObject newArtifact = GameObject.Instantiate(artifactPrefab, this.gameObject.transform);
            Artifact artifactScript = newArtifact.GetComponent<Artifact>();
            artifactScript.LoadArtifactData(newSet.artifacts[i]);
            artifacts[i] = artifactScript;
        }
        
    }

    void CleanupArtifacts()
    {
        foreach(Artifact artifact in artifacts)
        {
            Destroy(artifact.gameObject);
        }
    }

    public Artifact[] GetArtifacts()
    {
        return artifacts;
    }
}
