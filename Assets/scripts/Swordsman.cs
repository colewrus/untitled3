using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordsman : MonoBehaviour {


    public GameObject target;
    public float moveSpeed;
    public bool playerSeen;
    public bool attack;

    bool attackStart; //primarily to control the animation of the attack

    public GameObject swordObj;//this is the collider for the sword
    Animator myAnim;
    public float attackDist;

    //health and damage variables
    public float health;
    bool takeDamage;
    [Tooltip("How long until the body disappears")]
    public float decayTimer;
    [Tooltip("How long until takes damage again")]
    public float dmgReset;


    public bool hardFreeze;

    // Use this for initialization
    void Start () {
        playerSeen = false;
        attack = false;
        myAnim = this.GetComponent<Animator>();
        swordObj.SetActive(false);
	}

    private void Awake()
    {
        takeDamage = true;
    }

    // Update is called once per frame
    void Update () {

        if(!hardFreeze){
            if (playerSeen && !attack)
            {
                //ok but limit the y movement
                transform.position += (target.transform.position - transform.position).normalized * moveSpeed * Time.deltaTime;
                Vector3 pos = (target.transform.position - transform.position);

                if (pos.x > 0)
                {
                    transform.eulerAngles = new Vector3(0, 180, 0);
                }

                if (pos.x < 0)
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }

                if (pos.magnitude < attackDist)
                {
                    attack = true;
                }

                //run the baddie attack
                if (attack)
                {
                    //Start animation and run through animation events?
                    if (!attackStart)
                    {
                        myAnim.SetTrigger("knightWindUp");
                        //start animation
                    }
                    attackStart = true;
                }
            }

        }
       

       
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.transform.tag == "Player")
        {
            playerSeen = true;
            target = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            playerSeen = false;
            attack = false;       
            Debug.Log("reset");
            //reset the other triggers too
        }
    }

    public void SetTrigger(string TriggerID)
    {
        myAnim.SetTrigger(TriggerID);
    }

   public void AttackEnd()
    {
        swordObj.SetActive(false);
        attack = false;
        attackStart = false;
    }


    public void HitReg(float dmg){
        if(takeDamage){
            health -= dmg;
            if(health <= 0){
                Debug.Log(this.name + " ded");
                hardFreeze = true;
                GetComponent<Animator>().SetTrigger("die");
                StartCoroutine("BodyDecay");
            }
            GetComponent<Animator>().SetTrigger("damage");
            takeDamage = false;
        }
    }


    public void DmgReset(){
        takeDamage = true;
    }

    public void TriggerFromAnim(string trig){
        GetComponent<Animator>().SetTrigger(trig);
    }

    IEnumerator BodyDecay(){
        yield return new WaitForSeconds(decayTimer);
     
    }

    public void DeActivate(){
        gameObject.SetActive(false);
    }

}
