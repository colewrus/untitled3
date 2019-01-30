using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class T_mobileMovement : MonoBehaviour {



    private Vector3 position;
    private float width;
    private float height;

    //Movement
    public float speed;
    public float jumpMin; //minimum swipe distance to jump
    public float jumpPower;
    Vector2 startPos;

    [Range(0.0f, 15.0f)]
    public float jumpStopDrag;
    bool jump;

    Animator anim;

    Rigidbody2D rb;

  
    public float tapTimer;

    [Header("Attack Variables")]
    //Attack vars
    public float swingTimer;
    //public GameObject swordHitBox;
    public LayerMask playerMask;
    [Tooltip("Sets how far the raycast for the attack shoots")]
    public float attackRange;
    public float damage;

    Vector2 deltaSwipe;
    bool menu;
    public GameObject debugPanel;

    //PC controls
    float horiz;

    void Awake()
    {
       
        
        width = (float)Screen.width / 2.0f;
        height = (float)Screen.height / 2.0f;

        // Position used for the cube.
        position = new Vector3(0.0f, 0.0f, 0.0f);
        menu = false;
        debugPanel.SetActive(false);
      //  swordHitBox.SetActive(false);
        jump = false;
        
    }

    void OnGUI()
    {
        // Compute a fontSize based on the size of the screen width.
    

        GUI.skin.horizontalSlider.fixedHeight = (int)(Screen.height / 25.0f);
        GUI.skin.horizontalSliderThumb.fixedHeight = (int)(Screen.height / 25.0f);
        GUI.skin.horizontalSliderThumb.fixedWidth = (int)(Screen.height / 25.0f);
        if (menu)
        {
            jumpStopDrag = GUI.HorizontalSlider(new Rect(120, Screen.height - ((Screen.height / 20.0f)) , width, height * 0.25f), jumpStopDrag, 0, 15);
            jumpPower = GUI.HorizontalSlider(new Rect(120, Screen.height - (2*(Screen.height / 20.0f)), width, height * 0.25f), jumpPower, 0, 15);

        }
            
    }


    // Use this for initialization
    void Start () {
        Debug.Log("Init");
        anim = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {


#if UNITY_STANDALONE || UNITY_WEBPLAYER
        PCMovement();
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        MobileMovement();

#endif

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "floor")
        {
            anim.SetBool("floored", true);
        }

        if(collision.transform.tag == "platform" && rb.velocity.y == 0)
        {
            anim.SetBool("floored", true);
        }

    }

    void PCMovement()
    {
        if (Input.GetKeyDown(KeyCode.Space)  && !jump)
        {
            jump = true;
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetTrigger("jump");
            anim.SetBool("floored", false);
        }

        if (anim.GetBool("floored"))
        {
            jump = false;
        }

        horiz = Input.GetAxis("Horizontal");

        if(horiz < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (horiz > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles = transform.eulerAngles;
        }

        rb.velocity = new Vector2(horiz * speed, rb.velocity.y);
        if (Input.GetMouseButtonDown(0))
        {
            Attack((Vector2)Input.mousePosition);
            //swordHitBox.SetActive(true);
            anim.SetTrigger("attack");
        }
    }


    //Movement
    void MobileMovement()
    {

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            tapTimer += 1 * Time.deltaTime;



            if (touch.phase == TouchPhase.Began)
                startPos = touch.position;


            if (Camera.main.ScreenToWorldPoint(touch.position).x < transform.position.x)
            {
                //gameObject.GetComponent<SpriteRenderer>().flipX = true;
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                //gameObject.GetComponent<SpriteRenderer>().flipX = false;
                transform.eulerAngles = new Vector3(0, 0, 0);
            }



            Vector2 pos = touch.position;

            //go ahead and move
            if (tapTimer > swingTimer)
            {
           
                int direction = (pos.x > (Screen.width / 2)) ? 1 : -1;


                pos.x = (pos.x - width) / width;
                pos.y = (pos.y - height) / height;
               
                position = new Vector3(pos.x, pos.y, 0.0f);



                if (!jump)
                {
                    //float xSpeed = Mathf.Clamp((direction * ((speed / 2) + tapTimer)), -speed, speed);
                   
                    float xSpeed = Mathf.Clamp(direction * speed, -speed, speed);              
                    rb.velocity = new Vector3(xSpeed, rb.velocity.y, 0);
                }
                else
                {
                    float xSpeed = Mathf.Clamp((direction * ((speed / 2) + tapTimer)), -speed/2, speed/2);
                }


            }




            if (touch.phase == TouchPhase.Ended)
            {
                
                Attack(touch.position);
                

                float yDist = touch.position.y - startPos.y;
                Vector3 worldRelease = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));
                //float yDist = Vector2.Distance(new Vector3(0,transform.position.y, 0),new Vector3(0,worldRelease.y, 0));

                deltaSwipe = touch.deltaPosition;

                //do a jump
                if (yDist > jumpMin)
                {
                    rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                    anim.SetTrigger("jump");
                    anim.SetBool("floored", false);

                }

                tapTimer = 0;

            }
        }

    }



    //attack functions
    void Attack(Vector2 p)
    {
        Vector2 newP = Camera.main.ScreenToWorldPoint(p) - transform.position;
      
        RaycastHit2D hit = Physics2D.Raycast(transform.position, newP, attackRange, playerMask);

        Debug.DrawRay(transform.position, newP, Color.red, attackRange);
        

        if(hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name);
        }




        if (tapTimer < swingTimer)
        {
            Vector2 localTap = Camera.main.ScreenToWorldPoint(p);
     
           if(hit.collider.tag == "swordman")
            {
                Debug.Log("hit the swordsman proper");
                hit.collider.transform.GetComponentInParent<Swordsman>().HitReg(damage);
            }

            anim.SetTrigger("attack");
        }
    }

    public void DeactivateHitbox()
    {
        //swordHitBox.SetActive(false);
    }



    //Test Controls
    public void menuToggle()
    {
        menu = !menu;
        if (menu)
        {
            debugPanel.SetActive(true);
        }
        else
        {
            debugPanel.SetActive(false);
        }
    }

    public void setSpeed(InputField field)        
    {
        speed = float.Parse(field.text);
    }

    public void setJump(InputField field)
    {
        jumpPower = float.Parse(field.text);
    }


    public void setTimer(InputField field)
    {
        swingTimer = float.Parse(field.text);
    }

    public void ResetScene()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

}
