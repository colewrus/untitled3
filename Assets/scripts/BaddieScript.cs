﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType{backForth, random};

public enum BaddieType { skull, bat, blob, skeleton};



public class BaddieScript : MonoBehaviour {


	public MoveType myType;
    public BaddieType badType;
    public Vector3 startPos;
    public Vector3 destPos;
    Vector3 actualDest;
    public bool moveBack; //when true it sets object to move back towards it's start position
	public BoxCollider2D moveZone;
    public GameObject ParentObject;
    


    public float timer;
    float startTime;
    public float speed;
    float distCovered;
    float fracJourney;
    float journeyLength;
    public float tick;

    public float health; 

    public GameObject bullet;
    public float BulletSpeed;
    public bool playerSeen;

    public Color redColor;
    public Color baseColor;

    public bool firstSeen; //is this the first time the bad guy has seen the player?
    public bool shotLock;


    
    



    //Bat Behaviour Variables below
    public bool flutterBool;
    public int shotTick;
    Vector3 attackDest;
    int flutterCount;
    public int maxFlutter;
    public float flutterTimer; //how long in between flutters?
    public float batAttackSpeed;
    float baseSpeed; //stored to return to base value after attack;

    private void Awake()
    {
       
      
        moveBack = false;
      
        startTime = Time.time;
    }

    // Use this for initialization
    void Start () {
        Debug.Log(1 << LayerMask.NameToLayer("player"));
        baseSpeed = speed;
        shotLock = true;
        startPos = this.transform.position;
        moveBack = false;
        actualDest = destPos;
        startTime = Time.time;
        Physics2D.IgnoreCollision(GameObject.Find("player").GetComponent<CapsuleCollider2D>(), GetComponent<BoxCollider2D>());
        
        if(badType == BaddieType.skull)
        {
            destPos = new Vector3(Random.Range(moveZone.bounds.min.x, moveZone.bounds.max.x), Random.Range(moveZone.bounds.min.y, moveZone.bounds.max.y), 0);
        }

        if(badType == BaddieType.bat)
        {    
            destPos = new Vector3(Random.Range(moveZone.bounds.min.x, moveZone.bounds.max.x), Random.Range(moveZone.bounds.min.y, moveZone.bounds.max.y), 0);
        }
        
        actualDest = destPos;
        playerSeen = false;
        StartCoroutine("Awake_FireLock");

        shotTick = 0;
        flutterCount = 0;
        flutterBool = false;
    }
	
	// Update is called once per frame
	void Update () {
		
        if(badType == BaddieType.skull)
        {
            SkullBehavior();
        }
        if(badType == BaddieType.blob){
            BlobBehavior();
        }
        if(badType == BaddieType.bat)
        {
            BatBehavior();
        }
    }


    void SkullBehavior()
    {
		if(tick < timer)
        {
            tick += Time.deltaTime;
        }else
        {
            if (playerSeen && badType == BaddieType.skull)
            {
                StartCoroutine("FlashRed");
                StartCoroutine("SkullFire");
            }
            	
			destPos = new Vector3 (Random.Range (moveZone.bounds.min.x, moveZone.bounds.max.x), Random.Range (moveZone.bounds.min.y, moveZone.bounds.max.y), 0);
			actualDest = destPos;
			
            journeyLength = Vector3.Distance(startPos, destPos);
            startTime = Time.time;	            
            tick = 0;
        }
        
        distCovered = (Time.time - startTime) * speed;
        fracJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(transform.position, actualDest, speed*Time.deltaTime);
       
    }

    void BatBehavior()
    {

        if (!flutterBool)
        {
            if(tick < timer)
            {
                tick += Time.deltaTime;
                shotTick = 0;
            }else
            {
                EnemyScan();
                if (shotTick == 20) //shot tick is for the raycast to check around enemy
                {
                    tick = 0;
                }
            }
        }else
        {
            if(flutterCount < maxFlutter)
            {
                if(tick < flutterTimer)
                {
                    tick += 1 * Time.deltaTime;                   
                }else
                {                 
                    //need to check for walls to make sure it somewhat makes sense
                    int someValue = Random.Range(0, 2) * 2 - 1;

                    destPos = transform.position + new Vector3(0.95f * someValue, 0, 0);
                    tick = 0;
                    flutterCount++;
                }
            }else
            {
                flutterBool = false;
                speed = baseSpeed;
                flutterCount = 0;
            }
        }
        

        if(transform.position != destPos)
        {
            transform.position = Vector3.Lerp(transform.position, destPos, speed * Time.deltaTime);
        }       

    }

    void BlobBehavior(){

        if(playerSeen){
            actualDest = GameObject.FindWithTag("Player").transform.position;

            if(tick<timer){ 
                tick += Time.deltaTime;
            }else{
                
                Vector2 tempV2 = new Vector2((actualDest.x - transform.position.x), 1.2f);
                tempV2 = tempV2.normalized* speed;
                gameObject.GetComponent<Rigidbody2D>().AddForce(tempV2, ForceMode2D.Impulse);
                
                tick = 0;
            }



            /*
             * distCovered = (Time.time - startTime) * speed;
            fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(transform.position, actualDest, speed * Time.deltaTime);
            */
        }
    }

    /*
    private void OnDrawGizmos()
    {

        Color tempColor = Color.green;
        tempColor.a = 0.35f;
        Gizmos.color = tempColor;
        Gizmos.DrawSphere(transform.position, 2.75f);
    }
    */

    public void EnemyScan()
    {        
        Collider2D[] hitCollider;
        hitCollider = Physics2D.OverlapCircleAll(transform.position, 2.25f, 1 << LayerMask.NameToLayer("player"));
        for (var i = 0; i < hitCollider.Length; i++)
        {             
            playerSeen = true;
            flutterBool = true;
            destPos = hitCollider[i].transform.position;
            speed = batAttackSpeed;
            tick = 0;
        }

        if(hitCollider.Length == 0)
        {
            destPos = new Vector3(Random.Range(moveZone.bounds.min.x, moveZone.bounds.max.x), Random.Range(moveZone.bounds.min.y, moveZone.bounds.max.y), 0);
            tick = 0;
        }
        Debug.Log("hits " + hitCollider.Length);
        

        /*
        if (shotTick < 20)
        {
            var theta = -2 * Mathf.PI * shotTick / 20;
            Vector3 tempV = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0) * 3;

            Debug.DrawLine(transform.position, transform.position + tempV, Color.yellow, Vector3.Distance(transform.position, transform.position + tempV));

            shotTick++;
            RaycastHit2D tempHit = Physics2D.Raycast(transform.position + new Vector3(0.5f, -0.5f, 0), transform.position + 
                tempV, Vector3.Distance(transform.position, transform.position + tempV),1<<LayerMask.NameToLayer("player"));
            if(tempHit.collider != null)
            {
                              
                playerSeen = true;
                // store destination
                flutterBool = true;              
                destPos = tempHit.point;
            }else
            {
                //sets new destination for player
                destPos = new Vector3(Random.Range(moveZone.bounds.min.x, moveZone.bounds.max.x), Random.Range(moveZone.bounds.min.y, moveZone.bounds.max.y), 0);
            }
        }else
        {
            
        }
        */


    }


    IEnumerator Awake_FireLock()
    {
        yield return new WaitForSeconds(1.1f);
        shotLock = false;
    }

    IEnumerator FlashRed()
    {
        GetComponent<SpriteRenderer>().color = redColor;
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().color = baseColor;
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().color = redColor;
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().color = baseColor;

    }

    public void pub_Fire()
    {
        if(badType == BaddieType.skull){
            StartCoroutine("SkullFire");
        }

    }

    IEnumerator SkullFire()
    {
       
        destPos = this.transform.position;
        StartCoroutine("FlashRed");
        yield return new WaitForSeconds(0.65f);
        if (!shotLock)
        {
            GameObject bull = GM.instance.GetBullets();

            if (bull != null)
            {
                Vector3 target = PlayerScript.instance.transform.position - transform.position;

                bull.transform.position = this.transform.position;
                bull.GetComponent<BulletScript>().target = target;
                bull.GetComponent<BulletScript>().speed = BulletSpeed;
                bull.SetActive(true);
                bull.GetComponent<Rigidbody2D>().velocity = target.normalized * BulletSpeed;
            }
        }

    }

    public void run_FirstSeen()
    {
        StartCoroutine("First_Seen_Decay");
    }

    IEnumerator First_Seen_Decay()
    {
      
        yield return new WaitForSeconds(5.0f);      
        firstSeen = false;
    }
}
