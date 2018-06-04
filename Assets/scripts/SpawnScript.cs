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
     

            GM.instance.AddEnemy();
            GM.instance.mainSource.PlayOneShot(GM.instance.fxClips[0]);
            PlayerScript.instance.enemyCollider.Add(temp.GetComponent<BoxCollider2D>());
        }
    }           

  
}

[System.Serializable]
public class PlacedSpawn
{
    public List<GameObject> spawnPlace = new List<GameObject>();
    public List<GameObject> spawnObj = new List<GameObject>();
    GameObject temp;
    public BoxCollider2D waveZone;
    public bool activateGate; //does this wave activate a gate when it's killed?
    public GameObject gate;

    public float waveCount; //the number of enemies currently alive in wave. used to determine if a wave is finished

    public void SpawnWave(GameObject parentObj)
    {
       
        for (int i = 0; i < spawnObj.Count; i++)
        {
            
            temp = GameObject.Instantiate(spawnObj[i], spawnPlace[i].transform.position, Quaternion.identity) as GameObject;
            temp.GetComponent<BaddieScript>().moveZone = waveZone;
            //init baddie
            GM.instance.AddEnemy();
            GM.instance.mainSource.PlayOneShot(GM.instance.fxClips[0]);
            PlayerScript.instance.enemyCollider.Add(temp.GetComponent<BoxCollider2D>());
            waveCount++;
            if(activateGate)
                temp.GetComponent<BaddieScript>().ParentObject = parentObj;
        }
    }

    public void EnemyKilled()
    {
        Debug.Log("holy shit");
        waveCount--;
        if(waveCount <= 0)
        {
            gate.GetComponent<DoorScript>().activeDoor = true;
        }
    }
}
public class SpawnScript : MonoBehaviour {

    public List<PlacedSpawn> p_Waves = new List<PlacedSpawn>();
    public int waveCounter;

    public float timer;
    public float tick;
    public bool activeSpawn;

   

    GameObject tempSpawn;
	// Use this for initialization
	void Start () {
        tick = 0;
        waveCounter = 0;

	}
	

    IEnumerator StartSpawn()
    {
        yield return new WaitForSeconds(1f);
        GM.instance.AnnounceWave();
        yield return new WaitForSeconds(timer);
        Spawn();
    }

    public void Spawn()
    {
        if(waveCounter < p_Waves.Count)
        {
            p_Waves[waveCounter].SpawnWave(this.gameObject);
            waveCounter++;
        }
    }

}
