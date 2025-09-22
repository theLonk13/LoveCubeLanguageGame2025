using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WordDialSelectorUI : MonoBehaviour
{
    [SerializeField] string[] wordOptions;
    [SerializeField] TextMeshProUGUI optionText;
    [SerializeField] Animator animator;

    int currOptionIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(wordOptions != null && wordOptions.Length >= 1)
        {
            optionText.text = wordOptions[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.mouseScrollDelta.y != 0)
        {
            ScrollOptions(Input.mouseScrollDelta.y);
        }
    }

    public void ScrollOptions(float input)
    {
        if(input < 0f)
        {
            currOptionIndex--;
            if(currOptionIndex < 0)
            {
                currOptionIndex = wordOptions.Length - 1;
            }
        }
        else
        {
            currOptionIndex++;
            if (currOptionIndex >= wordOptions.Length)
            {
                currOptionIndex = 0;
            }
        }

        optionText.text = wordOptions[currOptionIndex];
    }

    // Placeholder game object will be the script that requires the output of this selector
    public void OpenWordSelector(string[] options, GameObject PLACEHOLDER)
    {
        wordOptions = options;
        // TODO make the script that will take the output of this selector and display it
    }

    void SelectWord()
    {
        // TODO send selected word to the expectant script
    }
}
