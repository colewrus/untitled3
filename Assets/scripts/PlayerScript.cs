using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerScript : MonoBehaviour {

    public static PlayerScript instance = null;
    public float speed;
    public float jumpPower;

    public GameObject aimReticule;
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

    float jumpCount;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {

        jumpCount = 0;
        rb = this.GetComponent<Rigidbody2D>();
        aimCollider = aimReticule.GetComponent<Collider2D>();
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
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit2D hit_S = Physics2D.Raycast(transform.position, new Vector3(0, -0.75f, 0));
        Debug.DrawRay(transform.position, new Vector3(0, -0.75f, 0), Color.red);
        if(hit_S.collider.name == "floors")
        {           
            GetComponent<CapsuleCollider2D>().enabled = true;
        }


        PlayerMove();
        if (Input.GetMouseButtonDown(0))
        {

            if(!fireLock){
                reloading = false;
                if (bulletCount <= 0)
                {
                    return;
                }
                if (bulletCount > 1)
                {
                    Shoot();
                    StopCoroutine("c_Reload");
                }else
                {              
                    Shoot();
                    StartCoroutine("c_Reload");
                } 
            }
          
 
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if(!reloading)
                StartCoroutine("c_Reload");
        }

        if(bulletCount > 6)
        {
            bulletCount = 6;
        }
        LadderMovement();
        Reticule();
	}

    public List<RaycastResult> RaycastMouse()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);

        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        Debug.Log(results.Count);
        return results;

    }

    void Reticule()
    {
        aimReticule.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);


        for (int i=0; i < enemyCollider.Count; i++)
        {
            if (aimCollider.bounds.Intersects(enemyCollider[i].bounds))
            {
                Color tmp = new Color(255, 0, 0);
                aimReticule.GetComponent<SpriteRenderer>().color = tmp;                
            }else
            {
                Color tmp = new Color(255,255,255);
                aimReticule.GetComponent<SpriteRenderer>().color = tmp;
            }
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

    void Shoot()
    {

        Vector2 rayDest = Input.mousePosition;
       
        rayDest = Camera.main.ScreenToWorldPoint(rayDest);
              
        Vector2 destinationActual = rayDest - new Vector2(transform.position.x, transform.position.y);

        hit2d = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), destinationActual, gunRange, viableLayers);

        if(hit2d.collider != null)
        {
            
            part.transform.position = hit2d.point;
            part.Play();
            
            if(hit2d.collider.transform.tag == "enemies")
            {
                //check for their health;
                Debug.Log(hit2d.collider.gameObject.GetComponent<BaddieScript>().health);
                if(hit2d.collider.gameObject.GetComponent<BaddieScript>().health - damage <= 0){
                    enemyCollider.Remove(hit2d.collider);
                    Destroy(hit2d.collider.gameObject);
                    if(hit2d.collider.gameObject.GetComponent<BaddieScript>().ParentObject!= null)
                    {
                        SpawnScript tempScript = hit2d.collider.gameObject.GetComponent<BaddieScript>().ParentObject.GetComponent<SpawnScript>();
                        //hit2d.collider.gameObject.GetComponent<BaddieScript>().ParentObject.GetComponent<SpawnScript>().p_Waves
                        Debug.Log(tempScript.p_Waves.Count);
                       
                        tempScript.p_Waves[tempScript.waveCounter-1].EnemyKilled();
                        Debug.Log(tempScript.p_Waves[tempScript.waveCounter - 1].waveCount);
                    }
                    GM.instance.RemoveEnemy();
                }else{
                    hit2d.collider.gameObject.GetComponent<BaddieScript>().health -= damage;
                }
            }
        }

        Ray myRay = new Ray(transform.position, destinationActual*gunRange);
        Physics2D.Raycast(transform.position, destinationActual, gunRange);  

        Debug.DrawRay(transform.position, destinationActual*gunRange, Color.cyan);
        Debug.DrawLine(transform.position, destinationActual * gunRange, Color.white, 10);
       
        myAudio.PlayOneShot(EffectsClips[0], 0.8f);
        bulletCount--;
        
        bullets[Mathf.FloorToInt(bulletCount)].SetActive(false);
    }

    void PlayerMove()
    {
        horiz = Input.GetAxis("Horizontal");   

        rb.velocity = new Vector3(horiz * speed, Mathf.Clamp(rb.velocity.y, -10, 10));
        if(rb.velocity.y == 0 && rb.velocity.x != 0 && !walkingSource.isPlaying)
        {
            walkingSource.volume = Random.Range(0.25f, 0.55f);
            walkingSource.pitch = Random.Range(0.6f, 1.1f);
            walkingSource.Play();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!onLadder)
            {
                
                if(jumpCount < 2)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(Vector2.up * (jumpPower + jumpCount), ForceMode2D.Impulse);
                    jumpCount++;
                }
                
            }
                
        }

        if(Input.GetKeyDown(KeyCode.Alpha0)){
            Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        }

        if (Input.GetKey(KeyCode.S))
        {
            if (onPlatform)
            {

                this.GetComponent<CapsuleCollider2D>().enabled = !GetComponent<CapsuleCollider2D>().enabled;
                //GetComponent<CapsuleCollider2D>().enabled = !GetComponent<CapsuleCollider2D>().enabled;
                //currentPlatform.GetComponent<Collider2D>().isTrigger = true;
                Invoke("DownThrough", 0.41f);                
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
            onPlatform = true;
            jumpCount = 0;
        }

        if(collision.collider.name == "floors")
        {
            jumpCount = 0;
        }
    }

    void LadderMovement()
    {
        if (onLadder)
        {
            rb.gravityScale = 0;
          
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                rb.velocity = transform.up * 2.75f;
            }
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                rb.velocity = Vector3.zero;
            }
            if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                rb.velocity = transform.up * -2.75f;
            }
            if(Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
            {
                rb.velocity = Vector3.zero;
            }
        }else
        {
            rb.gravityScale = 1;
        }
    }

  

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.transform.tag == "platform")
        {
            
            onPlatform = false;
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.transform.tag == "door")
        {
            if (collision.gameObject.GetComponent<DoorScript>().activeDoor)
            {
                transform.position = collision.gameObject.GetComponent<DoorScript>().dest.position;
                bulletZone.transform.position = collision.gameObject.GetComponent<DoorScript>().bulletZoneReset.position;
            }
           
        }

        if(collision.transform.tag == "ladders")
        {
           
                onLadder = true;
                rb.velocity = Vector3.zero;           
        }        

        if(collision.transform.parent != null)
        {
            if (collision.transform.parent.tag == "enemies")
            {
                collision.transform.parent.GetComponent<BaddieScript>().playerSeen = true; //bad guy sees the player
                if (!collision.transform.parent.GetComponent<BaddieScript>().firstSeen)
                {
                    collision.transform.parent.GetComponent<BaddieScript>().pub_Fire();
                    collision.transform.parent.GetComponent<BaddieScript>().run_FirstSeen();
                    collision.transform.parent.GetComponent<BaddieScript>().firstSeen = true; 
                }
                
                if (collision.transform.name == "triggerDetection") //hit box on enemies for on-touch damage
                {
                    StartCoroutine("DamageFlash");
                }
            }
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
            Debug.Log(new Vector2(dir.x, dir.y).normalized*15);
            horiz = 0.5f * dir.x;
            //Vector3 dir = (collision.transform.position - transform.position);
            //Debug.Log(dir.normalized);

            //Debug.Break();
            /*
            RaycastHit2D hit;
            hit = Physics2D.Raycast(transform.position, transform.forward, gameObject.GetComponent<CapsuleCollider2D>().size.x);
            Debug.DrawLine(transform.position, transform.forward * gameObject.GetComponent<CapsuleCollider2D>().size.x, Color.green);
            Debug.Log(gameObject.GetComponent<CapsuleCollider2D>().size.x);
            if (hit.collider != null)
            {
                Debug.Log(hit.point);
            }
            */
          
            StartCoroutine("DamageFlash");
        }
    }

    IEnumerator DamageFlash()
    {
        GetComponent<SpriteRenderer>().color = redColor;
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().color = baseColor;
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().color = redColor;
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().color = baseColor;


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.transform.tag == "ladders")
        {
            onLadder = false;            
        }

        if (collision.transform.parent != null)
        {
            if (collision.transform.parent.tag == "enemies")
                collision.transform.parent.GetComponent<BaddieScript>().playerSeen = false;
        }
    }
}
