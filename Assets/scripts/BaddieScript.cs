using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaddieScript : MonoBehaviour {

    public Vector3 startPos;
    public Vector3 destPos;
    Vector3 actualDest;
    public bool moveBack; //when true it sets object to move back towards it's start position

    public float timer;
    float startTime;
    public float speed;
    float distCovered;
    float fracJourney;
    float journeyLength;
    float tick;

    private void Awake()
    {
        startPos = this.transform.position;
        moveBack = false;
        actualDest = destPos;
        startTime = Time.time;
    }
    // Use this for initialization
    void Start () {
        startPos = this.transform.position;
        moveBack = false;
        actualDest = destPos;
        startTime = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
		if(tick < timer)
        {
            tick += Time.deltaTime;
        }else
        {
            journeyLength = Vector3.Distance(startPos, destPos);
            startTime = Time.time;
            if (!moveBack)
            {
                actualDest = destPos;
                startTime = Time.time;
                moveBack = true;        
            }
            else
            {
                actualDest = startPos;
                moveBack = false;
            }
            tick = 0;
        }
        distCovered = (Time.time - startTime) * speed;
        fracJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(transform.position, actualDest, speed*Time.deltaTime);

      }
}
