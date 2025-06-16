using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGElement : MonoBehaviour
{
    public int layer;

    [SerializeField] private float startPercent;
    [SerializeField] private float endPercent;
    [SerializeField] private float duration;
    [SerializeField] private AnimationCurve movementCurve;
    [SerializeField] private bool loop;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
