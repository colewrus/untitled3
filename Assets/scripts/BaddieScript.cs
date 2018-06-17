using System.Collections;
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
    float tick;

    public float health; 

    public GameObject bullet;
    public float BulletSpeed;
    public bool playerSeen;

    public Color redColor;
    public Color baseColor;

    public bool firstSeen; //is this the first time the bad guy has seen the player?
    public bool shotLock;


    //Bat Behaviour Variables below
    bool flutterBool;

    private void Awake()
    {
       
      
        moveBack = false;
      
        startTime = Time.time;
    }

    // Use this for initialization
    void Start () {
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
        if (tick < timer && !flutterBool)
        {
            tick += Time.deltaTime;
        }
        else
        {
            destPos = new Vector3(Random.Range(moveZone.bounds.min.x, moveZone.bounds.max.x), Random.Range(moveZone.bounds.min.y, moveZone.bounds.max.y), 0);
            tick = 0;
            
            //pick a point, fly to it
            //arrive
            //flutter timer starts - enable collision with player
                //flutter to the right
                //give time to arrive
                //flutter to the left
                //give time to arrive
        }
        if(transform.position != destPos)
        {
            transform.position = Vector3.Lerp(transform.position, destPos, speed * Time.deltaTime);
        }
        else
        {
            flutterBool = true;
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

    public void EnemyScan()
    {
        RaycastHit2D hit_N = Physics2D.Raycast(transform.position, new Vector3(0, 1, 0), 2.5f);
        RaycastHit2D hit_NE = Physics2D.Raycast(transform.position, new Vector3(0.5f, 0.5f, 0) * 2);
        RaycastHit2D hit_NW = Physics2D.Raycast(transform.position, new Vector3(-0.5f, 0.5f, 0) * 2);
        RaycastHit2D hit_E = Physics2D.Raycast(transform.position, new Vector3(1, 0, 0) * 2.5f);
        RaycastHit2D hit_S = Physics2D.Raycast(transform.position, new Vector3(0, -1, 0) * 2.5f);
        RaycastHit2D hit_SE = Physics2D.Raycast(transform.position, new Vector3(0.5f, -0.5f, 0) * 2);
        RaycastHit2D hit_SW = Physics2D.Raycast(transform.position, new Vector3(-0.5f, -0.5f, 0) * 2.5f);
        RaycastHit2D hit_W = Physics2D.Raycast(transform.position, new Vector3(-1, 0, 0) * 2.5f);

        Debug.DrawRay(transform.position, new Vector3(1, 0, 0) * 0.5f, Color.cyan);
        Debug.DrawRay(transform.position, new Vector3(0, -1, 0) * 2.5f, Color.red);
        if (hit_N.collider.tag == "Player" || hit_E.collider.tag == "Player" || hit_NE.collider.tag == "Player" || hit_SE.collider.tag == "Player" || hit_S.collider.tag == "Player" || hit_SW.collider.tag == "Player" || hit_W.collider.tag == "Player" || hit_NW.collider.tag == "Player")
        {
            playerSeen = true;
            Debug.Log(hit_S.collider.name);
        }

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
