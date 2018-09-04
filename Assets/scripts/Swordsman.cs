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
     
    // Use this for initialization
    void Start () {
        playerSeen = false;
        attack = false;
        myAnim = this.GetComponent<Animator>();
        swordObj.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (playerSeen && !attack)
        {
            //ok but limit the y movement

            //check which side player is on then flip

            if(target.transform.position.x > transform.position.x)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            transform.position += (target.transform.position - transform.position).normalized * moveSpeed * Time.deltaTime;
            Vector3 pos = (target.transform.position - transform.position);

            if(pos.magnitude < attackDist)
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

        swordObj.SetActive(false);
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
            myAnim.ResetTrigger("knightAttack");
            myAnim.ResetTrigger("knightWindUp");

        }
    }

    public void SetTrigger(string TriggerID)
    {
        myAnim.SetTrigger(TriggerID);
    }

   public void AttackEnd()
    {
        attack = false;
        attackStart = false;
    }


}
