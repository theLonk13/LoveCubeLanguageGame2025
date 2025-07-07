using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    // Pick which test to check
    public enum Tests
    {
        TestLetterBox1, // Preset variables
        TestBackgroundImage1,
        TestCharacterShadow1,
        TestLetterBoxVariable, // Adjustable variables in inspector
        TestBackgroundImageVariable,
        TestCharacterShadowVariable,
    }
    [Header("Debug")]
    [SerializeField] private bool RunTests = false;
    [SerializeField] private Tests runTest;

    // Game Objects that are used
    [SerializeField] private GameObject LBObj;
    [SerializeField] private GameObject BGObj;
    [SerializeField] private GameObject CharObj;
    [SerializeField] private LetterboxAnimator testLB;
    [SerializeField] private BGElement testBG;
    [SerializeField] private CharacterSprite testChar;

    [Header("Test Variables")]
    [SerializeField] private float startPercent; // Start of movement
    [SerializeField] private float endPercent; // End of movement
    [SerializeField] private float duration; // Length of movement
    [SerializeField] private AnimationCurve movementCurve; // Curve of movement

    [SerializeField] private bool useLoop; // If background repeats
    [SerializeField] private bool isBackground; // If background image needs resize to screen size

    // CharacterSprite variables
    [SerializeField] private bool addShadow; // Add shadow
    [SerializeField] private Color shadowColor; // Color of shadow
    [SerializeField] private Vector2 shadowOffset; // Placement of shadow
    void Start()
    {
        LBObj.SetActive(false);
        BGObj.SetActive(false);
        CharObj.SetActive(false);

        if (RunTests)
        {
            switch (runTest)
            {
                case Tests.TestLetterBox1:
                    LBObj.SetActive(true);
                    testLB.animateTopBar(0, 20, 5, 
                        new AnimationCurve(
                            new Keyframe(0, 0, 0, 3),
                            new Keyframe(0.5f, 0.5f, 1, 1),
                            new Keyframe(0.75f, 0.25f, 1, 2),
                            new Keyframe(1, 1, 3, 0)
                        ));
                    testLB.animateBottomBar(0, 40, 5, AnimationCurve.EaseInOut(0, 0, 1, 1));
                    break;
                case Tests.TestBackgroundImage1:
                    BGObj.SetActive(true);
                    testBG.SetMovement(0, 100, 5, AnimationCurve.EaseInOut(0, 0, 1, 1));
                    testBG.SetLoop(true);
                    testBG.SetIfBackground(true);
                    break;
                case Tests.TestCharacterShadow1:
                    CharObj.SetActive(true);
                    testChar.AddShadow(true);
                    testChar.ShadowSettings(Color.black, new Vector2(3, 3));
                    break;
                case Tests.TestLetterBoxVariable:
                    LBObj.SetActive(true);
                    testLB.animateTopBar(startPercent, endPercent, duration, movementCurve);
                    testLB.animateBottomBar(startPercent, endPercent, duration, movementCurve);
                    break;
                case Tests.TestBackgroundImageVariable:
                    BGObj.SetActive(true);
                    testBG.SetMovement(startPercent, endPercent, duration, movementCurve);
                    testBG.SetLoop(useLoop);
                    testBG.SetIfBackground(isBackground);
                    break;
                case Tests.TestCharacterShadowVariable:
                    CharObj.SetActive(true);
                    testChar.AddShadow(addShadow);
                    testChar.ShadowSettings(shadowColor, shadowOffset);
                    break;
            }
        }
    }
}
