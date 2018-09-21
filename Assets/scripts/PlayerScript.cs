using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class PlayerScript : MonoBehaviour {

    public static PlayerScript instance = null;
    public float speed;
    public float jumpPower;

   
    Collider2D aimCollider;
    public GameObject bulletZone;
    public List<Collider2D> enemyCollider = new List<Collider2D>();
    bool aimOverlap;

    public bool onPlatform, onLadder;

    RaycastHit hit;
    RaycastHit2D hit2d;
    float horiz;
    float vert;
    Rigidbody2D rb;

    public float gunRange;
    public ParticleSystem part;

    public LayerMask viableLayers;
    public List<AudioClip> EffectsClips = new List<AudioClip>();
    public List<AudioClip> WalkingClips = new List<AudioClip>();
    AudioSource myAudio;

    public List<GameObject> bullets = new List<GameObject>();
    public float bulletCount;
    public float shotDelay;
    public float ReloadTimer;
    bool reloading; //are you actively reloading
    public float damage;

    public bool fireLock; 

    GameObject currentPlatform;



    public Color redColor;
    public Color baseColor;

    public AudioSource walkingSource;

    ContactFilter2D contactFilter;
    Collider2D[] colliders;
    Collider2D myCollider;

    public float jumpCount;

    public List<GameObject> keys = new List<GameObject>();
    public bool lookAtBoss;
    public GameObject tempBoss; //temporarily hold the boss as a gameobject so camera can pan to it
    bool returnFromBoss; 

    public bool moveLock; //lock the player movement;

    public float fadeModifier; //how fast do we want the screen to fade
    public Image fadeImg;
    float fadeOpacity;
    float fadeTime;
    Color fadeColor;
    public bool fadeIn;
    bool fadeOut;
    float doorFadeOut; //how long does the fade take for each door?

    //control the walking sounds
    public float walkTimer;
    float walkTick;

    //combat control
    public float attackSpeed;
    float attackTick;
    bool attackGate;
    public GameObject sword;
    [Tooltip("Use this to make sure player doesn't take damage every frame")]
    public bool takeDamage;

    public GameObject door;


    public List<AudioClip> swingClips = new List<AudioClip>();
    public AudioSource mySwingSource;


    public float floorCheckTimer;
    public bool floored;

    //player animation stuph
    GameObject swingObj;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        jumpCount = 0;
        rb = this.GetComponent<Rigidbody2D>();

        bulletCount = 6;       
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("enemies"))
        {
            if (obj)
            {
                enemyCollider.Add(obj.GetComponent<Collider2D>());
            }                
            else
            {
                break;
            }
        }
        walkingSource.clip = WalkingClips[0];
        myAudio = GetComponent<AudioSource>();
        reloading = false;
        lookAtBoss = false;
        moveLock = false;
        swingObj = gameObject.transform.Find("swing").gameObject;
        attackGate = false;
        //Set the offset from the player for the sword
        sword.SetActive(false);
	}



	// Update is called once per frame
	void Update () {
        RaycastHit2D hit_S = Physics2D.Raycast(transform.position, new Vector3(0, -0.75f, 0));

        if (Input.GetKey(KeyCode.X))
        {
            fadeIn = true;
            fadeTime = 0;
        }

        FadeControl();

        if(hit_S.collider.name == "floors")
        {           
            GetComponent<CapsuleCollider2D>().enabled = true;
        }

        if (lookAtBoss)
        {
            float step = 0.75f * Time.deltaTime;
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, tempBoss.transform.position + new Vector3(0,0,-2), step);
        }

        if (returnFromBoss)
        {
            float step = 1.25f * Time.deltaTime;
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, transform.position + new Vector3(0, 0, -3), step);
        }

        PlayerMove();


        if (Input.GetMouseButtonDown(0))
        {
            if (!attackGate) //need to change to a attack gate
            {
                StartCoroutine("Attack");
            }
            
        }
	}

    IEnumerator Attack()
    {
        attackGate = true;
        swingObj.GetComponent<Animator>().SetTrigger("swing");
        mySwingSource.PlayOneShot(swingClips[0]);
        sword.SetActive(true);
        yield return new WaitForSeconds(attackSpeed);
        sword.SetActive(false);
        attackGate = false;    
    }

    public List<RaycastResult> RaycastMouse()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
      
        return results;
    }

    void FadeControl()
    {
        if (fadeIn)
        {
            if (fadeTime > fadeModifier) //this value determines how long the fade will sit before transitioning away
            {                
                fadeTime = 0;
                fadeIn = false;
                return;
            }
            fadeTime += 1 * Time.deltaTime;
            fadeOpacity = Mathf.Lerp(0, 1, fadeTime);
            fadeColor = fadeImg.color;
            fadeColor.a = fadeOpacity;
            fadeImg.color = fadeColor;
        }

        if (fadeOut)
        {          
            if (fadeTime > fadeModifier)
            {
                fadeTime = 0;
                fadeOut = false;
                return;
            }
            fadeTime += fadeModifier * Time.deltaTime;
            fadeOpacity = Mathf.Lerp(1, 0, fadeTime);
            fadeColor = fadeImg.color;
            fadeColor.a = fadeOpacity;
            fadeImg.color = fadeColor;
        }
    }



    IEnumerator c_Reload()
    {
        while (bulletCount < 6)
        {
            reloading = true;
            yield return new WaitForSeconds(ReloadTimer);
            bulletCount++;
            myAudio.PlayOneShot(EffectsClips[2], 0.45f);
            bullets[Mathf.FloorToInt(bulletCount) - 1].SetActive(true);
            if (bulletCount == 6)
            {
                myAudio.PlayOneShot(EffectsClips[3]);
                yield return new WaitForSeconds(EffectsClips[3].length);
                myAudio.PlayOneShot(EffectsClips[4], 1);
            }
        }    
    }


    void PlayerMove()
    {
        horiz = Input.GetAxis("Horizontal");
        if (!moveLock)
        {
            rb.velocity = new Vector3(horiz * speed, Mathf.Clamp(rb.velocity.y, -18, 10));
        }else
        {
            
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
 
        }
        

        if(horiz < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if(horiz > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if(rb.velocity.y == 0 && rb.velocity.x != 0 && !walkingSource.isPlaying)
        {
            if(walkTick < walkTimer)
            {
                walkTick += 1 * Time.deltaTime;
            }else
            {
                walkingSource.volume = Random.Range(0.25f, 0.35f);
                walkingSource.pitch = Random.Range(0.6f, 1.1f);
                walkingSource.Play();
                walkTick = 0;
            }
            
  
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
           
            if (!onLadder)
            {
                /*
                if(jumpCount == 0){
                    GetComponent<Animator>().SetTrigger("jump");
                    jumpCount++;
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(Vector2.up * (jumpPower), ForceMode2D.Impulse);
                }
                */
                if(jumpCount == 0){
                  
                        floored = false;
                        GetComponent<Animator>().SetBool("floored", false);
                        GetComponent<Animator>().SetTrigger("jump");
                    
                   
                }
                if(jumpCount < 2)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(Vector2.up * (jumpPower *1.75f), ForceMode2D.Impulse);
                    jumpCount++;
                }
           
            }
        }

       

        if (Input.GetKey(KeyCode.S))
        {
            if (onPlatform)
            {
                this.GetComponent<CapsuleCollider2D>().enabled = !GetComponent<CapsuleCollider2D>().enabled;  
                Invoke("DownThrough", 0.31f);
                GetComponent<Animator>().SetTrigger("down");
            }
        }


    }



    void DownThrough()
    {
        GetComponent<CapsuleCollider2D>().enabled = !GetComponent<CapsuleCollider2D>().enabled;        
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "platform") //check to see if we're on platform to enable down jump-through
        {

            Debug.Log(rb.velocity.y);
            if(rb.velocity.y <= 0.1f)
            {
                onPlatform = true;
            }
            StartCoroutine("FloorCheck");
        }

        if(collision.collider.name == "floors")
        {

            floored = true;
            StartCoroutine("FloorCheck");
        }

        if(collision.transform.tag == "key")
        {
            keys.Add(collision.gameObject);
            collision.gameObject.SetActive(false);
        }


    }

    IEnumerator FloorCheck(){
        yield return new WaitForSeconds(floorCheckTimer);
        if(onPlatform || floored){
            jumpCount = 0;       
            GetComponent<Animator>().SetBool("floored", true);

        }
      
    }

  

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.transform.tag == "platform")
        {
            onPlatform = false;
            GetComponent<Animator>().SetBool("floored", false);
        }

        if(collision.transform.tag == "floor"){
            floored = false;
    
        }
    }

    public IEnumerator _DoorTransfer(GameObject DoorObj)
    {
        fadeIn = true;
        //play the door sound
       
        yield return new WaitForSeconds(fadeModifier);
        myAudio.PlayOneShot(EffectsClips[5], 1.0f);
        //play close sound
        transform.position = DoorObj.GetComponent<DoorScript>().dest.position;
        
        yield return new WaitForSeconds(fadeModifier/2);
        fadeOut = true;
       
//        bulletZone.transform.position = DoorObj.GetComponent<DoorScript>().bulletZoneReset.position;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.transform.tag == "enemyWeapon")
        {

            if(!takeDamage){
                Debug.Log("trigger stay");
                GetComponent<Animator>().SetTrigger("damage");
                takeDamage = true;
            }
          
        }
        /*

        if (collision.transform.parent != null)
        {
            if (collision.transform.parent.tag == "enemies")
            {
                if (collision.transform.parent.GetComponent<BaddieScript>())
                {

                    collision.transform.parent.GetComponent<BaddieScript>().playerSeen = true; //bad guy sees the player
                    if (!collision.transform.parent.GetComponent<BaddieScript>().firstSeen)
                    {
                        collision.transform.parent.GetComponent<BaddieScript>().pub_Fire();
                        collision.transform.parent.GetComponent<BaddieScript>().run_FirstSeen();
                        collision.transform.parent.GetComponent<BaddieScript>().firstSeen = true;
                    }

                
                }

                if (collision.transform.name == "triggerDetection") //hit box on enemies for on-touch damage
                {
                    // StartCoroutine("DamageFlash");
                }
            }
        }
        */

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.transform.tag == "door")
        {
            if (collision.gameObject.GetComponent<DoorScript>().activeDoor)
            {
                fadeIn = true;
                IEnumerator tempCo = _DoorTransfer(collision.gameObject);
                StartCoroutine(tempCo);
            }

            if(collision.gameObject.GetComponent<DoorScript>().key != null)
            {
                if(collision.gameObject.GetComponent<DoorScript>().key == keys[0])
                {
                    if (!collision.gameObject.GetComponent<DoorScript>().BossFightLocked) //make sure you can't open the door once you init the boss fight
                        collision.gameObject.GetComponent<DoorScript>().OpenBossDoor();
                }               
            }
           
        }

        if(collision.transform.tag == "ladders")
        {
           
                onLadder = true;
                rb.velocity = Vector3.zero;           
        }        

        /*
        if(collision.transform.parent != null)
        {
            if (collision.transform.parent.tag == "enemies")
            {
                if (collision.transform.parent.GetComponent<BaddieScript>())
                {

                    collision.transform.parent.GetComponent<BaddieScript>().playerSeen = true; //bad guy sees the player
                    if (!collision.transform.parent.GetComponent<BaddieScript>().firstSeen)
                    {
                        collision.transform.parent.GetComponent<BaddieScript>().pub_Fire();
                        collision.transform.parent.GetComponent<BaddieScript>().run_FirstSeen();
                        collision.transform.parent.GetComponent<BaddieScript>().firstSeen = true;
                    }
                }

                
                if (collision.transform.name == "triggerDetection") //hit box on enemies for on-touch damage
                {
                    //StartCoroutine("DamageFlash");
                }
            }
        }
        */

        if(collision.transform.tag == "bat"){

            GameObject temp = collision.transform.parent.gameObject;

            if(temp.GetComponent<BatScript>().attack && !temp.GetComponent<BatScript>().aggro)
            {
                if (!takeDamage)
                {
                    this.GetComponent<Animator>().SetTrigger("damage");
                    takeDamage = true;
                }
            }

        }

        if(collision.transform.tag == "enemyWeapon"){
            GetComponent<Animator>().SetTrigger("damage");
            takeDamage = true;
            Debug.Log("enemy weapon should enter");
        }

        if(collision.transform.tag == "spawner")
        {
            if (collision.GetComponent<SpawnScript>().activeSpawn)
            {
                collision.GetComponent<SpawnScript>().Spawn();
            }            
        }

        if(collision.transform.tag == "bullet")
        {
            Vector3 dir = (gameObject.GetComponent<CapsuleCollider2D>().bounds.ClosestPoint(collision.transform.position) - transform.position);
            rb.AddForce(new Vector2(-8, 1), ForceMode2D.Impulse);
            
            horiz = 0.5f * dir.x;
            this.GetComponent<Animator>().SetTrigger("damage");
            takeDamage = true;
        }
    }


    public void DamageRest(){
        takeDamage = false;
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.transform.tag == "ladders")
        {
            onLadder = false;            
        }

        /*
        if(collision.transform.tag == "door")
        {
            if (collision.gameObject.GetComponent<DoorScript>().key != null) {

                
                if (collision.gameObject.GetComponent<DoorScript>().key == keys[0])
                {
                    collision.gameObject.GetComponent<DoorScript>().CloseBossDoor();
                    collision.gameObject.GetComponent<DoorScript>().BossFightLocked = true;
                    tempBoss = collision.gameObject.GetComponent<DoorScript>().boss;
                    collision.gameObject.GetComponent<DoorScript>().boss.GetComponent<Boss_Script>()._BatState = BatState.intro;
                    lookAtBoss = true;
                    moveLock = true;
                    StartCoroutine("ReturnFromBossIntro");
                   
                }
            }
        }
        */

        if (collision.transform.parent != null)
        {
            if (collision.transform.parent.tag == "enemies")
            {
                if (collision.transform.parent.GetComponent<BaddieScript>())
                {
                    collision.transform.parent.GetComponent<BaddieScript>().playerSeen = false;
                }
            }
                
        }
    }

    IEnumerator ReturnFromBossIntro()
    {
  
        yield return new WaitForSeconds(3);
        lookAtBoss = false;
        returnFromBoss = true;
        transform.position = door.GetComponent<DoorScript>().bossSetup.position;
        yield return new WaitForSeconds(2);

        moveLock = false;

        if(tempBoss.GetComponent<Boss_Script>().thisBoss == BossType.bat)
        {
            tempBoss.GetComponent<Boss_Script>()._BatState = BatState.search;
            door.GetComponent<DoorScript>().CloseBossDoor();
        }
        tempBoss.GetComponent<Boss_Script>().InitCanvas();

        Camera.main.GetComponent<CameraScript>().controlOverride = false;//return camera control to script

    }

}
