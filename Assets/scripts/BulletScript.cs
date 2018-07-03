using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    public Vector2 target;
    public float speed;
    public float lifetime;
    Rigidbody2D rb;

	// Use this for initialization
	void Start () {
    
	}
    /*
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = target.normalized * speed;
    }
    */

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "bulletZone")
        {
            gameObject.active = false;
        }
        //transform.localScale = Vector3.one;            
    }

}
