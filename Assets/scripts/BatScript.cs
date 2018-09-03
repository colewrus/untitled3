using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatScript : MonoBehaviour {

    public GameObject Target;
    public Vector3 flyDest;
    public float speed;
    public bool attack;
    public bool search;
    public float searchDelay;
    public BoxCollider2D ActiveSpace;

    public bool aggro; //used to give player a window to escape aggro range
    public float aggroTimer;

    Vector3 windBack;
    Vector3 dashDest;

    public int health;

    //Random variablies
    [Tooltip("Is this bat part of a boss summoning?")]
    public bool summoned;
    [Tooltip("Provide the boss object so we can decrease the summoned count")]
    public GameObject Boss_Summoner;
    bool takeDamage;

    public float damageResetTimer;


	// Use this for initialization
	void Start () {
        flyDest = transform.position;
      
		
	}

    private void Awake()
    {
        takeDamage = true;
    }

    // Update is called once per frame
    void Update () {
        BatMove();

        if (attack)
        {
            BatAttack();
        }
        
        
	}


    private void BatMove()
    {

        if(Target != null && !attack)
        {
           
            //ok but limit the y movement
            transform.position += (Target.transform.position - transform.position).normalized * speed * Time.deltaTime;
            Vector3 pos = (Target.transform.position - transform.position);

            if (pos.magnitude < 0.95f)
            {
                windBack = transform.position + (Target.transform.position - transform.position).normalized * -1.25f;
                dashDest = Target.transform.position;
                aggro = true;
                attack = true;
             
            }

        }else if(Target == null && !attack)
        {

            transform.position += (flyDest - transform.position).normalized * speed * Time.deltaTime;
            Vector3 pos = (flyDest - transform.position);
            
            if (pos.magnitude < 0.15f)
            {
                StartCoroutine("MoveDelay");               
            }
        }
    }


    void BatAttack()
    {
        if (aggro)
        {
                      
            transform.position += (windBack - transform.position).normalized * (speed/2) * Time.deltaTime;
            Vector3 pos = (windBack - transform.position);
        
            if(pos.magnitude < 0.15f)
            {
                
                aggro = false;
            }        
        }

        if (!aggro)
        {
            transform.position += (dashDest - transform.position).normalized * (speed * 2.3f) * Time.deltaTime;
            Vector3 pos = (dashDest - transform.position);

            if(pos.magnitude < 0.15f)
            {
                StartCoroutine("AggroDelay");
            }
        }

        //wind up at 30% move speed
        //once within 10% of moveback target
        //store player position
        //dash toward stored position 
        //start and wait for aggro cooldown
    }

    IEnumerator MoveDelay()
    {
        yield return new WaitForSeconds(searchDelay);
        flyDest = new Vector3(Random.Range(ActiveSpace.bounds.min.x, ActiveSpace.bounds.max.x), Random.Range(ActiveSpace.bounds.min.y, ActiveSpace.bounds.max.y), 0);
    
    }

    IEnumerator AggroDelay()
    {
        attack = false;
        yield return new WaitForSeconds(aggroTimer);
        attack = true;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Target = collision.gameObject;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !aggro)
        {
            Target = null;
            attack = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Target = collision.gameObject;
        }

        if(collision.tag == "hitbox"){
            if(takeDamage){
                health = health - 1;

                if(health <= 0){

                }
                takeDamage = false;
                //play damage sound
                StartCoroutine("DamageReset");
            }


        }
    }


    IEnumerator DamageReset(){
        yield return new WaitForSeconds(damageResetTimer);
        takeDamage = true;
    }
}
