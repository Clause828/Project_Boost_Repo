using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 MovementVector = new Vector3(10f,10f,10f);
    [SerializeField] float period = 2f;
    float MovementFactor; //0 for not moved and 1 for moved
    // Start is called before the first frame update

    Vector3 startingPosition;
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) { return;  }
        
            float cycles = Time.time / period;
            const float tau = Mathf.PI * 2; // tau is 2 pi
            float rawSinWave = Mathf.Sin(cycles * tau);
            MovementFactor = rawSinWave / 2f + 0.5f;


            Vector3 offset = MovementVector * MovementFactor;
            transform.position = startingPosition + offset;
    }
}
