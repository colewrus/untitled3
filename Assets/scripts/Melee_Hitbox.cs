using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_Hitbox : MonoBehaviour
{

    public float Damage;
    public bool armed; //can it do damage?
    public float swingDeltaY;

    // Use this for initialization

    private void Awake()
    {
        armed = true;
    }

    private void OnEnable()
    {
        armed = true;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (armed)
        {
            if (collision.tag == "bat")
            {
                collision.transform.parent.GetComponent<BatScript>().HitReg(Damage);
                armed = false;
            }

            if (collision.tag == "swordman")
            {
                collision.transform.parent.GetComponent<Swordsman>().HitReg(Damage);
                armed = false;
                Debug.Log("hit swordsman");
            }

            if (collision.tag == "boss")
            {
                collision.GetComponent<Boss_Script>().ReceiveDamage(Damage);
                armed = false;
            }
           
        }

    }


 
}
