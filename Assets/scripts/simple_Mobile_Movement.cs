﻿using System.Collections;
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


    bool move; //Should you be moving?
    Vector2 dir; //direction for player to move

    [Tooltip("Control the sprite direction flip here?")]
    public bool flipSprite;

	// Use this for initialization
	void Start () {
        rb = this.GetComponent<Rigidbody2D>();
        move = false;
	}
	
	// Update is called once per frame
	void Update () {
		

        if(Input.touchCount > 0)
        {
            Touch myTouch = Input.touches[0];

            if (move)
            {
                Debug.Log("I should move " + dir);
                rb.AddForce(dir);
            }

            if(myTouch.phase == TouchPhase.Began) //--------------BEGIN
            {
                startTouch = myTouch.position;
            }else if(myTouch.phase == TouchPhase.Moved) //-----------------MOVED
            {
                if ((myTouch.position.x - startTouch.x) >= minXdist)
                {
                   
                    move = true;
                    dir = new Vector2(1 * moveSpeed, 0);
                    if (flipSprite)
                        this.GetComponent<SpriteRenderer>().flipX = false;
                  
                }               
                else if((myTouch.position.x - startTouch.x) <= -minXdist)
                {
                   
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
                    rb.AddForce(direction * jumpPower, ForceMode2D.Impulse);
                    Debug.Log("UP " + direction);

                }
                else if (y <= -minYdist)
                {
                    Vector2 worldRelease = Camera.main.ScreenToWorldPoint(touchEnd);
                    Vector2 direction = worldRelease - new Vector2(transform.position.x, transform.position.y);
                    rb.AddForce(direction * jumpPower, ForceMode2D.Impulse);
                    Debug.Log("Down " + direction);
                }

                
            }
        }

	}
}
