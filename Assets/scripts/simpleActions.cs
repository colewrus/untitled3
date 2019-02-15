using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleActions : MonoBehaviour {


    public float attackTimer;
    public float tapTimer;
    public GameObject rock;
    public float projectileSpawnGap;
    public bool attackGate; //false = ready to attack. True = closed, 
    public int rockPool;
    public List<projectile> Armory = new List<projectile>();
    public int currentWeapon;
    List<GameObject> rockList = new List<GameObject>();


   

	// Use this for initialization
	void Start () {

        currentWeapon = 0;

        for(int i=0; i<rockPool; i++)
        {
            GameObject r = Instantiate(Armory[0].prefab, Vector3.zero, Quaternion.identity);
            r.GetComponent<projectileBehavior>().myType = Armory[0];
            rockList.Add(r);
            r.SetActive(false);
        }
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (Input.touchCount > 0)
        {

           

            if (Armory[currentWeapon].name == "Arrow")
            {
                //MULTI TOUCH
                Touch[] touches = Input.touches;
                Touch touchOne = Input.GetTouch(0); //can measure distance between for draw distance
                Touch touchTwo = Input.GetTouch(1);


                /*
                foreach (Touch c in touches) {
                    Debug.Log(Camera.main.ScreenToWorldPoint(c.position));
                }
                */
               // Debug.Log(touchOne.position + " | " + touchTwo.position);

                //TAP AND HOLD
                //works opposite of short tap time, the longer you hold the further towards the destination it will travel;
            }
         

            Touch touch = Input.GetTouch(0);
            tapTimer += 1 * Time.deltaTime;
            if (touch.phase == TouchPhase.Began)
            {
                attackGate = false;
                tapTimer = 0;
            }

            if(touch.phase == TouchPhase.Ended)
            {
                if(tapTimer < attackTimer && !attackGate)
                {
                  
                    if(currentWeapon == 0)
                    {
                        Debug.Log("should throw the rock");
                        GameObject thisRock = GetRock();
                        Vector2 tapDir = Camera.main.ScreenToWorldPoint(touch.position);
                        if (thisRock == null)
                            return;
                        if (tapDir.x - this.transform.position.x < 0)
                        {
                            thisRock.transform.position = this.transform.position + (transform.right * -projectileSpawnGap);
                            this.GetComponent<SpriteRenderer>().flipX = true;

                        }
                        else
                        {
                            thisRock.transform.position = this.transform.position + (transform.right * projectileSpawnGap);
                            this.GetComponent<SpriteRenderer>().flipX = false;
                        }
                        thisRock.SetActive(true);

                        Vector2 throwDir = tapDir - new Vector2(this.transform.position.x, this.transform.position.y);
                        thisRock.GetComponent<Rigidbody2D>().AddForce(throwDir.normalized*Armory[currentWeapon].speed, ForceMode2D.Impulse);
                        Debug.Log(thisRock.GetComponent<Rigidbody2D>().velocity);
                    }

                    Debug.Log("attack");
                    attackGate = true;

                }
              
              
            }
        }
		
	}


  

    public GameObject GetRock()
    {
        for(int i=0; i < rockList.Count; i++)
        {
            if (!rockList[i].activeInHierarchy)
            {
                return rockList[i];
            }
        }
        return null;
    }

}
