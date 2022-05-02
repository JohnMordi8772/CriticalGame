using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public CapsuleCollider2D coll;

    public bool grounded = false;
    public bool slipping = false;
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D slide;
    public PhysicsMaterial2D slip;
    public ContactPoint2D[] contacts;
    public List<Vector2> groundNormals;
    public List<Vector2> hits;
    public Vector2 groundNormal;
    public Vector2 point;
    public Vector2 curveCenterBottom;
    public Vector2 curveCenterTop;

    public Vector2 moveDir;
    public Vector2 lastDir;
    public Vector2 finalMove;
    public Vector2 heldVelocity;
    public float moveForce = 150f;
    public float walkSpeed = 10f;
    public float sprintSpeed = 20f;
    public float speedCap = 10f;
    public bool doubleJump = true;

    public Vector2 hopDir;
    public WaitForSeconds jumpCD;
    public float hopAngle = 45f;
    public float hopUp = 60f;
    public float hopOut = 30f;
    public float jumpForce = 200f;
    public bool wallHop = false;
    public int right = 0;
    public float slideForce = 50f;
    public float gravityNegate;

    public KeyCode jump = KeyCode.Space;
    public KeyCode sprint = KeyCode.LeftControl;
    public float sensitivity = 200f;

    //public int itemCount;
    //public int sleepItemCount;
    //public int gameItemCount;
    //public int loseCounter;
    //public GameObject LoseObject;

    //public GameObject[] Last10Pickups = new GameObject[10];
    //public GameObject PickupSweet;
    //public GameObject PickupSleep;
    //public GameObject PickupGame;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        jumpCD = new WaitForSeconds(0.2f);
    }

    void Update()
    {
        lastDir = moveDir;
        moveDir = (transform.right * Input.GetAxisRaw("Horizontal")).normalized;

        if (Input.GetKeyDown(jump))
        {
            StartCoroutine(Jump());
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        //else if (Input.GetKey(KeyCode.R))
        //{
        //    UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        //}

        //LoseCondition();
    }

    void FixedUpdate()
    {
        Movement();

        if (grounded)
        {
            coll.sharedMaterial = normal;
        }
        else if (wallHop)
        {
            coll.sharedMaterial = slide;
        }
        else
        {
            coll.sharedMaterial = slip;
        }
    }

    /// <summary>
    /// Contains the script for walking based on where the player is touching the ground (needs revising for corners)
    /// </summary>
    void Movement()
    {
        if (Input.GetKey(sprint))
        {
            speedCap = sprintSpeed;
        }
        else
        {
            speedCap = walkSpeed;
        }

        if (wallHop == true)
        {
            rb2d.AddForce(Vector2.down * slideForce);
        }

        if (grounded == true)
        {
            if (moveDir != lastDir || rb2d.velocity.magnitude < speedCap)
            {
                finalMove = Vector3.ProjectOnPlane(moveDir, groundNormal);

                rb2d.AddForce(finalMove * moveForce, ForceMode2D.Impulse);
            }
        }
        else
        {
            if (moveDir != lastDir || (rb2d.velocity.x < speedCap && rb2d.velocity.x > -speedCap))
            {
                rb2d.AddForce(moveDir * moveForce, ForceMode2D.Impulse);
            }
        }
    }

    /// <summary>
    /// Used to jump
    /// </summary>
    IEnumerator Jump()
    {
        if (wallHop == true && grounded == false)
        {
            switch (Input.GetAxisRaw("Vertical"))
            {
                case -1:
                    hopDir = Quaternion.Euler(0f, 0f, hopUp * right) * Vector2.up;
                    break;

                case 1:
                    hopDir = Quaternion.Euler(0f, 0f, hopOut * right) * Vector2.up;
                    break;

                default:
                    hopDir = Quaternion.Euler(0f, 0f, hopAngle * right) * Vector2.up;
                    break;
            }

            rb2d.velocity = Vector2.zero;
            hopDir = new Vector2(hopDir.x, hopDir.y * gravityNegate);
            rb2d.AddForce(hopDir * jumpForce, ForceMode2D.Impulse);
        }
        else if (grounded == true || doubleJump == true)
        {
            if (grounded == false)
            {
                doubleJump = false;
                heldVelocity = new Vector2(rb2d.velocity.x, 0f);
                rb2d.velocity = heldVelocity;
            }

            rb2d.AddForce(Vector2.up * jumpForce * gravityNegate, ForceMode2D.Impulse);
        }

        yield return jumpCD;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
        collision.GetContacts(contacts);
        GroundCheck(contacts);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (grounded == false)
        {
            ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contacts);
            GroundCheck(contacts);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.contactCount == 0)
        {
            grounded = false;
            wallHop = false;
        }
        else
        {
            ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contacts);
            GroundCheck(contacts);
        }
    }

    void GroundCheck(ContactPoint2D[] contacts_)
    {
        grounded = false;
        wallHop = false;
        right = -1;

        float curveCenters = (coll.bounds.extents.y - coll.bounds.extents.x);
        curveCenterBottom = coll.bounds.center - Vector3.up * curveCenters;
        curveCenterTop = coll.bounds.center + Vector3.up * curveCenters;

        foreach (ContactPoint2D c in contacts_)
        {
            Vector2 dir = curveCenterBottom - c.point;
            Vector2 dir2 = c.point - curveCenterTop;

            //Ground detect
            if (dir.y > 0f && Mathf.Abs(Vector2.Angle(c.normal, Vector2.up)) <= 40)
            {
                groundNormal = c.normal;

                doubleJump = true;
                grounded = true;
            }
            //Wall check
            else if (dir2.y < 0f)
            {
                if(c.point.x > transform.position.x)
                {
                    right = 1;
                }
                else
                {
                    right = -1;
                }

                doubleJump = true;
                wallHop = true;
            }
        }
    }

    //void LoseCondition()
    //{
        
    //    for (int i = 0; i < 9 ; i++)
    //    {
    //      if(itemCount == 10 || sleepItemCount == 10 || gameItemCount == 10)
    //        {
    //            LoseObject.SetActive(true);
    //        }
    //    }    
    //}
}
