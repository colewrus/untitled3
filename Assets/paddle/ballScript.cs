using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballScript : MonoBehaviour {

    public float speed = 10;
    public Vector3 restartPos;


	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody2D>().velocity = Vector2.up * speed;
	}



    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.name == "Paddle")
        {
            float x = hitFactor(transform.position, col.transform.position, col.collider.bounds.size.x);
            Vector2 dir = new Vector2(x, 1).normalized;
            GetComponent<Rigidbody2D>().velocity = dir * speed;
        }
    }

    float hitFactor(Vector2 ballPos, Vector2 paddlePos, float paddleWidth)
    {
        return (ballPos.x - paddlePos.x) / paddleWidth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "killZone")
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            transform.position = restartPos;
            StartCoroutine("Reset");
        }
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(2);
        GetComponent<Rigidbody2D>().velocity = Vector2.up * speed;
    }
}
