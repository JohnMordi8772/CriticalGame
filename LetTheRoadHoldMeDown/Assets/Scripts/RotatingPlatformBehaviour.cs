using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatformBehaviour : MonoBehaviour
{
    public float rotationSpeed = .1f;

    // Start is called before the first frame update
    void Start()
    {
        rotationSpeed = .1f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 360 * rotationSpeed * Time.deltaTime);
    }
}
