using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTest : MonoBehaviour
{

    Rigidbody2D rb;
    public float jPower,jTimer, moveSpeed, runSpeed;
    public float jumpTick;
    public bool grounded, falling; //Are you touching the ground?
    float horiz;
    Animator ani;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpTick = 0;
        ani = GetComponent<Animator>();
    }


    private void FixedUpdate()
    {
        
        if(!falling)
            Jump();

        //_pMove();
        Move();


    }

    void Move()
    {
        horiz = Input.GetAxis("Horizontal");
        if (horiz < 0)
        {
            transform.Translate(new Vector3(-moveSpeed, 0, 0)*Time.deltaTime);
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (horiz > 0)
        {
            transform.Translate(new Vector3(moveSpeed, 0, 0)*Time.deltaTime);
            GetComponent<SpriteRenderer>().flipX = false;
        }

    }

    //Physics movement
    void _pMove()
    {
        horiz = Input.GetAxis("Horizontal");

        //animation controller
        if (horiz != 0)
        {           
            if (grounded)
                ani.SetTrigger("run"); //Animation
        }
        else
        {
            if (grounded)
                ani.SetTrigger("toIdle"); //animation
        }

        //Control spriteDirection
        if (horiz < 0)
        {
            rb.AddForce(Vector2.right * -moveSpeed);
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (horiz > 0)
        {
            rb.AddForce(Vector2.right * moveSpeed);
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }


    void Jump()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded)
            {
                ani.SetTrigger("jumpInit"); //animation
                rb.velocity = new Vector2(0, 0);
                rb.AddRelativeForce(Vector2.up * jPower, ForceMode2D.Impulse);
                grounded = false;
            }

            
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (!falling)
            {
                if (jumpTick < jTimer)
                {
                    rb.gravityScale = 0;
                    jumpTick += 1 * Time.deltaTime;
                }
                else
                {
                    rb.gravityScale = 1;
                    jumpTick = 0;
                    falling = true;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
          
            rb.gravityScale = 1;
            jumpTick = 0;
        }

 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "floor")
        {
            ani.SetTrigger("toIdle"); //animation
            grounded = true;
            falling = false;
            jumpTick = 0;
        }
    }

    

}
