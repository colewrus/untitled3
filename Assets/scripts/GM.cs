using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GM : MonoBehaviour {

    public static GM instance = null;
    public int enemyCount;
    public Text t_enemyCount;

    AudioSource mainSource;

    public List<AudioClip> clips = new List<AudioClip>();

    public float timer;
    float tick;

    public List<GameObject> Spawners = new List<GameObject>();

    public List<GameObject> BulletPool = new List<GameObject>();
    public GameObject bullet;
    public int poolCount;

    private void Awake()
    {
        instance = this;
        mainSource = this.GetComponent<AudioSource>();
        mainSource.clip = clips[0];
        mainSource.Play();
        mainSource.loop = true;
    }


    // Use this for initialization
    void Start () {
        timer = 0;
        enemyCount = GameObject.FindGameObjectsWithTag("enemies").Length;

        t_enemyCount.text = "x" + enemyCount;

        for(int i=0; i<poolCount; i++)
        {
            GameObject obj = (GameObject)Instantiate(bullet);
            obj.SetActive(false);
            BulletPool.Add(obj);
        }
    }
	
    public GameObject GetBullets()
    {
        for(int i= 0; i < BulletPool.Count; i++)
        {
            if (!BulletPool[i].activeInHierarchy)
            {
                return BulletPool[i];
            }
        }
        return null;
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
        return;
    }

    public void RemoveEnemy()
    {
        enemyCount--;
        t_enemyCount.text = "x" + enemyCount;
    }
}
