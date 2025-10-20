using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabsUIScript : MonoBehaviour
{
    [SerializeField] Animator animator;

    [Header("Tabs")]
    [SerializeField] RectTransform peopleTab;

    [Header("Scripts for predetermined info slots in tabs")]
    [SerializeField] TabInfoObjectScript[] peopleInfoObjects;
    [SerializeField] TabInfoObjectScript[] placesInfoObjects;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowTab(int tabNum)
    {
        if (animator != null)
        {
            animator.SetInteger("ShowTab", tabNum);
        }
    }
}
