using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordsman : MonoBehaviour {


    public GameObject target;
    public float moveSpeed;
    bool playerSeen;
    bool attack;

    bool attackStart; //primarily to control the animation of the attack

    public GameObject swordObj;//this is the collider for the sword

     
    // Use this for initialization
    void Start () {
        playerSeen = false;
        attack = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (playerSeen && !attack)
        {
            transform.position += (target.transform.position - transform.position).normalized * moveSpeed * Time.deltaTime;
            Vector3 pos = (target.transform.position - transform.position);

            if(pos.magnitude < 2)
            {
                attack = true;
            }

            //run the baddie attack
            if (attack)
            {
                //Start animation and run through animation events?
                if (!attackStart)
                {
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

    public void SwordActive()
    {
        Debug.Log("sweet");
    }


}
