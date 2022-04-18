using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportingPlatformBehaviour : MonoBehaviour
{

    public float xPos1;
    public float xPos2;
    public bool xPos1Check;
    public bool xPos2Check;
    // Start is called before the first frame update
    void Start()
    {
        xPos1 = transform.position.x;
        xPos2 = xPos1 + 10;
        InvokeRepeating("FlipPos", 3f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FlipPos()
    {
        if(transform.position.x == xPos1)
        {
            xPos1Check = true;
            xPos2Check = false;
            transform.position += new Vector3(10, 0, 0);
        }

        else if(transform.position.x == xPos2)  
        {
            xPos1Check = false;
            xPos2Check = true;
            transform.position -= new Vector3(10, 0, 0);
        }
        
    }
}
