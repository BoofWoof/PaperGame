using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectHover : MonoBehaviour
{
    public Vector3 startPosition;
    private float timePassed = 0.0f;
    public float frequency = 0.01f;
    public float amplitude = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        timePassed = timePassed + Time.deltaTime;
        float positionsShift = Mathf.Sin(timePassed * 360 * frequency);
        if (timePassed > 1 / frequency)
        {
            timePassed = timePassed - (1 / frequency);
        }
        transform.position = new Vector3(startPosition.x, startPosition.y + amplitude*positionsShift, startPosition.z);
    }
}
