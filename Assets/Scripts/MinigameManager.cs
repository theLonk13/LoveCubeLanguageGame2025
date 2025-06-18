using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class MinigameManager : MonoBehaviour
{
    // Conversation Manager
    [SerializeField] private ConversationManager conversationMan;

    // Tracks what are the taret symbols for the player
    private Color[] requiredSymbols;
    private int[] requiredSymbolsIDs;

    // Temporary array for the possible symbols in the minigame, probably will change to sprites or a scriptable object later
    [SerializeField] private List<Color> possibleSymbols = new List<Color>();

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

    // Modifier objects
    [SerializeField] private GameObject staticFilter;
    private void Awake()
    {
        SetupPossibleSymbols();
    }

    private void SetupPossibleSymbols()
    {
        possibleSymbols.Clear();
        possibleSymbols.Add(Color.white);
        possibleSymbols.Add(Color.black);
        possibleSymbols.Add(Color.blue);
        possibleSymbols.Add(Color.red);
        possibleSymbols.Add(Color.green);
        possibleSymbols.Add(Color.yellow);
    }

    /*
     * MODIFIER IDS AND EFFECTS
     * -1 - no modifiers
     * 0 - tv static filter
     * 1 - timer; additional params following ID : [timer length (seconds in float)];[line jump # on fail];[line jump # on success]
     * 2 - obstacles
     */
    public void SetupMinigame(int numSymbols, string[] modifierParams)
    {
        // Select symbols at random
        requiredSymbols = new Color[numSymbols];
        requiredSymbolsIDs = new int[numSymbols];
        List<Color> tempPossibleSymbols = new List<Color>(possibleSymbols);
        for(int index = 0; index < requiredSymbols.Length; index++)
        {
            int thisSymbol = UnityEngine.Random.Range(0, tempPossibleSymbols.Count);
            requiredSymbols[index] = tempPossibleSymbols[thisSymbol];
            requiredSymbolsIDs[index] = possibleSymbols.IndexOf(requiredSymbols[index]);
            if(!allowRepeatSymbols)
            {
                tempPossibleSymbols.RemoveAt(thisSymbol);
            }
        }

        // Display on lower screen
        foreach(Color symbol in requiredSymbols)
        {
            GameObject newSymbolFrame = GameObject.Instantiate(symbolFramePrefab, symbolFramesParentObj.transform);
            SymbolFrameScript newSymbolFrameScript = newSymbolFrame.GetComponent<SymbolFrameScript>();
            newSymbolFrameScript.SetupSymbolFrame(possibleSymbols.IndexOf(symbol), symbol);
        }
        currSymbol = 0;

        // Setup Top Screen
        SetupTopScreen();

        // Set up modifiers
        for (int i = 0; i < modifierParams.Length; i++)
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
                    case 1:
                        float timer = float.Parse(modifierParams[++i]);
                        int jumpFail = int.Parse(modifierParams[++i]);
                        int jumpSuccess = int.Parse(modifierParams[++i]);
                        Debug.LogFormat($"Timer modifier activated with {timer} seconds. Jump to {jumpFail} on fail. Jump to {jumpSuccess} on success");
                        break;
                    case 2:
                        Debug.Log("Obstacle modifier activated");
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
        ToggleMinigameWindow(true);
    }

    private void SetupTopScreen()
    {
        symbolClickableTransforms = new Transform[possibleSymbols.Count];
        int index = 0;
        foreach(Color symbol in possibleSymbols)
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

    public void SymbolClicked(int symbolID)
    {
        if (currSymbol < requiredSymbolsIDs.Length && symbolID == requiredSymbolsIDs[currSymbol])
        {
            currSymbol++;
        }
        else
        {
            Debug.LogFormat($"Wrong symbol! {symbolID}");
        }

        if(currSymbol >= requiredSymbolsIDs.Length)
        {
            FinishMinigame();
        }
    }

    private void FinishMinigame()
    {
        ToggleMinigameWindow(false);
        CleanUpMinigameWindow();
        if(conversationMan != null) conversationMan.FinishMinigame();
    }

    private void ToggleMinigameWindow(bool isOpen)
    {
        minigameWindowAnimator.SetBool("Open", isOpen);
    }

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
    }
}
