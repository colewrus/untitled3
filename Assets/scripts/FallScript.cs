using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallScript : MonoBehaviour {


    public Transform[] spawnPoints;
    public int spawnPosition;

	// Use this for initialization
	void Start () {
        spawnPosition = 0;
	}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            collision.transform.position = spawnPoints[spawnPosition].position;
        }
    }

}
