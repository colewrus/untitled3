using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class math_move_test : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float frequency = 20.0f;
    public float magnitude = 0.5f;

    Vector3 axis;
    Vector3 pos;
    Vector3 dir;
    float Xtime;

    public bool jumping, onPlatform;
    public float groundMeasure = 1.0f;
    public float fallSpeed;
    private float fallRate, fallTick;
  

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        axis = transform.up;
        dir = transform.right;
        jumping = false; 
        
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();

        GroundDetect();

        Fall();

        MoveControls();
    }


    void Inputs()
    {

     

        if (Input.GetKeyDown(KeyCode.Space))
        {
            pos = transform.position;
            jumping = true;
            onPlatform = false;
        }



        if (Input.GetKey(KeyCode.Space))
        {

            if (Xtime < 1)
            {
//rising animation
                Xtime += Time.deltaTime;
                Debug.Log(Xtime);
                //pos += dir * Time.deltaTime * moveSpeed;

                //pos = new Vector3(0, Mathf.Clamp(Mathf.Log(Mathf.Pow(Xtime, 5) * 5+2), 0, 12), 0);

                pos = new Vector3(0, 2+Mathf.Clamp(Mathf.Sin((frequency * Xtime) * magnitude), 0, 10));
               // pos = new Vector3(0, (3*Mathf.Sin(frequency * Xtime) * magnitude), 0);
                transform.Translate(pos*Time.deltaTime);
                //transform.position = pos;
                //pos + axis * Mathf.Sin(Time.time * frequency) * magnitude;
            }
            else
            {
                jumping = false;
                onPlatform = false;
 //can set to a falling animation here
            }

        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            //use time gate for a minimum jump
            Xtime = 0;
            jumping = false;
            if(onPlatform)
                onPlatform = false;
        }

    }

    void MoveControls()
    {
        if (Input.GetAxis("Horizontal") > 0){
            dir = Vector2.right;
        }else if(Input.GetAxis("Horizontal") < 0)
        {
            dir = Vector2.left;
        }
        else
        {
            dir = Vector2.zero;
        }


            transform.Translate(dir*Time.deltaTime, Space.World);
    }

    void Fall()
    {
        if (!jumping && !onPlatform)
        {
            //fall speed should speed up to a maximum
            fallTick += Time.deltaTime;
          
            fallRate = Mathf.Clamp(Mathf.Pow(Mathf.Log(fallTick), Mathf.PI), 0, 12);
            Debug.Log(fallTick);
            transform.Translate((Vector2.down * fallRate)*Time.deltaTime);
            //transform.position += (Vector3.up * -fallSpeed) * Time.deltaTime;
        }
    }

    void GroundDetect()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundMeasure);

        Debug.DrawRay(transform.position, Vector2.down, Color.red * groundMeasure);

        if(hit.collider != null)
        {
            
            if(hit.collider.tag == "floor")
            {
                onPlatform = true;
            }
        }

    }
}
