using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundGM : MonoBehaviour {

    public AudioSource mySource;


	// Use this for initialization
	void Start () {
        mySource = this.GetComponent<AudioSource>();
	}	



    public void playSound(AudioClip clip)
    {
        mySource.clip = clip;
        mySource.Play();
    }
}
