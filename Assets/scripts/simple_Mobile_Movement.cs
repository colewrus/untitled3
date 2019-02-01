using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simple_Mobile_Movement : MonoBehaviour {


    Vector2 startTouch;
    Vector2 touchEnd;
    public float minXdist; //minimum distance to swipe before movement kicks in
    public float minYdist; //minimum distance to swipe for a jump

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		

        if(Input.touchCount > 0)
        {
            Touch myTouch = Input.touches[0];

            if(myTouch.phase == TouchPhase.Began)
            {
                startTouch = myTouch.position;
            }else if(myTouch.phase == TouchPhase.Moved)
            {
                if ((myTouch.position.x - startTouch.x) >= minXdist)
                    Debug.Log("I should be moving right");
                else if((myTouch.position.x - startTouch.x) <= -minXdist)
                    Debug.Log("I should move left");
            }
            else if(myTouch.phase == TouchPhase.Ended){
                touchEnd = myTouch.position;
                //Debug.Log("Start Pos " + startTouch);
                //Debug.Log("End Pos " + touchEnd);
                float x = touchEnd.x - startTouch.x;
                float y = touchEnd.y - startTouch.y;

                if (y >= minYdist)
                {
                    //Debug.Log("Jump");
                    Vector2 worldRelease = Camera.main.ScreenToWorldPoint(touchEnd);
                    Debug.Log(worldRelease);

                }
                else if (y <= -minYdist)
                {
                   // Debug.Log("Down through");
                }

                
            }
        }

	}
}
