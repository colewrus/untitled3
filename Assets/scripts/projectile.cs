using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile_")]
public class projectile : ScriptableObject {

    public string name;

    public GameObject prefab;
    public float damage;
    public float range;    
    public float lifetime;
    public float speed;
    public float chargeTime; //how long to get the maximum power out of this

    public AudioClip travelSound;
    public AudioClip contactSound;

}
