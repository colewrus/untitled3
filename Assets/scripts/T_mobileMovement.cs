using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class T_mobileMovement : MonoBehaviour {

    //GUI
    public GUIStyle skin;

    private Vector3 position;
    private float width;
    private float height;

    //Movement
    public float speed;
    public float jumpMin; //minimum swipe distance to jump
    public float jumpPower;


    [Range(0.0f, 15.0f)]
    public float jumpStopDrag;

    Animator anim;

    Rigidbody2D rb;

    float tapTimer;
    public float swingTimer;

    Vector2 deltaSwipe;
    bool menu;
    public GameObject debugPanel;

    void Awake()
    {
        width = (float)Screen.width / 2.0f;
        height = (float)Screen.height / 2.0f;

        // Position used for the cube.
        position = new Vector3(0.0f, 0.0f, 0.0f);
        menu = false;
        debugPanel.SetActive(false);
        
    }

    void OnGUI()
    {
        // Compute a fontSize based on the size of the screen width.
        GUI.skin.label.fontSize = (int)(Screen.width / 25.0f);

        GUI.Label(new Rect(20, 20, width, height * 0.25f),
            "x = " + position.x.ToString("f2") +
            ", y = " + position.y.ToString("f2"));

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
	void Update () {

        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            tapTimer += 1 * Time.deltaTime;


            

            if(Camera.main.ScreenToWorldPoint(touch.position).x < transform.position.x)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }else
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }


            // Move the cube if the screen has the finger moving.
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 pos = touch.position;

                //go ahead and move
                if(tapTimer > swingTimer*2)
                {
                    int direction = (pos.x > (Screen.width / 2)) ? 1 : -1;


                    pos.x = (pos.x - width) / width;
                    pos.y = (pos.y - height) / height;
                    position = new Vector3(pos.x, pos.y, 0.0f);
                    rb.velocity = new Vector3((float)direction * speed, rb.velocity.y, 0);


                    transform.Translate(new Vector3(pos.x * speed, 0, 0) * Time.deltaTime, Space.World);
                }


            }

            if(touch.phase == TouchPhase.Ended)
            {

               if(tapTimer < swingTimer)
                {
                    Debug.Log("swing");
                    anim.SetTrigger("attack");
                }

                Vector3 worldRelease = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));
                float yDist = Vector2.Distance(new Vector3(0,transform.position.y, 0),new Vector3(0,worldRelease.y, 0));
                
                deltaSwipe = touch.deltaPosition;

                //do a jump
                if(yDist > jumpMin)
                {
                    gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up*jumpPower, ForceMode2D.Impulse);
                    anim.SetTrigger("jump");
                    anim.SetBool("floored", false);
                    rb.velocity = new Vector2(jumpStopDrag, rb.velocity.y);
                        
                }

                tapTimer = 0;

            }
        }
		
	}


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "floor")
        {
            Debug.Log("ground");
            anim.SetBool("floored", true);
        }
    }

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
        jumpMin = float.Parse(field.text);
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
