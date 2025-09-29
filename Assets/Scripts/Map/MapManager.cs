using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private TimeTrack timeTracker;
    [SerializeField] private Location[] listOfLocations;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updateLocationVisibility();
    }

    // Looks through all locations and checks if they're supposed to be visible in a certain day phase
    public void updateLocationVisibility()
    {
        int dayPhase = timeTracker.getDayPhaseID();
        foreach (Location l in listOfLocations)
        {
            bool showLocation = true;
            switch (dayPhase)
            {
                case 0:
                    showLocation = l.showMorning;
                    break;
                case 1:
                    showLocation = l.showAfternoon;
                    break;
                case 2:
                    showLocation = l.showEvening;
                    break;
            }
            l.locationObject.SetActive(showLocation);
        }
    }
}
