using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_Hitbox : MonoBehaviour
{

    public float Damage;
    public bool armed; //can it do damage?
    public float swingDeltaY; //how far down does it swing?

    // Use this for initialization

    private void Awake()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "bat"){
            collision.transform.parent.GetComponent<BatScript>().HitReg(Damage);
        }

        if(collision.tag == "swordman"){
            collision.transform.parent.GetComponent<Swordsman>().HitReg(Damage);
            Debug.Log("hit swordsman");
        }

        if(collision.tag == "boss"){
            collision.GetComponent<Boss_Script>().ReceiveDamage(Damage);   
        }

    }
    
}
