using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactsManager : MonoBehaviour
{
    Artifact[] artifacts;
    [SerializeField] GameObject artifactPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadArtifactSet(ArtifactSet newSet)
    {
        CleanupArtifacts();

        artifacts = new Artifact[newSet.artifacts.Length];
        for(int i = 0; i < newSet.artifacts.Length; i++)
        {
            GameObject newArtifact = GameObject.Instantiate(artifactPrefab);
            Artifact artifactScript = newArtifact.GetComponent<Artifact>();
            artifactScript.LoadArtifactData(newSet.artifacts[i]);
        }
    }

    void CleanupArtifacts()
    {
        foreach(Artifact artifact in artifacts)
        {
            Destroy(artifact.gameObject);
        }
    }
}
