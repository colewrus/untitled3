using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    public static PlayerScript instance = null;
    public float speed;
    public float jumpPower;

    public GameObject aimReticule;
    Collider2D aimCollider;
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

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        rb = this.GetComponent<Rigidbody2D>();
        aimCollider = aimReticule.GetComponent<Collider2D>();
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
       
	}
	
	// Update is called once per frame
	void Update () {
        PlayerMove();
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
        LadderMovement();
        Reticule();
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

    void Shoot()
    {

        Vector2 rayDest = Input.mousePosition;
       
        rayDest = Camera.main.ScreenToWorldPoint(rayDest);
              
        Vector2 destinationActual = rayDest - new Vector2(transform.position.x, transform.position.y);

        hit2d = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), destinationActual * gunRange);

        if(hit2d.collider != null)
        {
            part.transform.position = hit2d.point;
            part.Play();
            Debug.Log(hit2d.point);
            if(hit2d.collider.transform.tag == "enemies")
            {
                enemyCollider.Remove(hit2d.collider);
                Destroy(hit2d.collider.gameObject);
                GM.instance.RemoveEnemy();
            }
        }

        Ray myRay = new Ray(transform.position, destinationActual*gunRange);
        Physics2D.Raycast(transform.position, destinationActual, gunRange);  
        Debug.DrawRay(transform.position, destinationActual*gunRange, Color.cyan);
        
    }

    void PlayerMove()
    {
        horiz = Input.GetAxis("Horizontal");   

        rb.velocity = new Vector3(horiz * speed, Mathf.Clamp(rb.velocity.y, -10, 10));


        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (onPlatform)
            {
                //this.GetComponent<CapsuleCollider2D>().enabled = !GetComponent<CapsuleCollider2D>().enabled;
                GetComponent<CapsuleCollider2D>().enabled = !GetComponent<CapsuleCollider2D>().enabled;
                Invoke("DownThrough", 0.25f);          
            }
        }
    }

    void DownThrough()
    {
        Debug.Log("invoked");
        GetComponent<CapsuleCollider2D>().enabled = !GetComponent<CapsuleCollider2D>().enabled;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "platform") //check to see if we're on platform to enable down jump-through
        {
            onPlatform = true;
        }
    }

    void LadderMovement()
    {
        if (onLadder)
        {
            rb.gravityScale = 0;
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                rb.velocity = transform.up * 0.75f;
            }
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                rb.velocity = Vector3.zero;
            }
            if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                rb.velocity = transform.up * -0.75f;
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
        if(collision.transform.tag == "ladders")
        {
            onLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.transform.tag == "ladders")
        {
            onLadder = false;            
        }
    }
}
