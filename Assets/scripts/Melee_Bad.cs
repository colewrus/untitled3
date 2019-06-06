using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_Bad : MonoBehaviour {

    public float health;

	// Use this for initialization
	void Start () {
		
	}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "hitbox")
        {
            
             health -= collision.GetComponent<Melee_Hitbox>().Damage;           
        }
    }
}
