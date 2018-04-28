using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveBuilder
{
    public Transform Target;
    public List<GameObject> waveObj = new List<GameObject>();
    public BoxCollider2D waveZone;
    GameObject temp;
    public void WaveBuilderSpawn()
    {
        //instantiate game objects at random point in the collider
        //assign collider to enemy script
        
        Vector3 randoV = new Vector3(Random.Range(waveZone.bounds.min.x, waveZone.bounds.max.x), Random.Range(waveZone.bounds.min.y, waveZone.bounds.max.y), 0);
        for(int i=0; i< waveObj.Count; i++)
        {
            temp = GameObject.Instantiate(waveObj[i], randoV, Quaternion.identity) as GameObject;
            temp.GetComponent<BaddieScript>().moveZone = waveZone;
     ;

            GM.instance.AddEnemy();
            PlayerScript.instance.enemyCollider.Add(temp.GetComponent<BoxCollider2D>());
        }
    }           
}

public class SpawnScript : MonoBehaviour {

    public int waveCounter;
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
        waveCounter = 0;
        for(int i=0; i < myWaves.Count; i++)
        {
            myWaves[i].waveZone = transform.parent.GetComponent<BoxCollider2D>();
        }
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
               // Spawn();
                SpawnWave();
                tick = 0;
            }
        }
	}


    void Spawn()
    {
        tempSpawn = (GameObject)Instantiate(myObj[1], transform.position, Quaternion.identity);
        //tempSpawn.GetComponent<BaddieScript>().destPos = destPos;
    }


    void SpawnWave()
    {
        
        if(waveCounter < myWaves.Count)
        {
            myWaves[waveCounter].WaveBuilderSpawn();
            waveCounter++;
        }        
    }
}
