using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeTest_P : MonoBehaviour {

    float horiz;
    float vert;
    public bool moveLock;
    Rigidbody2D rb;
    public float speed;
    public Vector2 offset; //offset amount for child collider
    public bool attackLock;
    public float attackTimer;
    float attackTick;
    Vector2 destV;
    GameObject hitBox;

	// Use this for initialization
	void Start () {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        attackTick = 0;
        hitBox = gameObject.transform.GetChild(1).gameObject;
        if (this.GetComponent<SpriteRenderer>().flipX)
        {
            hitBox.transform.position = new Vector2(transform.position.x + offset.x, transform.position.y + offset.y);
        }
        else
        {
            hitBox.transform.position = new Vector2(transform.position.x + -offset.x, transform.position.y + offset.y);
        }
    }
	
	// Update is called once per frame
	void Update () {
        Move();
        Attack();
	}



    void Move()
    {
        //vert = Input.GetAxis("Vertical");
        horiz = Input.GetAxis("Horizontal");

        if (!moveLock)
        {
            rb.velocity = new Vector2(horiz*speed, Mathf.Clamp(rb.velocity.y, -10, 10));
            if(horiz > 0)
            {
                if (!attackLock)
                {
                   hitBox.transform.position = new Vector2(transform.position.x + offset.x, transform.position.y + offset.y);
                    //gameObject.GetComponent<SpriteRenderer>().flipX = true;
                }
            }

            if(horiz < 0)
            {
                if (!attackLock)
                {
                    hitBox.transform.position = new Vector2(transform.position.x + -offset.x, transform.position.y + offset.y);
                    //gameObject.GetComponent<SpriteRenderer>().flipX = false;
                }
            }
        }

    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
        
            //start swing
            if (!attackLock)
            {          
                destV = new Vector2(hitBox.transform.position.x, hitBox.transform.position.y - transform.GetChild(1).GetComponent<Melee_Hitbox>().swingDeltaY);
                //gameObject.GetComponent<Animator>().Play("swingAnim");
                hitBox.GetComponent<Melee_Hitbox>().armed = true;
                attackLock = true;
            }
        }
       
        if (attackLock)
        {
            if(attackTick < attackTimer)
            {
                attackTick += 1 * Time.deltaTime;
                /*fuck the swing - just arm the hitbox. Maybe set hitbox position based on mouseclick position X
                if(attackTick > (attackTimer / 5))
                {
                    destV.x = hitBox.transform.position.x;
                    hitBox.transform.position = Vector2.Lerp(hitBox.transform.position, destV, (attackTick));
                }
                */
            }
            else
            {   
                //reset the hitbox
                if (this.GetComponent<SpriteRenderer>().flipX)
                {
                    hitBox.transform.position = new Vector2(transform.position.x + -offset.x, transform.position.y + offset.y);
                }else
                {
                    hitBox.transform.position = new Vector2(transform.position.x + offset.x, transform.position.y + offset.y);
                }
                Debug.Log("melee test");
                hitBox.GetComponent<Melee_Hitbox>().armed = false; //unarm the weapon otherwise enemies will take demanage when you walk past
                attackLock = false;
                attackTick = 0;
            }
        }
    }
}
