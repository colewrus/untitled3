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

	// Use this for initialization
	void Start () {
        rb = this.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		

        if(Input.touchCount > 0)
        {
            Touch myTouch = Input.touches[0];

            if(myTouch.phase == TouchPhase.Began) //--------------BEGIN
            {
                startTouch = myTouch.position;
            }else if(myTouch.phase == TouchPhase.Moved) //-----------------MOVED
            {
                if ((myTouch.position.x - startTouch.x) >= minXdist)
                {
                    Debug.Log("I should be moving right");
                    rb.AddForce(new Vector2(1 * moveSpeed, 0));
                }               
                else if((myTouch.position.x - startTouch.x) <= -minXdist)
                {
                    Debug.Log("I should move left");
                    rb.AddForce(new Vector2(-1*moveSpeed, 0));
                 
                }                    
            }
            else if(myTouch.phase == TouchPhase.Ended) //-----------------------ENDED
            {
                touchEnd = myTouch.position;

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
