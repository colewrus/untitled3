using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GM : MonoBehaviour {

    public static GM instance = null;
    public int enemyCount;
    public Text t_enemyCount;

    public float timer;
    float tick;

    private void Awake()
    {
        instance = this;
    }


    // Use this for initialization
    void Start () {
        timer = 0;
        enemyCount = GameObject.FindGameObjectsWithTag("enemies").Length;

        t_enemyCount.text = "x" + enemyCount;
    }
	
	// Update is called once per frame
	void Update () {
        
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

    public void AddEnemy()
    {
        enemyCount++;
        t_enemyCount.text = "x" + enemyCount;
    }

    public void RemoveEnemy()
    {
        enemyCount--;
        t_enemyCount.text = "x" + enemyCount;
    }
}
