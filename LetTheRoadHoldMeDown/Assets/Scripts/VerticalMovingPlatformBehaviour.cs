using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMovingPlatformBehaviour : MonoBehaviour
{
    public float moveSpeed;
    public bool moveLeft;
    public bool moveRight;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 10f;

        InvokeRepeating("FlipDirection", 6, 6);
    }

    // Update is called once per frame
    void Update()
    {

        Move();
       
    }


    void Move()
    {

        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

    }

    void FlipDirection()
    {
        moveSpeed *= -1;
    }
    private void OnTriggerStay2D(Collider2D target)
    {
        if (target.tag == "Player")
        {
            target.transform.parent = gameObject.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            collision.transform.parent = null;
    }
}
