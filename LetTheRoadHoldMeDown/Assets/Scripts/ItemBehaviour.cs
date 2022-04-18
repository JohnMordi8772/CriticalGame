using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviour : MonoBehaviour
{
    /*public CapsuleCollider2D cc2d;
    public GameObject Player;
    public PlayerBehaviour pb;
    public bool isChecked;
    public CircleCollider2D circ2d;

    // Start is called before the first frame update
    void Start()
    {
        circ2d = GetComponent<CircleCollider2D>();
        cc2d = GetComponent<CapsuleCollider2D>();
        Player = GameObject.Find("Player");
        pb = Player.GetComponent<PlayerBehaviour>();

        Destroy(circ2d);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.tag == "Sweets")
        {
           cc2d.size = new Vector2(1 + (pb.itemCount),
                1 + (pb.itemCount ));
        }
        if (gameObject.tag == "Sleep")
        {
            cc2d.size = new Vector2(1 + (pb.sleepItemCount),
                1 + (pb.sleepItemCount));
        }
        if (gameObject.tag == "Games")
        {
            cc2d.size = new Vector2(1 + (pb.gameItemCount ),
                1 + (pb.gameItemCount));
        }

        if (pb.itemCount < 0)
        {
            pb.itemCount = 0;
        }
        if (pb.sleepItemCount < 0)
        {
            pb.sleepItemCount = 0;
        }
        if (pb.gameItemCount < 0)
            {
                pb.gameItemCount = 0;
            }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
         if(collision.collider.tag == "Player")
         {
             pb.itemCount++;
             Destroy(gameObject);
         }
     }

    private void OnTriggerStay2D(Collider2D target)
    {
        if(target.tag == "Player")
        {
            if(gameObject.tag == "Sweets")
            {
                target.transform.position = Vector3.MoveTowards(target.transform.position, transform.position, 
                    ((1 + ((pb.itemCount ) * .5f)) * .03f));
            }
            if (gameObject.tag == "Sleep")
            {
                target.transform.position = Vector3.MoveTowards(target.transform.position, transform.position,
                    ((1 + ((pb.sleepItemCount) * .5f)) * .03f));
            }
            if (gameObject.tag == "Games")
            {
                target.transform.position = Vector3.MoveTowards(target.transform.position, transform.position,
                    ((1 + ((pb.gameItemCount) * .5f)) * .03f));
            }

            if (Vector2.Distance(Player.transform.position, gameObject.transform.position) < 1.0f && isChecked == false)
            {

                isChecked = true;

                if (gameObject.tag == "Sweets")
                {
                    pb.itemCount++;
                    pb.sleepItemCount--;
                    pb.gameItemCount--;
                  
                }   
                if (gameObject.tag == "Sleep")
                {
                     pb.itemCount--;
                     pb.sleepItemCount++;
                     pb.gameItemCount--;
                    
                        
                }
                if (gameObject.tag == "Games")
                {
                    pb.itemCount--;
                    pb.sleepItemCount--;
                    pb.gameItemCount++;


                }
                
               
                
                Destroy(gameObject);
            }
        }
    }*/
}
