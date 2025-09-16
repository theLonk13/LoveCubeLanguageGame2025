using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeTrack : MonoBehaviour
{
    public int dayUnits = 0; // Units to spend on a location
    private int dayID = 0; // Corresponds to Day of Week
    [SerializeField] private int unitsPerDay = 6; // How many units used until the next day
    private int unitsUsed = 0;
    public int dayCost = 1; // How many units are used per location

    public int dayPhaseCost = 2; // How many units until next phase of the day
    private int dayPhaseCounter = 0;
    private int dayPhaseID = 0;

    public int weeksPerArc = 3;
    public int week = 0;
    public enum DayTime { Morning, Afternoon, Evening }
    public enum Days { Monday, Tuesday, Wednesday, Thursday, Friday }
    private DayTime dayPhase = DayTime.Morning;
    private Days dayOfWeek;

    // Debug
    [SerializeField] private bool debug;
    [SerializeField] private Text text;
    void Start()
    {
        debugLogData();
        //Start of Day
        setDayUnits(unitsPerDay);
        setDayPhase(0);

    }

    void Update()
    {
        if (debug) debugText();
    }

    public void debugLogData()
    {
        Debug.Log("Day Units:" + dayUnits);
        Debug.Log("Day Units Used" + checkUnitsLeft());
        Debug.Log("Day: " + dayID + ": " + checkDay());
        Debug.Log("Week:" + week);
        Debug.Log("Day Phase: " + dayPhaseID + ": " + dayPhase.ToString());
        Debug.Log("Day Phase Counter: " + dayPhaseCounter);
    }

    public void debugReset()
    {
        dayUnits = 0;
        dayID = 0; // Corresponds to Day of Week
        unitsPerDay = 6;
        unitsUsed = 0;
        dayCost = 1;
        dayPhaseCost = 2;
        dayPhaseCounter = 0;
        dayPhaseID = 0;
        weeksPerArc = 3;
        week = 0;
        dayPhase = DayTime.Morning;
        dayOfWeek = Days.Monday;
    }

    public void debugText()
    {
        text.text = 
            "Day Units:" + dayUnits + "\n" +
            "Day Units Used" + checkUnitsLeft() + "\n" +
            "Day: " + checkDay() + ": " + dayOfWeek.ToString() + "\n" +
            "Week:" + week + "\n" +
            "Day Phase: " + dayPhaseID + ": " + dayPhase.ToString() + "\n" +
            "Day Phase Counter: " + dayPhaseCounter + "\n";

    }

    public void setDayUnits(int dayUnits)
    {
        this.dayUnits = dayUnits;
    }

    public void setDayPhase(int phaseID)
    {
        this.dayPhaseID = phaseID;
        dayPhaseCounter = 0;
        convertDayPhaseID();
    }

    private void nextDayPhase()
    {
        dayPhaseID++;
        if (dayPhaseID > 2)
        {
            dayPhaseID = dayPhaseID % 2 - 1;
        }
        dayPhaseCounter = 0;
        convertDayPhaseID();
    }

    // Main function for locations. Have a button use this function to use a day unit.
    public void useDayUnit()
    {
        dayUnits -= dayCost;
        unitsUsed += dayCost;
        dayPhaseCounter++;
        
        if(dayPhaseCounter == 2)
        {
            nextDayPhase();
        }

        if(dayUnits <= 0)
        {
            if (checkDay() >= 4)
            {
                nextDay();
                nextWeek();
            } else if (checkDay() < 5)
            {
                nextDay();
            }
        }
    }

    public int checkUnitsInDay()
    {
        return unitsPerDay;
    }
    public int checkUnitsLeft()
    {
        return unitsPerDay - unitsUsed;
    }

    public int checkDay()
    {
        return dayID;
    }

    public void setDay(int dayID)
    {
        if (dayID < 0 || dayID > 4)
        {
            Debug.LogError("dayID not valid: " + dayID);
            return;
        }
        this.dayID = dayID;
        convertDayID();
    }

    public void nextDay()
    {
        this.dayID = this.dayID + 1;
        // Incase DayID is above 4 for some reason, set to fit within day ID's
        if(dayID > 4)
        {
            dayID = dayID % 4 - 1;
        }
        dayUnits = unitsPerDay;
        convertDayID();
        setDayPhase(0);
    }

    private void convertDayID()
    {
        switch(dayID)
        {
            case 0:
                dayOfWeek = Days.Monday;
                break;
            case 1:
                dayOfWeek = Days.Tuesday;
                break;
            case 2:
                dayOfWeek = Days.Wednesday;
                break;
            case 3:
                dayOfWeek = Days.Thursday;
                break;
            case 4:
                dayOfWeek = Days.Friday;
                break;
        }
    }

    private void convertDayPhaseID()
    {
        switch(dayPhaseID)
        {
            case 0:
                dayPhase = DayTime.Morning;
                break;
            case 1:
                dayPhase = DayTime.Afternoon;
                break;
            case 2:
                dayPhase = DayTime.Evening;
                break;
        }
    }

    public void nextWeek()
    {
        week++;
        if(week > weeksPerArc)
        {
            // next arc
        }
    }
}
