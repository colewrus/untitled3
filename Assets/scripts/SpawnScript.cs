using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveBuilder
{
    public float id;
    public List<GameObject> waveObj = new List<GameObject>();
        
}

public class SpawnScript : MonoBehaviour {


    public List<GameObject> myObj = new List<GameObject>();
    public List<WaveBuilder> myWaves = new List<WaveBuilder>();
    public Vector3 destPos;
    public float timer;
    public float tick;
    public bool activeSpawn;

    GameObject tempSpawn;
	// Use this for initialization
	void Start () {
        tick = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (activeSpawn)
        {
            if(tick < timer)
            {
                tick += 1 * Time.deltaTime;
            }else
            {
                Spawn();
                tick = 0;
            }
        }
	}


    void Spawn()
    {
        tempSpawn = (GameObject)Instantiate(myObj[1], transform.position, Quaternion.identity);
        tempSpawn.GetComponent<BaddieScript>().destPos = destPos;
    }
}
