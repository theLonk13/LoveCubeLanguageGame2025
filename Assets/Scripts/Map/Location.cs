using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Location : MonoBehaviour
{
    public GameObject locationObject; // GameObject set in Start()
    public string locationName;
    [TextArea] public string locationInfo; // Additional description of Location
    [SerializeField] private Sprite mapIcon; // Icon in Map
    private Image im; // Component
    public int timeCost; // Cost to visit location
    public string sceneLoad; // Scene for location to load
    public bool visible; // Whether location is visible in map or not

    [SerializeField] private bool debugMode;
    [SerializeField] private TimeTrack TimeTracker;
    private MapManager MapManager;
    public bool showMorning;
    public bool showAfternoon;
    public bool showEvening;

    void Start()
    {
        locationObject = gameObject;
        im = GetComponent<Image>();
        im.sprite = mapIcon;
        MapManager = FindObjectOfType<MapManager>();
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
        
    }

    public bool IsVisible() { return visible; }

    public void GoToLocation()
    {
        if(timeCost <= TimeTracker.dayUnits)
        {
            TimeTracker.useDayUnit();
            Debug.Log($"{locationName}: Loading {sceneLoad}");
            Debug.Log($"Moving here will take {timeCost}, would you like to go?");
            loadScene();
            
        } else
        {
            Debug.Log($"{locationName}: Cost Too Much: {timeCost} , {TimeTracker.dayUnits}");
            //load dialogue
        }
    }

    public void ClickOnLocation()
    {
        MapManager.showConfirmation(this);
    }
}
