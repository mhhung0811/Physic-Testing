using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public float moveSpeed;
    private Vector2 moveDir;
    private Rigidbody2D myRb;

    [SerializeField] private LayerMask groundLayer;

    private bool isDashing = false;
    private bool canDash = true;
    private float dashPower = 40;    

    private int facing = 1;

    private int jumpCount = 0;

    private bool canSlide = true;
    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        moveDir = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.D))
        {
            myRb.velocity = new Vector2(0, myRb.velocity.y);
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (IsWall() && facing == 1)
            {
                myRb.velocity = new Vector2(0, myRb.velocity.y);
            }
            else
            {
                facing = 1;
                myRb.velocity = new Vector2(10, myRb.velocity.y);
            }
            //transform.position += new Vector3(7, 0, 0) * Time.deltaTime;

            //myRb.totalForce = new Vector2(50, myRb.totalForce.y);
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            if (IsWall() && facing == -1)
            {
                myRb.velocity = new Vector2(0, myRb.velocity.y);
            }
            else
            {
                facing = -1;
                myRb.velocity = new Vector2(-10, myRb.velocity.y);

            }
            //transform.position -= new Vector3(7, 0, 0) * Time.deltaTime;
            //myRb.totalForce = new Vector2(-50, myRb.totalForce.y);    
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            myRb.velocity = new Vector2(0, myRb.velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGround())
            {
                jumpCount = 1;
                myRb.velocity = new Vector2(myRb.velocity.x, 10);

            }
            else if (jumpCount< 2)
            {
                myRb.velocity = new Vector2(myRb.velocity.x, 10);
                //myRb.AddForce(new Vector2(0, 550));
                jumpCount++;
            }
            //canSlide = true;
        }

        if (Input.GetKeyDown(KeyCode.E) && canDash)
        {
            StartCoroutine(Dash());
        }

        if (IsWall() && canSlide)
        {
            StartCoroutine(WallSlide());
        }
    }
    private bool IsGround()
    {
        
        if (Physics2D.OverlapCircle(new Vector2(transform.position.x, this.transform.position.y - 0.5f), 0.01f, groundLayer))
            canSlide = true;
        return Physics2D.OverlapCircle(new Vector2(transform.position.x ,this.transform.position.y - 0.5f), 0.01f, groundLayer);
    }
    private bool IsWall()
    {
        if(Physics2D.OverlapCircle(new Vector2(transform.position.x + facing * 0.25f, transform.position.y), 0.01f, groundLayer))
        {
            jumpCount = 0;
        }
        return Physics2D.OverlapCircle(new Vector2(transform.position.x + facing * 0.25f, transform.position.y), 0.01f, groundLayer);
    }
    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = myRb.gravityScale;
        myRb.gravityScale = 0;
        myRb.velocity = new Vector2(facing*dashPower, 0);
        yield return new WaitForSeconds(0.1f);
        isDashing = false;
        myRb.gravityScale = originalGravity;
        myRb.velocity = new Vector2(0, myRb.velocity.y);
        yield return new WaitForSeconds(0.1f);
        canDash = true;
    }
    IEnumerator WallSlide()
    {
        canSlide = false;
        myRb.gravityScale = 0;
        myRb.velocity = new Vector2(0, -1);

        yield return new WaitForSeconds(0.3f);

        myRb.gravityScale = 2;
        myRb.velocity = Vector2.zero;
    }
}
