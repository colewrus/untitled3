using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



[System.Serializable]
public class EnemyTypes
{
    public string EnemyName;
    public int damage;
}


public class m_PlayerActions : MonoBehaviour {


    public static m_PlayerActions instance = null;
    //Health stuff
    public int health;
    float maxHealth;
    public EnemyTypes[] EnemyList;
    public GameObject[] hearts;
    [Tooltip("0 is half heart, 1 is full heart")]
    public Sprite[] heartSprites; //0 is half heart, 1 is full heart;

    //Attack Variables
    RaycastHit hit;
    RaycastHit2D hit2d;
    

    //Money
    public int coins;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        maxHealth = health;
	}
	
	// Update is called once per frame
	void Update () {
		
	}



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "enemyWeapon")
        {

            for(int i=0; i < EnemyList.Length; i++)
            {
                if(EnemyList[i].EnemyName == collision.transform.parent.name)
                {
                    PlayerTakeDamage(EnemyList[i].damage);
                }
            }
        
        }

        if(collision.transform.tag == "coin")
        {
            coins++;
            collision.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "heart") {
            if(health < maxHealth)
            {
                // !!!! need to handle health going over max
                health++;
                collision.gameObject.SetActive(false);
                SetHeartUI();
            }
            else
            {

            }
          
        }

    }

    void RaycastAttack()
    {

    }

    void PlayerTakeDamage(int amount)
    {
        health -= amount;
        //update UI
        SetHeartUI();
    }

    public void SetHeartUI()
    {
        if (health % 2 == 0)
        {
            hearts[health / 2].SetActive(false);
        }
        else
        {
            hearts[health / 2].GetComponent<Image>().sprite = heartSprites[0];
        }
    }

}
