using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollisions : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        gameObject.transform.parent = gameObject.GetComponentInParent<Transform>();
        Physics2D.IgnoreLayerCollision(10, 11);
        Physics2D.IgnoreLayerCollision(6, 11);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
