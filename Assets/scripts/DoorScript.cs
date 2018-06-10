using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    public Transform dest;
    public bool activeDoor;
    public Transform bulletZoneReset; //moves the bullet zone for the pooled bullets 

    public GameObject doorBarrier;
    public bool boolBarrier;

	// Use this for initialization
	void Start () {
		
	}
	
    public void RemoveBarrier()
    {
        doorBarrier.SetActive(false);
    }
}
