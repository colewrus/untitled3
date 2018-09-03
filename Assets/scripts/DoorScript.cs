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
    public bool bossFight;
    public bool BossFightLocked; //is the door locked to keep the player in for the boss fight?
    public GameObject boss; //who is the boss that is about to get activated?
    [Tooltip("where do you want to move the player after opening the boss door")]
    public Transform bossSetup;

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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player"){
            if(LockDoor && boss == null){ //you just need a key to open this and go to the next room
                for (int i = 0; i < PlayerScript.instance.keys.Count; i++)
                {
                    if (PlayerScript.instance.keys[i] == key)
                    {
                        IEnumerator tempCo = PlayerScript.instance._DoorTransfer(this.gameObject);
                        PlayerScript.instance.StartCoroutine(tempCo);
                    }
                }
            }

            if(boss != null){
                for (int i = 0; i < PlayerScript.instance.keys.Count; i++){
                    if(PlayerScript.instance.keys[i] == key){
                        OpenBossDoor();
                        PlayerScript.instance.moveLock = true;
                        //override camera controls
                        Camera.main.GetComponent<CameraScript>().controlOverride = true;
                        PlayerScript.instance.lookAtBoss = true;
                        PlayerScript.instance.door = this.gameObject;

                        if (boss.name == "batBoss2")
                        {
                            BossFightLocked = true;
                            PlayerScript.instance.tempBoss = boss;

                            boss.GetComponent<Boss_Script>()._BatState = BatState.intro;

                            PlayerScript.instance.StartCoroutine("ReturnFromBossIntro");
                        }
                    }
                }
            }
        }
    }
}
