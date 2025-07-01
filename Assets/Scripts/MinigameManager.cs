using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class MinigameManager : MonoBehaviour
{
    // Conversation Manager
    [SerializeField] private ConversationManager conversationMan;

    // Tracks what are the taret symbols for the player
    private Sprite[] requiredSymbols;
    private int[] requiredSymbolsIDs;

    // Temporary array for the possible symbols in the minigame, probably will change to sprites or a scriptable object later
    [SerializeField] private List<Sprite> possibleSymbols = new List<Sprite>();
    [SerializeField] private SymbolList symbolList;

    // Toggle allowing repeat symbols
    [SerializeField] private bool allowRepeatSymbols;

    // Prefab for SymbolFrames
    [SerializeField] private GameObject symbolFramePrefab;
    // SymbolFrames parent obj
    [SerializeField] private GameObject symbolFramesParentObj;

    // tracks which symbol player currently must click
    private int currSymbol = -1;

    // Prefab for clickable symbols in top screen
    [SerializeField] private GameObject symbolClickablePrefab;
    // Top Screen parent
    [SerializeField] private GameObject symbolClickableParent;
    // Array of clickable symbols transforms
    private Transform[] symbolClickableTransforms = new Transform[1];
    // Minimum distance between two clickable symbols
    [Tooltip("Minimum distance between two symbols in the Top Captcha screen")]
    [SerializeField] float minSymbolDistance = 40.0f;

    // Animator for Minigame window
    [SerializeField] private Animator minigameWindowAnimator;

    // Context window
    [SerializeField] private TMP_Text contextTextbox;
    [SerializeField] private Animator contextAnimator;

    // Modifier objects
    [SerializeField] private GameObject staticFilter;
    [SerializeField] private GameObject timerParent;
    private Slider timerSlider;
    private IEnumerator timerEnumerator;
    [SerializeField] private GameObject obstaclesParent;
    private IEnumerator obstacleGenerator;
    [SerializeField] private GameObject obstaclePrefab;
    [Tooltip("Maximum number of obstacles on the screen at a time")]
    [SerializeField] private int maxNumObstacles;

    private void Awake()
    {
        SetupPossibleSymbols();
        if(timerParent != null ) timerSlider = timerParent.GetComponent<Slider>();
    }

    private void SetupPossibleSymbols()
    {
        //possibleSymbols.Clear();
    }

    /*
     * MODIFIER IDS AND EFFECTS
     * -1 - no modifiers
     * 0 - tv static filter
     * 1 - timer; additional params following ID : [timer length (seconds in float)];[line jump # on fail]
     * 2 - obstacles
     */
    public void SetupMinigame(string reqSymbols, int botSymbolSet, int topSymbolSet, string[] modifierParams)
    {
        int numSymbols = 0;
        numSymbols = ParseRequiredSymbols(reqSymbols);
        requiredSymbols = new Sprite[numSymbols];

        // Select symbols based on IDs if given
        possibleSymbols = new List<Sprite>(symbolList.GetSymbolSet(botSymbolSet));

        if (requiredSymbolsIDs[0] != -1)
        {
            for(int i = 0; i < requiredSymbolsIDs.Length; i++)
            {
                requiredSymbols[i] = possibleSymbols[requiredSymbolsIDs[i]];
            }
        }
        else
        {
            List<Sprite> tempPossibleSymbols = new List<Sprite>(symbolList.GetSymbolSet(botSymbolSet));
            for (int index = 0; index < requiredSymbols.Length; index++)
            {
                int thisSymbol = UnityEngine.Random.Range(0, tempPossibleSymbols.Count);
                requiredSymbols[index] = tempPossibleSymbols[thisSymbol];
                requiredSymbolsIDs[index] = possibleSymbols.IndexOf(requiredSymbols[index]);
                if (!allowRepeatSymbols)
                {
                    tempPossibleSymbols.RemoveAt(thisSymbol);
                }
            }
        }
        

        // Display on lower screen
        foreach(Sprite symbol in requiredSymbols)
        {
            GameObject newSymbolFrame = GameObject.Instantiate(symbolFramePrefab, symbolFramesParentObj.transform);
            SymbolFrameScript newSymbolFrameScript = newSymbolFrame.GetComponent<SymbolFrameScript>();
            newSymbolFrameScript.SetupSymbolFrame(possibleSymbols.IndexOf(symbol), symbol);
        }
        currSymbol = 0;

        // Setup Top Screen
        SetupTopScreen(topSymbolSet);

        // Set context text
        if(contextTextbox != null) { contextTextbox.text = modifierParams[0]; }

        // Set up modifiers
        for (int i = 1; i < modifierParams.Length; i++)
        {
            try
            {
                int modifierID = int.Parse(modifierParams[i]);
                switch (modifierID)
                {
                    case 0:
                        Debug.Log("Static filter modifier activated");
                        if(staticFilter != null) staticFilter.SetActive(true);
                        break;
                    case 1: // Dialogue for passing the timer should be immediately after the captcha sentence. Fail leads to a jump somewhere else
                        float timer = float.Parse(modifierParams[++i]);
                        int jumpFail = int.Parse(modifierParams[++i]);
                        Debug.LogFormat($"Timer modifier activated with {timer} seconds. Jump to {jumpFail} on fail.");
                        if(timerParent != null) timerParent.SetActive(true);
                        RunTimerHelper(timer, jumpFail);
                        break;
                    case 2:
                        Debug.Log("Obstacle modifier activated");
                        if(obstaclesParent != null) obstaclesParent.SetActive(true);
                        StartObstacleGenerator();
                        break;
                    default:
                        Debug.LogFormat($"No modifier of ID {modifierID} found.");
                        break;
                }
            }catch(Exception e)
            {
                //Debug.LogFormat($"Modifier param array elemet: {modifierParams[i]}");
                Debug.LogException(e);
            }
        }

        ToggleSymbolHighlight();
        ToggleMinigameWindow(true);
    }

    // Change the alignment int in the captcha's animator
    public void SetCaptchaAlignment(int align)
    {
        minigameWindowAnimator.SetInteger("Alignment", align);
    }

    // Take a string that is formatted as a series of integers separated by ':' and no spaces
    // The first int should be the number of symbols for the captcha, any additional integers are required symbols
    private int ParseRequiredSymbols(string reqSymbolString)
    {
        string[] splitString = reqSymbolString.Split(':');
        int numSymbols = int.Parse(splitString[0]);
        requiredSymbolsIDs = new int[numSymbols];

        if (splitString.Length > 1)
        {
            for (int i = 1; i < splitString.Length; i++)
            {
                requiredSymbolsIDs[i-1] = int.Parse(splitString[i]);
            }
        }
        else
        {
            for(int i = 0; i < requiredSymbolsIDs.Length; i++)
            {
                requiredSymbolsIDs[i] = -1;
            }
        }

        return numSymbols;
    }

    // Spawn symbols in top screen
    private void SetupTopScreen(int topSymbolSet)
    {
        possibleSymbols = new List<Sprite>(symbolList.GetSymbolSet(topSymbolSet));
        symbolClickableTransforms = new Transform[possibleSymbols.Count];
        int index = 0;
        foreach(Sprite symbol in possibleSymbols)
        {
            GameObject newSymbolClickable = GameObject.Instantiate(symbolClickablePrefab, symbolClickableParent.transform);
            SymbolClickableScript newSymbolClickableScript = newSymbolClickable.GetComponent<SymbolClickableScript>();
            newSymbolClickableScript.SetupSymbolClickable(this, possibleSymbols.IndexOf(symbol), symbol);
            symbolClickableTransforms[index] = newSymbolClickable.GetComponent<Transform>();
            index++;
        }

        foreach(Transform transform in symbolClickableTransforms)
        {
            //Debug.LogFormat($"{transform.localPosition}");
        }
    }

    // Helper method to set up timer modifier
    private void RunTimerHelper(float timerLength, int jumpToOnFail)
    {
        if(timerEnumerator != null)
        {
            StopCoroutine(timerEnumerator);
        }
        timerEnumerator = RunTimer(timerLength, jumpToOnFail);
        StartCoroutine(timerEnumerator);
    }

    // Coroutine for timer
    IEnumerator RunTimer(float timerLength, int jumpToOnFail)
    {
        float currTime = 0f;
        while (currTime < timerLength)
        {
            currTime += Time.deltaTime;
            timerSlider.value = (timerLength - currTime)/timerLength;
            yield return null;
        }
        conversationMan.JumpToLine(jumpToOnFail);
        timerEnumerator = null;
    }

    // Helper method to set up obstacle modifier
    private void StartObstacleGenerator()
    {
        if (obstacleGenerator != null)
        {
            StopCoroutine(obstacleGenerator);
        }
        obstacleGenerator = ObstacleGenerator();
        StartCoroutine(obstacleGenerator);
    }

    // Coroutine for obstacle modifier
    IEnumerator ObstacleGenerator()
    {
        int numObstacles = 0;
        while (true)
        {
            numObstacles = obstaclesParent.transform.childCount;
            if(numObstacles < maxNumObstacles)
            {
                if (obstaclePrefab != null)
                {
                    GameObject.Instantiate(obstaclePrefab, obstaclesParent.transform);
                }
            }
            yield return new WaitForSeconds(.2f);
        }
    }

    // Checks that a given location is far enough away from other symbols
    // returns true on valid location, false otherwise
    public bool CheckSymbolClickableSpacing(Transform transform)
    {
        if (transform == null) return false;
        foreach (Transform currSymbolTransform in symbolClickableTransforms)
        {
            if (currSymbolTransform == null) break;
            if (transform != currSymbolTransform && Vector3.Distance(transform.localPosition, currSymbolTransform.localPosition) < minSymbolDistance) return false;
        }
        return true;
    }

    // Handle a symbol being clicked
    public void SymbolClicked(int symbolID)
    {
        if (currSymbol < requiredSymbolsIDs.Length && symbolID == requiredSymbolsIDs[currSymbol])
        {
            currSymbol++;
        }
        else
        {
            ResetCaptcha();
        }

        if(currSymbol >= requiredSymbolsIDs.Length)
        {
            FinishMinigame();
        }
        else
        {
            ToggleSymbolHighlight();
        }
    }

    // Reset captcha when incorrect symbol is clicked
    private void ResetCaptcha()
    {
        currSymbol = 0;
        foreach(Transform transform in symbolClickableTransforms)
        {
            transform.localPosition = Vector3.zero;
        }
        foreach(Transform transform in symbolClickableTransforms)
        {
            transform.gameObject.GetComponent<SymbolClickableScript>().RandomizePosition();
        }
    }

    // Make the current symbol highlight
    private void ToggleSymbolHighlight()
    {
        for (int i = 0; i < requiredSymbolsIDs.Length; i++)
        {
            SymbolFrameScript symbolFrame = symbolFramesParentObj.transform.GetChild(i).GetComponent<SymbolFrameScript>();
            if(symbolFrame != null) {symbolFrame.ToggleHighlight(i == currSymbol); }
        }
    }

    // Handle end of minigame
    private void FinishMinigame()
    {
        ToggleMinigameWindow(false);
        CleanUpMinigameWindow();
        if(conversationMan != null) conversationMan.FinishMinigame();
    }

    // Toggle the minigame window being shown
    private void ToggleMinigameWindow(bool isOpen)
    {
        minigameWindowAnimator.SetBool("Open", isOpen);
        //contextAnimator.SetTrigger("ToggleContextWindow");
    }

    // Clean up the symbols and frames and modifier windows after minigame finishes
    private void CleanUpMinigameWindow()
    {
        int numSymbolFrames = symbolFramesParentObj.transform.childCount;
        for (int i = 0; i < numSymbolFrames; i++)
        {
            Destroy(symbolFramesParentObj.transform.GetChild(i).gameObject);
        }

        int numSymbolClickables = symbolClickableParent.transform.childCount;
        for (int i = 0; i < numSymbolClickables; i++)
        {
            Destroy(symbolClickableParent.transform.GetChild(i).gameObject);
        }

        if(staticFilter != null) staticFilter.SetActive(false);
        if(timerParent != null) timerParent.SetActive(false);
        if (timerEnumerator != null)
        {
            StopCoroutine(timerEnumerator);
            timerEnumerator = null;
        }
        if(obstacleGenerator != null)
        {
            StopCoroutine(obstacleGenerator);
            obstacleGenerator = null;
        }
        if (obstaclesParent != null)
        {
            int numObstacles = obstaclesParent.transform.childCount;
            for (int i = 0; i < numObstacles; i++)
            {
                Destroy(obstaclesParent.transform.GetChild(i).gameObject);
            }
            obstaclesParent.SetActive(false);
        }
    }
}
