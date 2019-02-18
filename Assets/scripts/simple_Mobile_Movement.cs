using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simple_Mobile_Movement : MonoBehaviour {


    Vector2 startTouch;
    Vector2 touchEnd;
    public float minXdist; //minimum distance to swipe before movement kicks in
    public float minYdist; //minimum distance to swipe for a jump

    
    Rigidbody2D rb;
    public float jumpPower;
    public float moveSpeed;
    public float jumpMin, jumpMax;

    public bool onPlatorm;
    

    bool move; //Should you be moving?
    Vector2 dir; //direction for player to move

    [Tooltip("Control the sprite direction flip here?")]
    public bool flipSprite;

	// Use this for initialization
	void Start () {
        rb = this.GetComponent<Rigidbody2D>();
        move = false;
        onPlatorm = false;
	}
	
	// Update is called once per frame
	void Update () {
		

        if(Input.touchCount > 0)
        {
            Touch myTouch = Input.touches[0];

            if (move)
            {
                rb.velocity = new Vector2(dir.x, rb.velocity.y);
            }

        

            if(myTouch.phase == TouchPhase.Began) //--------------BEGIN
            {
                startTouch = myTouch.position;                

            }
            else if(myTouch.phase == TouchPhase.Moved) //-----------------MOVED
            {
                if ((myTouch.position.x - startTouch.x) >= minXdist)
                {
                    startTouch = myTouch.position;
                    move = true;
                    dir = new Vector2(1 * moveSpeed, 0);
                    
                    if (flipSprite)
                        this.GetComponent<SpriteRenderer>().flipX = false;
                  
                }               
                else if((myTouch.position.x - startTouch.x) <= -minXdist)
                {
                    startTouch = myTouch.position;
                    move = true;
                    dir = new Vector2(-1 * moveSpeed, 0);
                
                    if (flipSprite)
                        this.GetComponent<SpriteRenderer>().flipX = true;

                }                    
            }
            else if(myTouch.phase == TouchPhase.Ended) //-----------------------ENDED
            {
                touchEnd = myTouch.position;
                move = false;
                float x = touchEnd.x - startTouch.x;
                float y = touchEnd.y - startTouch.y;

                if (y >= minYdist)
                {
                    //Debug.Log("Jump");
                    Vector2 worldRelease = Camera.main.ScreenToWorldPoint(touchEnd);
                    Vector2 direction = worldRelease - new Vector2(transform.position.x, transform.position.y);
                    direction = new Vector2(Mathf.Clamp(direction.x, -1, 1), Mathf.Clamp(direction.y * jumpPower, jumpMin, jumpMax));
                    rb.AddForce(direction, ForceMode2D.Impulse);
                }
                else if (y <= -minYdist)
                {
                    Vector2 worldRelease = Camera.main.ScreenToWorldPoint(touchEnd);
                    Vector2 direction = worldRelease - new Vector2(transform.position.x, transform.position.y);
                    Debug.Log("first step in drop down");
                    if (this.GetComponent<Collider2D>().IsTouchingLayers(LayerMask.NameToLayer("passThrough"))) ;
                    {
                        Debug.Log("could pass through");
                        
                    }
               
                }

                
            }
        }

	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "platform")
        {
            onPlatorm = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "platform")
        {
            onPlatorm = false;
        }
    }
}
