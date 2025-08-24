using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Location : MonoBehaviour
{
    public GameObject location;
    public string locationName;
    [TextArea] public string locationInfo;
    public int timeCost;
    public string sceneLoad;
    public bool visible;

    [SerializeField] private bool debugMode;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void loadScene()
    {
        Debug.Log($"{locationName}: Loading {sceneLoad}");
        if (sceneLoad == null || sceneLoad == "") Debug.Log($"{locationName}: sceneLoad empty");
        SceneManager.LoadScene(sceneLoad);
    }

    // Update is called once per frame
    void Update()
    {
        if(debugMode)
        {
            //SetVisible(visible);
        }
    }

    public bool IsVisible() { return visible; }

    public void SetVisible(bool isVisible)
    {
        Debug.Log($"{locationName}: Set Visible: {visible}");
        visible = isVisible;
        location.SetActive(isVisible);
    }
}
