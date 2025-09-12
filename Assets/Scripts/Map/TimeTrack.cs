using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrack : MonoBehaviour
{
    public int days;
    [SerializeField] private bool debug;
    // Start is called before the first frame update
    void Start()
    {
        debugLogDays();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void debugLogDays()
    {
        if (debug)
        {
            Debug.Log("Days:" + days);
        }
    }

    public void addDays(int days)
    {
        this.days += days;
        debugLogDays();
    }

    public void removeDays(int days)
    {
        this.days -= days;
        debugLogDays();
    }
}
