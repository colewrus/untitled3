using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class T_mobileMovement : MonoBehaviour {


    private Vector3 position;
    private float width;
    private float height;

    public float speed;
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
    }


    // Use this for initialization
    void Start () {
        Debug.Log("Init");
		
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Move the cube if the screen has the finger moving.
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 pos = touch.position;
           

                int direction = (pos.x > (Screen.width / 2)) ? 1 : -1;          

        
                pos.x = (pos.x - width) / width;
                pos.y = (pos.y - height) / height;
                position = new Vector3(pos.x, pos.y, 0.0f);
             
                    
                transform.Translate(new Vector3(pos.x*speed, 0, 0) * Time.deltaTime, Space.World);
            }

            if(touch.phase == TouchPhase.Ended)
            {

                
                Vector3 worldRelease = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));
                float yDist = Vector2.Distance(new Vector3(0,transform.position.y, 0),new Vector3(0,worldRelease.y, 0));
                Debug.Log(worldRelease);
                deltaSwipe = touch.deltaPosition;
                if(yDist > speed)
                {
                    gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up*5, ForceMode2D.Impulse);
                }
                //Debug.Log("deltaY " + yDist);
                //Debug.Log("delta: " + deltaSwipe);
            }
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

}
