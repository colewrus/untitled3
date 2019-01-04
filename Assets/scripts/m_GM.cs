using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class m_GM : MonoBehaviour {

    public static m_GM instance = null;
    public GameObject heart;


	// Use this for initialization
	void Start () {
        instance = this;
	}
	

}
