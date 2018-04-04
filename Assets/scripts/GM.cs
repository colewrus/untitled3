using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour {


    public float timer;
    float tick;

	// Use this for initialization
	void Start () {
        timer = 0;

	}
	
	// Update is called once per frame
	void Update () {
        Watch();
	}

    void Watch()
    {
        if(tick < timer)
        {
            tick += 1 * Time.deltaTime;
        }
        else //tock
        {

        }
    }
}
