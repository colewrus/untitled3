using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BossType { bat, ghost, golem};

public enum BatState { search, intro, attack4, attack8, summon, wait}

public class Boss_Script : MonoBehaviour {

    public BossType thisBoss;
    public BatState _BatState;

    public float timer; //how long between actions

    public bool battleMode; //has the player triggered the boss fight
    public float health;

    public GameObject bossCanvas;
    public Slider healthBar;
    public Text healthBarText;
    public string bossName;
    public float searchTimer;
    float searchTick;
    int destCounter;
    int destMax;
    public List<Transform> SearchDests = new List<Transform>();
    public float searchSpeed;

    public Transform attack4Dest;
    public Transform attack8Dest;
    public bool attack4Reposition = false;
    float shotCount;
    public float attack4Timer;
    float attack4Tick;
    public int attack4Max; //maximum number of 4-dir shots bat takes before moving
    int attack4Count;

	// Use this for initialization
	void Start () {
        destMax = SearchDests.Count;
        destCounter = 1;
        searchTick = 0;
        Debug.Log("Dest Max: " + destMax);
        Physics2D.IgnoreCollision(GameObject.Find("player").GetComponent<CapsuleCollider2D>(), GetComponent<BoxCollider2D>());
        Physics2D.IgnoreCollision(GameObject.Find("floors").GetComponent<Collider2D>(), GetComponent<BoxCollider2D>());
    }
	
	// Update is called once per frame
	void Update () {
        BatBehavior();
	}

    void BatBehavior()
    {
        if (_BatState == BatState.intro)
        {
            //set the animation
        }

        if (_BatState == BatState.search)
        {
            if(searchTick < searchTimer)
            {
                Debug.Log("Search Dest: " + destCounter);
                searchTick += 1 * Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, SearchDests[destCounter - 1].position, searchSpeed * Time.deltaTime);
                                 
            }
            else
            {
                
                if(destCounter == destMax)
                {
                    destCounter = 1;
                    _BatState = BatState.attack4;
                    attack4Reposition = true;
                }
                else
                {
                    destCounter++;
                }                
                searchTick = 0;
            }
        }

        if(_BatState == BatState.attack4)
        {
            if (attack4Reposition)
            {
                transform.position = Vector3.Lerp(transform.position, attack4Dest.position, 0.75f * Time.deltaTime);
            }else
            {
                Debug.Log("I should be shooting");
                if(attack4Tick < attack4Timer)
                {
                    attack4Tick += 1 * Time.deltaTime;
                }
                else
                {
                    if(attack4Count < attack4Max)
                    {
                        Fire4();
                    }
                    else
                    {
                        attack4Count = 0;

                        _BatState = BatState.search;
                    }
                                       
                }
                //Fire4();
            }
        
        }
    }


    void Fire4()
    {
        Debug.Log("Shot: " + shotCount);
        if(shotCount < 4)
        {
            GameObject bull = GM.instance.GetBullets();
            Vector3 target = Vector3.zero;

            if (bull != null)
            {
                if(shotCount == 0)
                {
                    target = Vector3.up;
                }else if(shotCount == 1)
                {
                    target = Vector3.right;
                }
                else if (shotCount == 2)
                {
                    target = Vector3.up * -1;
                }
                else if (shotCount == 3)
                {
                    target = Vector3.right*-1;
                }


                bull.transform.position = this.transform.position;
                bull.GetComponent<BulletScript>().target = Vector3.up;
                bull.GetComponent<BulletScript>().speed = 5;
                bull.SetActive(true);
                bull.GetComponent<Rigidbody2D>().velocity = target.normalized * 5;
            }
            shotCount++;
            
        }
        else
        {
            shotCount = 0;
            attack4Tick = 0;
            attack4Count++;
            //_BatState = BatState.search;
            //shotCount = 0;
        }


    }

    IEnumerator Attack4Setup()
    {
        yield return new WaitForSeconds(1.15f);
        shotCount = 0;
        Fire4();
        Debug.Log("co_1");
        yield return new WaitForSeconds(1.15f);
        shotCount = 0;
        Fire4();
        Debug.Log("co_2");
        yield return new WaitForSeconds(1.15f);
        shotCount = 0;
        Fire4();
        Debug.Log("co_3");
    }

    public void InitCanvas()
    {
        bossCanvas.SetActive(true);
        healthBar.maxValue = health;
        healthBar.value = health;
        healthBarText.text = bossName;
    }

    public void ReceiveDamage(float d) 
    {
        health -= d;
        healthBar.value = health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("help");
        if(_BatState == BatState.attack4)
        {
            
            attack4Reposition = false;
        }
    }

}
