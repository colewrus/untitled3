using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordsman : MonoBehaviour {


    public GameObject target;
    public float moveSpeed;
    public bool playerSeen;
    public bool attack;

    public bool attackStart; //primarily to control the animation of the attack

    public GameObject swordObj;//this is the collider for the sword
    Animator myAnim;
    public float attackDist;

    //health and damage variables
    public float health;
    public bool takeDamage;
    [Tooltip("How long until the body disappears")]
    public float decayTimer;
    [Tooltip("How long until takes damage again")]
    public float dmgReset;
    [Tooltip("How long until sword is recovered")]
    public float swingReset;

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
                hardFreeze = true;
                GetComponent<Animator>().SetTrigger("die");
                StartCoroutine("BodyDecay");
                takeDamage = false;
                return;
            }
            GetComponent<Animator>().SetTrigger("damage");
            takeDamage = false;
        }
    }


    public void DmgReset(){
   
        takeDamage = true;
        attack = false;
        attackStart = false;
    }

    public void TriggerFromAnim(string trig){
        GetComponent<Animator>().SetTrigger(trig);
    }

    IEnumerator BodyDecay(){
        yield return new WaitForSeconds(decayTimer);
     
    }

    public void DeActivate(){

        if (m_PlayerActions.instance.health < 5)
        {
            Debug.Log("Should spawn a heart");
            GameObject h = (GameObject)Instantiate(m_GM.instance.heart, transform.position, Quaternion.identity);
            h.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 1), ForceMode2D.Impulse);
        }

        gameObject.SetActive(false);
    }

    public IEnumerator PauseSwing()
    {
        float animSpeed = GetComponent<Animator>().speed;
        GetComponent<Animator>().speed = 0;
        yield return new WaitForSeconds(swingReset);
        GetComponent<Animator>().speed = animSpeed;
    }

}
