using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleportScript : MonoBehaviour {


    public Vector3 dest;

    GameObject obi;

	// Use this for initialization
	void Start () {
		
	}




    public void StartTP(){

        GetComponent<Animator>().SetTrigger("teleport");

    }



    public void TPDest(Vector3 d){
        dest = d;

    }

    IEnumerator TP(){
        yield return new WaitForSeconds(5);
        transform.parent.transform.position = dest;
        
    }
}
