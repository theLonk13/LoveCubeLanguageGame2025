using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// This scripts entire purpose is turning on/off the choice buttons. That's it
public class ChoiceUIScript : MonoBehaviour
{
    [SerializeField] private GameObject[] choiceObjects;
    [SerializeField] private TMP_Text[] choiceTexts;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < choiceObjects.Length; i++)
        {
            if (choiceTexts[i].text.Equals("") && choiceObjects[i].activeSelf)
            {
                choiceObjects[i].SetActive(false);
            }
            else if (!choiceTexts[i].text.Equals("") && !choiceObjects[i].activeSelf)
            {
                choiceObjects[i].SetActive(true);
            }
        }
    }
}
