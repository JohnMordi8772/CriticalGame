using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformBehaviour : MonoBehaviour
{
    public float moveSpeed;
    public bool moveLeft;
    public bool moveRight;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 10f;

        InvokeRepeating("FlipDirection", 5, 5);
    }

    // Update is called once per frame
    void Update()
    {

        Move();
      
    }


    void Move()
    {

        transform.position += Vector3.right * moveSpeed * Time.deltaTime;

    }

    void FlipDirection()
    {
        moveSpeed *= -1;
    }
    private void OnTriggerStay2D(Collider2D target)
    {
        if (target.tag == "Player")
        {
            target.transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
    }

    
}
