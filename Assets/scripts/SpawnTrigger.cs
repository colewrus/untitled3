using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour {

    FallScript parentScript;

	// Use this for initialization
	void Start () {
        parentScript = this.transform.GetComponentInParent<FallScript>();
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            parentScript.spawnPosition++;
            this.gameObject.SetActive(false);
        }
    }

}
