using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//literally the only thing this script does is move the obstacle left (for now)
// in the future this should handle obstacle movement if it will have different settings
public class ObstacleScript : MonoBehaviour
{
    [SerializeField] private float speed = 300f;

    private void Awake()
    {
        transform.localPosition = new Vector3(350, Random.Range(-100, 100), 5);
        Debug.LogFormat($"Initializing new obstacle at {transform.localPosition.x}:{transform.localPosition.y}");
    }
    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x - speed*Time.deltaTime, transform.localPosition.y, transform.localPosition.z);
        if(transform.localPosition.x < -350)
        {
            Destroy(this.gameObject);
        }
    }
}
