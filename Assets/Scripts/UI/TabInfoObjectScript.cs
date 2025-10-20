using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Script for each instance of the tab info object prefab to manage the info displayed
public class TabInfoObjectScript : MonoBehaviour
{
    [SerializeField] Image infoImage;
    [SerializeField] TextMeshProUGUI infoTextbox;

    public void SetupTabInfoObject(Sprite sprite, string text)
    {
        infoImage.sprite = sprite;
        infoTextbox.text = text;
    }
}
