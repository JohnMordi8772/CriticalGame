using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{

    public GameObject SweetsDoor;
    public GameObject SleepDoor;
    public GameObject GamesDoor;
    public bool inRange;
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        inRange = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(inRange == true)
        {
            if (gameObject.name == "SweetsDoor")
            {
                if (Input.GetKeyDown("e"))
                {
                    Player.transform.position = SleepDoor.transform.position;
                }
            }
            if (gameObject.name == "SleepDoor")
            {
                if (Input.GetKeyDown("e"))
                {
                    Player.transform.position = GamesDoor.transform.position;
                }
            }
            if (gameObject.name == "GamesDoor")
            {
                if (Input.GetKeyDown("e"))
                {
                    Player.transform.position = SweetsDoor.transform.position;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D target)
    {
        if(target.tag == "Player")
        {
            inRange = true;
           
        }

        
    }
    private void OnTriggerExit2D(Collider2D target)
    {
        inRange = false;
    }
}
