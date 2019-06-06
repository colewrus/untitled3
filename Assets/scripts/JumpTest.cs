using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTest : MonoBehaviour
{

    Rigidbody2D rb;
    public float jPower,jTimer;
    public float jumpTick;
    public bool grounded, falling; //Are you touching the ground?
   

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpTick = 0;
    }


    private void FixedUpdate()
    {
        
        if(!falling)
            Jump();




    }


    void Jump()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded)
            {
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
            grounded = true;
            falling = false;
            jumpTick = 0;
        }
    }

    

}
