using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossType { bat, ghost, golem};

public enum BatState { search, attack4, attack8, summon}

public class Boss_Script : MonoBehaviour {

    public BossType thisBoss;

    public float timer; //how long between actions

    public bool battleMode; //has the player triggered the boss fight
    public float health;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void BatBehavior()
    {

    }

}
