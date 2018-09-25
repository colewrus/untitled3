using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BossType { bat, ghost, golem};

public enum BatState { search, intro, attack4, attack8, summon, wait, dead}

public class Boss_Script : MonoBehaviour {

    public BossType thisBoss;
    public BatState _BatState;

    public float timer; //how long between actions

    public bool battleMode; //has the player triggered the boss fight
    public float health;
    float maxHealth; //so we can compare during the fight
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
    int shotCount;
    public float attack4Timer; //time between the 4-direction shot
    float attack4Tick;
    public int attack4Max; //maximum number of 4-dir shots bat takes before moving
    int attack4Count;

    public List<Vector3> shot20Dir = new List<Vector3>(); //order will affect the order shit is fired in
    public float shot20Timer; //time between each bullet shot in the 20 bullet burst, this'll be fast
    float shot20Tick;
    public bool fire20cooldown;
    public float v_fire20Cooldown;

    int randomSearchInt; //random index for search destinations

    public List<AudioClip> musicClips = new List<AudioClip>();
    //Summon Variables
    public List<GameObject> minions = new List<GameObject>();
    public List<Transform> minionSpawn = new List<Transform>();
    public int summonCount;
    public Transform waitPos;
    public float waitTimer;
    float waitTick;
    bool summoned; //have you already called for the new wave of bats?



    GameObject deathObject;
    float deathResetTimer; //how long before you get teleported out
    bool deathTriggered; //so we just call some of the death stuff once
    public Font BatBossFont;
    float deathTick;
    GameObject newText;

    public GameObject teleporter;
    public Vector3 dest;

	// Use this for initialization
	void Start () {
        destMax = SearchDests.Count;
        destCounter = 1;
        searchTick = 0;
      
        Physics2D.IgnoreCollision(GameObject.Find("player").GetComponent<CapsuleCollider2D>(), GetComponent<BoxCollider2D>());
        Physics2D.IgnoreCollision(GameObject.Find("floors").GetComponent<Collider2D>(), GetComponent<BoxCollider2D>());
        fire20cooldown = false;
        summoned = false;
        maxHealth = health;
        summonCount = 0;
    }

	
	// Update is called once per frame
	void Update () {
        BatBehavior();
	}

    void NewIndex()
    {
        int temp = Random.Range(0, SearchDests.Count);
        if (temp != randomSearchInt)
        {
            randomSearchInt = temp;          
        }
        else
        {
            NewIndex();
        }
    }

    void BatBehavior()
    {
        if (_BatState == BatState.intro)
        {
            //set the animation
        }

        if (_BatState == BatState.search)
        {
            Search();
        }

        if(_BatState == BatState.attack8)
        {
            Position_Fire20();
        }

        if(_BatState == BatState.summon)
        {
            Summon();
        }

        if(_BatState == BatState.attack4)
        {
            Attack4();        
        }
        
        if(_BatState == BatState.dead)
        {
            BatDead();
        }
    }

    void Search()
    {
        if (searchTick < searchTimer)
        {
            searchTick += 1 * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, SearchDests[randomSearchInt].position, searchSpeed * Time.deltaTime);

        }
        else
        {
            if (destCounter == destMax-1)
            {
                destCounter = 0;
                float roll = Random.RandomRange(0, 10);
                if (roll <= 4)
                {
                    _BatState = BatState.attack4;
                }
                else
                {
                    if (roll > 4 && roll < 8)
                        _BatState = BatState.attack8;
                    if (roll >= 8)
                        _BatState = BatState.summon;
                }

                attack4Reposition = true;
            }
            else
            {
                NewIndex();
                searchTick = 0;
                destCounter++;
            }
        }
    }

    void Attack4()
    {
        if (attack4Reposition)
        {
            transform.position = Vector3.Lerp(transform.position, attack4Dest.position, 0.75f * Time.deltaTime);
        }
        else
        {
            if (attack4Tick < attack4Timer)
            {
                attack4Tick += 1 * Time.deltaTime;
            }
            else
            {
                if (attack4Count < attack4Max)
                {
                    Fire4();
                }
                else
                {
                    attack4Count = 0;

                    _BatState = BatState.search;
                }
            }
        }
    }

    void Summon()
    {
        //got to summoning spot

        //if tick < waitTimer and minions are active then count
        //else go back to search
        transform.position = Vector3.Lerp(transform.position, waitPos.position, searchSpeed * Time.deltaTime);

        if (!summoned)
        {
            //spawn bats
            for (int i = 0; i < minions.Count; i++)
            {
                minions[i].transform.position = minionSpawn[i].position;
                minions[i].SetActive(true);
                summonCount++;
            }
            summoned = true;            
        }

        if(summoned && waitTick < waitTimer)
        {
            waitTick += Time.deltaTime;

            if(summonCount <= 0){
                destCounter = 0;
                _BatState = BatState.search;
            }

        }else
        {
            destCounter = 0;
            _BatState = BatState.search;
        }
      
    }

    void CheckMinions()
    {
        int check = 0;
        
        for(int i = 0; i < minions.Count; i++)
        {
            
            if (!minions[i].active)
            {
                check++;
            }
        }
        if (check == minions.Count)
        {
            destCounter = 0;
            _BatState = BatState.search;
        }
    }

    void Position_Fire20()
    {
        if (attack4Reposition)
        {
            transform.position = Vector3.Lerp(transform.position, attack4Dest.position, 1.25f * Time.deltaTime);
        }else
        {          
            Fire20();
        }

        if (fire20cooldown && !attack4Reposition)
        {

            if(shot20Tick < v_fire20Cooldown)
            {
                shot20Tick += 1 * Time.deltaTime;
            }else
            {
                shotCount = 0;
                fire20cooldown = false;
                _BatState = BatState.search;
            }
        }
        
    }

    void Fire20()
    {

        if(shotCount < 20)
        {
            GameObject bull = GM.instance.GetBullets();
            Vector3 target = Vector3.zero;

            if(shot20Tick < shot20Timer)
            {
                shot20Tick += Time.deltaTime;
            }
            else
            {
                var theta = -2 * Mathf.PI * shotCount / 20;
                Vector3 tempV = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0) * 2;
                target = tempV;                
                Debug.DrawLine(transform.position, transform.position + tempV, Color.blue, 3);

                //ran into a null reference error, making a hard break
                if(bull != null){
                    bull.transform.position = this.transform.position;
                    bull.transform.localScale = new Vector3(1.5f, 1.5f, 0);
                    bull.GetComponent<BulletScript>().speed = 5;
                    bull.SetActive(true);
                    bull.GetComponent<Rigidbody2D>().velocity = target.normalized * 5;
                    shotCount++;
                    shot20Tick = 0;
                }else{
                    fire20cooldown = true;
                    return;
                }
           
             }
        }else
        {
          
            fire20cooldown = true;
        }
    }
    void Fire4()
    {
       
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

               
                bull.transform.localScale = new Vector3(1.5f, 1.5f, 0);
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
            _BatState = BatState.search;
            shotCount = 0;
        }


    }

    IEnumerator Attack4Setup()
    {
        yield return new WaitForSeconds(1.15f);
        shotCount = 0;
        Fire4();
        yield return new WaitForSeconds(1.15f);
        shotCount = 0;
        Fire4();
        yield return new WaitForSeconds(1.15f);
        shotCount = 0;
        Fire4();
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
        if(_BatState != BatState.summon)
        {
            health -= d;
            healthBar.value = health;
        }

        if(health <= (maxHealth / 2))
        {
            _BatState = BatState.summon;
        }

        if(health <= 0)
        {
            SpriteRenderer tempR = gameObject.GetComponent<SpriteRenderer>();
            tempR.enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            _BatState = BatState.dead;
        }
        
    }

    void BatDead()
    {

        if (!deathTriggered)
        {
            //disable minions
            for(int i=0; i < minions.Count; i++)
            {
                minions[i].GetComponent<Animator>().SetTrigger("die");
            }


            newText = new GameObject("winText", typeof(RectTransform));
            var newTextComp = newText.AddComponent<Text>();
            newTextComp.text = "Vile Bat Defeated";
            newTextComp.alignment = TextAnchor.MiddleCenter;
            newTextComp.font = BatBossFont;
            newTextComp.fontSize = 24;
            newTextComp.fontStyle = FontStyle.Bold;
            

            newText.transform.SetParent(GameObject.Find("Boss-Canvas").transform);
            newText.AddComponent<Outline>();
            newText.GetComponent<RectTransform>().anchoredPosition = new Vector2(6, 6);
            //teleport shit
            teleporter.GetComponent<teleportScript>().StartTP();
            teleporter.GetComponent<teleportScript>().TPDest(dest);
            teleporter.GetComponent<teleportScript>().StartCoroutine("TP");
            deathTick = 0;
            PlayerScript.instance.moveLock = true;
            deathTriggered = true;
        }

        if (deathTriggered)
        {
            if(deathTick < 2.5f)
            {
                deathTick += 1 * Time.deltaTime;
            }
            else
            {
                PlayerScript.instance.moveLock = false;
                newText.SetActive(false);
                healthBar.gameObject.SetActive(false);
                healthBarText.gameObject.SetActive(false);
            }
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {      
        if(collision.transform.name == "attack4Pos")
        {         
            if (_BatState == BatState.attack4 || _BatState == BatState.attack8)
            {
                attack4Reposition = false;
            }
        }

        if(collision.transform.tag == "hitbox")
        {
            if (collision.transform.GetComponent<Melee_Hitbox>().armed)
            {
                if (health - collision.transform.GetComponent<Melee_Hitbox>().Damage <= 0)
                {
                    ReceiveDamage(collision.transform.GetComponent<Melee_Hitbox>().Damage);
                }
                else
                {
                    ReceiveDamage(collision.transform.GetComponent<Melee_Hitbox>().Damage);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.name == "attack4Pos")
        {
            if (_BatState == BatState.attack4 || _BatState == BatState.attack8)
            {
                attack4Reposition = false;
            }
        }
        if (collision.transform.tag == "hitbox")
        {
            if (collision.transform.GetComponent<Melee_Hitbox>().armed)
            {
                if (health - collision.transform.GetComponent<Melee_Hitbox>().Damage <= 0)
                {
                    ReceiveDamage(collision.transform.GetComponent<Melee_Hitbox>().Damage);
                }
                else
                {
                    ReceiveDamage(collision.transform.GetComponent<Melee_Hitbox>().Damage);
                }
                collision.transform.GetComponent<Melee_Hitbox>().armed = false;
            }
        }
    }

}
