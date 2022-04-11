using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public CapsuleCollider2D coll;
    public SpriteRenderer sr;

    public bool grounded = false;
    public bool slipping = false;
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D slip;
    public ContactPoint2D[] contacts;
    public List<Vector2> groundNormals;
    public List<Vector2> hits;
    public Vector2 groundNormal;
    public Vector2 point;
    public Vector2 curveCenterBottom;
    public Vector2 curveCenterTop;

    public Vector2 moveDir;
    public Vector2 hopDir;
    public float moveForce = 150f;
    public float walkSpeed = 10f;
    public float sprintSpeed = 20f;
    public float crouchSpeed = 5f;
    public float airSpeed = 3.5f;
    public float speedCap = 10f;
    public float ungroundedModifier = 2f;

    public Vector2 finalMove;
    
    public float jumpForce = 200f;
    public bool wallHop = false;
    public bool right = false;

    public bool crouched = false;

    public KeyCode jump = KeyCode.Space;
    public KeyCode sprint = KeyCode.LeftControl;
    public KeyCode crouch = KeyCode.LeftShift;
    public float sensitivity = 200f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        moveDir = (transform.right * Input.GetAxisRaw("Horizontal")).normalized;
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
        if (moveDir != Vector2.zero)
        {
            Movement();
        }

        if (slipping != true)
        {
            coll.sharedMaterial = normal;
        }
        else
        {
            coll.sharedMaterial = slip;
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
            sr.size = new Vector2(sr.size.x, sr.size.y / 1.5f);

            crouched = true;
        }

        if (Input.GetKeyUp(crouch))
        {
            coll.size = new Vector2(coll.size.x, coll.size.y * 1.5f);
            sr.size = new Vector2(sr.size.x, sr.size.y * 1.5f);

            crouched = false;
        }
    }

    /// <summary>
    /// Contains the script for walking based on where the player is touching the ground (needs revising for corners)
    /// </summary>
    void Movement()
    {
        if (grounded == true)
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

            if (rb2d.velocity.magnitude < speedCap || moveDir.normalized != rb2d.velocity.normalized)
            {
                finalMove = Vector3.ProjectOnPlane(moveDir, groundNormal);

                rb2d.AddForce(finalMove * moveForce, ForceMode2D.Impulse);
            }
        }
        else
        {
            speedCap = airSpeed;

            if (rb2d.velocity.x < speedCap && rb2d.velocity.x > -speedCap && rb2d.velocity.x != moveDir.x)
            {
                rb2d.AddForce(moveDir * moveForce, ForceMode2D.Impulse);
            }
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
            }
            else if (wallHop == true)
            {
                if (right)
                {
                    hopDir = Quaternion.Euler(0f, 0f, 45f) * Vector2.up;
                }
                else
                {
                    hopDir = Quaternion.Euler(0f, 0f, -45f) * Vector2.up;
                }

                rb2d.velocity = Vector2.zero;
                rb2d.AddForce(hopDir * jumpForce * 1.5f, ForceMode2D.Impulse);
            }
        }
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
        right = false;

        float curveCenters = (coll.bounds.extents.y - coll.bounds.extents.x);
        curveCenterBottom = coll.bounds.center - Vector3.up * curveCenters;
        curveCenterTop = coll.bounds.center + Vector3.up * curveCenters;

        foreach (ContactPoint2D c in contacts_)
        {
            Vector3 dir = curveCenterBottom - c.point;
            Vector3 dir2 = c.point - curveCenterTop;

            //Ground detect
            if (dir.y > 0f && Mathf.Abs(Vector2.Angle(c.normal, Vector3.up)) <= 40)
            {
                groundNormal = c.normal;

                grounded = true;
            }
            //Wall check
            else if (dir2.y < 0f)
            {
                if(c.point.x > transform.position.x)
                {
                    right = true;
                }

                wallHop = true;
            }
        }
    }
}
