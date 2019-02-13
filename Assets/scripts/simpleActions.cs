using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleActions : MonoBehaviour {


    public float attackTimer;
    float tapTimer;
    public GameObject rock;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if(Input.touchCount > 0)
        {

            Touch touch = Input.GetTouch(0);
            tapTimer += 1 * Time.deltaTime;

            if(touch.phase == TouchPhase.Ended)
            {
                if(tapTimer < attackTimer)
                {
                    Debug.Log("attack");
                }
            }
        }
		
	}
}
