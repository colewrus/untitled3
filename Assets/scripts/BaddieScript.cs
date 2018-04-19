using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType{backForth, random, bat, skull, blob};

public class BaddieScript : MonoBehaviour {


	public MoveType myType;
    public Vector3 startPos;
    public Vector3 destPos;
    Vector3 actualDest;
    public bool moveBack; //when true it sets object to move back towards it's start position
	public BoxCollider2D moveZone;

    public float timer;
    float startTime;
    public float speed;
    float distCovered;
    float fracJourney;
    float journeyLength;
    float tick;


    public GameObject bullet;
    public float BulletSpeed;
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
        Physics2D.IgnoreCollision(GameObject.Find("player").GetComponent<CapsuleCollider2D>(), GetComponent<BoxCollider2D>());       
		if (myType == MoveType.random) {
			destPos = new Vector3 (Random.Range (moveZone.bounds.min.x, moveZone.bounds.max.x), Random.Range (moveZone.bounds.min.y, moveZone.bounds.max.y), 0);
		}
    }
	
	// Update is called once per frame
	void Update () {
		//Right
		Debug.DrawRay (transform.position, new Vector3 (1,0, 0)*0.5f, Color.cyan);
		Ray eastRay = new Ray (transform.position, new Vector3 (1, 0, 0) * 0.5f);
	
		RaycastHit2D rayHit = Physics2D.Raycast (transform.position, new Vector3 (1, 0, 0) * 0.5f);

		if (rayHit.collider != null) {			
		}
		//Up
		Debug.DrawRay (transform.position, new Vector3 (0, 1, 0) * 1.5f, Color.red);
		//North-East
		Debug.DrawRay (transform.position, new Vector3 (0.5f, 0.5f, 0) * 2, Color.green);
		//SouthEast
		Debug.DrawRay (transform.position, new Vector3 (0.5f, -0.5f, 0) * 2, Color.green);
		//Down
		Debug.DrawRay (transform.position, new Vector3 (0, -1, 0) * 1.5f, Color.magenta);
		//south west
		Debug.DrawRay (transform.position, new Vector3 (-0.5f, -0.5f, 0) * 1.5f, Color.yellow);
		//North west
		Debug.DrawRay (transform.position, new Vector3 (-0.5f, 0.5f, 0) * 2, Color.yellow);
		//left
		Debug.DrawRay (transform.position, new Vector3 (-1, 0, 0) * 1.5f, Color.blue);


		if(tick < timer)
        {
            tick += Time.deltaTime;
        }else
        {
            StartCoroutine("SkullFire");
		
			if (myType == MoveType.random) {				
				destPos = new Vector3 (Random.Range (moveZone.bounds.min.x, moveZone.bounds.max.x), Random.Range (moveZone.bounds.min.y, moveZone.bounds.max.y), 0);
				actualDest = destPos;
			}
            journeyLength = Vector3.Distance(startPos, destPos);
            startTime = Time.time;

			//backforth movement
			if (myType == MoveType.backForth) {
					
				if (!moveBack) {
					actualDest = destPos;
					startTime = Time.time;
					moveBack = true;        
				} else {
					actualDest = startPos;
					moveBack = false;
				}
			}
            tick = 0;

        }
        distCovered = (Time.time - startTime) * speed;
        fracJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(transform.position, actualDest, speed*Time.deltaTime);

      }


    IEnumerator SkullFire()
    {
        yield return new WaitForSeconds(0.5f);

        GameObject bull = GM.instance.GetBullets();
        if(bull != null)
        {
            Vector3 target = PlayerScript.instance.transform.position - transform.position;


            // bull.GetComponent<Rigidbody2D>().velocity = target.normalized * speed;
            bull.transform.position = this.transform.position;
            bull.GetComponent<BulletScript>().target = target;
            bull.GetComponent<BulletScript>().speed = BulletSpeed;

            bull.SetActive(true);
        }
      

    }
}
