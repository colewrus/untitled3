using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_Hitbox : MonoBehaviour
{

    public float Damage;
    public bool armed; //can it do damage?
    public float swingDeltaY; //how far down does it swing?
    public List<AudioClip> swingClips = new List<AudioClip>();
    AudioSource mySource;
    // Use this for initialization

    private void Awake()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "bat"){
            Debug.Log("sword collided");

            collision.transform.parent.GetComponent<BatScript>().HitReg(Damage);
        }
    }
}
