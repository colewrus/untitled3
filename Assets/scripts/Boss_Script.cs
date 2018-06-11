using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BossType { bat, ghost, golem};

public enum BatState { search, intro, attack4, attack8, summon}

public class Boss_Script : MonoBehaviour {

    public BossType thisBoss;
    public BatState _BatState;

    public float timer; //how long between actions

    public bool battleMode; //has the player triggered the boss fight
    public float health;

    public GameObject bossCanvas;
    public Slider healthBar;
    public Text healthBarText;
    public string bossName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void BatBehavior()
    {

    }


    public void InitCanvas()
    {
        bossCanvas.SetActive(true);
        healthBar.maxValue = health;
        healthBar.value = health;
        healthBarText.text = bossName;
    }

    public void ReceiveDamage(float d) 
    {
        health -= d;
        healthBar.value = health;
    }

}
