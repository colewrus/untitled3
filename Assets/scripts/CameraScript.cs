using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public Vector3 offset;
    public Transform target;
    public float Speed;
    [Tooltip("Enable if you need to control camera")]
    public bool controlOverride;

	// Use this for initialization
	void Start () {
        controlOverride = false;
	}

    private void LateUpdate()
    {
        if(!controlOverride){
            Vector3 newPosition = target.transform.position + offset;
            transform.position = Vector3.Slerp(transform.position, newPosition, Speed * Time.deltaTime);
        }
      

    }
}
