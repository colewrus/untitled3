using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    public Transform dest;
    public bool activeDoor;
    public Transform bulletZoneReset; //moves the bullet zone for the pooled bullets 

    public GameObject doorBarrier;
    public bool boolBarrier; //does this door have a barrier in front of it?

    
    public bool LockDoor; //does it open with a key
    public GameObject key;
    public bool BossFightLocked; //is the door locked to keep the player in for the boss fight?

	// Use this for initialization
	void Start () {
        BossFightLocked = false;
	}
	
    public void RemoveBarrier()
    {
        doorBarrier.SetActive(false);
    }

    public void OpenBossDoor()
    {
        foreach(Transform child in transform)
        {            
            child.gameObject.SetActive(false);
        }
    }

    public void CloseBossDoor()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
