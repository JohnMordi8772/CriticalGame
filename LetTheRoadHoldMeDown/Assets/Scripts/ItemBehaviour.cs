using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviour : MonoBehaviour
{
    public CapsuleCollider2D cc2d;
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
        cc2d.size = new Vector2(1 + (pb.itemCount), 1 + (pb.itemCount));
    }

   /* private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
        {
            pb.itemCount++;
            Destroy(gameObject);
        }
    }*/

    private void OnTriggerStay2D(Collider2D target)
    {
        if(target.tag == "Player")
        {
            target.transform.position = Vector3.MoveTowards(target.transform.position, transform.position, ((1 + (pb.itemCount * .1f)) * .03f));

            if(Vector2.Distance(Player.transform.position, gameObject.transform.position) < .75f && isChecked == false)
            {
                isChecked = true;
                pb.itemCount++;
                Destroy(gameObject);
            }
        }
    }
}
