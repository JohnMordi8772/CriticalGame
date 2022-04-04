using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public CapsuleCollider2D coll;

    public bool grounded = false;
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D slip;
    public ContactPoint2D[] contacts;
    public List<Vector2> groundNormals;
    public List<Vector2> hits;
    public Vector2 groundNormal;
    public Vector2 point;
    public Vector2 pointDir;
    public Vector2 curveCenterBottom;

    public Vector2 moveDir;
    public float moveForce;
    public float walkSpeed = 10f;
    public float sprintSpeed = 20f;
    public float crouchSpeed = 5f;
    public float speedCap = 10f;

    public Vector2 finalMove;
    
    public float jumpForce = 200f;
    public bool jumped = false;

    public bool crouched = false;

    public KeyCode jump = KeyCode.Space;
    public KeyCode sprint = KeyCode.LeftControl;
    public KeyCode crouch = KeyCode.LeftShift;
    public float sensitivity = 200f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        normal.friction = 1;
        slip.friction = 0;
    }

    void Update()
    {
        moveDir = (transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical")).normalized;
        Crouch();

        if (Input.GetKeyDown(jump))
        {
            Jump();
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        else if (Input.GetKey(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

    void FixedUpdate()
    {
        if (grounded == true)
        {
            coll.sharedMaterial = normal;

            if (moveDir != Vector2.zero)
            {
                Movement();
            }
        }
        else
        {
            coll.sharedMaterial = slip;
        }
    }

    /// <summary>
    /// Calls GroundCheck, inserting its contact points
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        contacts = new ContactPoint2D[collision.contactCount];
        collision.GetContacts(contacts);
        GroundCheck(contacts);
    }

    /// <summary>
    /// Calls GroundCheck, inserting its contact points, only called if ungrounded while still colliding with something
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (grounded == false)
        {
            contacts = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contacts);
            GroundCheck(contacts);
        }
    }

    /// <summary>
    /// Alters camera height and collider height to simulate crouching
    /// </summary>
    void Crouch()
    {
        if (Input.GetKeyDown(crouch))
        {
            coll.size = new Vector2(coll.size.x, coll.size.y / 1.5f);
            crouched = true;
        }

        if (Input.GetKeyUp(crouch))
        {
            coll.size = new Vector2(coll.size.x, coll.size.y * 1.5f);
            crouched = false;
        }
    }

    /// <summary>
    /// Contains the script for walking based on where the player is touching the ground (needs revising for corners)
    /// </summary>
    void Movement()
    {
        if (crouched == true)
        {
            speedCap = crouchSpeed;
        }
        else if (Input.GetKey(sprint))
        {
            speedCap = sprintSpeed;
        }
        else
        {
            speedCap = walkSpeed;
        }

        if (rb2d.velocity.magnitude < speedCap)
        {
            finalMove = Vector3.ProjectOnPlane(moveDir, groundNormal);

            rb2d.AddForce(finalMove * moveForce, ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// Used to jump
    /// </summary>
    void Jump()
    {
        if (Input.GetKeyDown(jump))
        {
            if (grounded == true)
            {
                rb2d.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
                jumped = true;
            }
        }
    }

    float xRotation;

    /// <summary>
    /// Script used to find a grounding or wall hop-off point
    /// </summary>
    /// <param name="contacts_">
    /// Contacts gathered when OnCollisionEnter is called
    /// </param>
    void GroundCheck(ContactPoint2D[] contacts_)
    {
        groundNormals = new List<Vector2>();
        hits = new List<Vector2>();
        point = Vector3.zero;
        groundNormal = Vector3.zero;
        jumped = true;

        curveCenterBottom = coll.bounds.center - Vector3.up * (coll.bounds.extents.y - coll.bounds.extents.x);
        curveCenterBottom = 

        foreach (ContactPoint2D c in contacts_)
        {
            Vector3 dir = curveCenterBottom - c.point;

            if (dir.y > 0f && Mathf.Abs(Vector3.Angle(c.normal, Vector3.up)) <= 40)
            {
                groundNormals.Add(c.normal);
                hits.Add(c.point);

                grounded = true;
                jumped = false;
            }
        }

        //Ground normal calculation
        if (grounded == true)
        {
            if (groundNormals.Count == 1)
            {
                groundNormal = groundNormals[0];
                point = hits[0];
            }
            else
            {
                for (int i = 0; i < groundNormals.Count; i++)
                {
                    groundNormal += groundNormals[i];
                    point += hits[i];

                    groundNormal /= 2;
                    point /= 2;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.contactCount == 0)
        {
            grounded = false;
        }
        else
        {
            contacts = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contacts);
            GroundCheck(contacts);
        }
    }
}
