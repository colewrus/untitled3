using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileBehavior : MonoBehaviour {


    public projectile myType;






    private void OnEnable()
    {
        if(myType != null)
            StartCoroutine("Decay");
    }
    void InitType()
    {

    }

    // Use this for initialization
    void Start () {
		
	}

    IEnumerator Decay()
    {
        yield return new WaitForSeconds(myType.lifetime);
        this.gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       /*Some stuff to make fun interactions when it hits a wall, etc.
        * 
        * 
        if(collision.transform.tag == "floor")
        {
            foreach(ContactPoint2D c in collision.contacts)
            {
                Debug.Log(c.point);
            }
        }
        */
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
    }
}
