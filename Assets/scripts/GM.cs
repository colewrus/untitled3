using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[System.Serializable]
public class Sound
{

    public string name;

    public AudioClip clip;
    [Range(0f,1f)]
    public float volume;
    [Range(0f,1f)]
    public float pitch;

    [HideInInspector]
    public AudioSource source;
    public bool isPlaying;
    [Tooltip("Is this looped for music or not for SFX")]
    public bool loop;
}


public class GM : MonoBehaviour {

    public static GM instance = null;

 
    public AudioSource mainSource;

    public Sound[] sounds;

    public List<AudioClip> clips = new List<AudioClip>();
    public List<AudioClip> fxClips = new List<AudioClip>();


    public List<GameObject> BulletPool = new List<GameObject>();
    public GameObject bullet;
    public int poolCount;


    private void Awake()
    {
        instance = this;
        mainSource = this.GetComponent<AudioSource>();
        mainSource.clip = clips[0];
      // mainSource.Play();
        mainSource.loop = true;

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            if (s.loop)
                s.source.loop = true;
        }
    }


    // Use this for initialization
    void Start()
    {
        //pool our bullets
        for (int i = 0; i < poolCount; i++)
        {
            GameObject obj = (GameObject)Instantiate(bullet);
            obj.SetActive(false);
            BulletPool.Add(obj);
        }

        PlaySound("Main");
    }

    public void PlaySound(string name)
    {
  
        Sound s = Array.Find(sounds, sound => sound.name == name);
       
        s.source.Play();
        s.isPlaying = true;
                
        return;
    }

    public void PlaySound(string name, bool stop)
    {
        if (stop)
        {
            foreach (Sound y in sounds)
            {

                if (y.isPlaying && y.loop)
                {
                    StartCoroutine(FadeOut(y.source, 0.9f));
                   //y.source.Stop();
                    y.isPlaying = false;
                }

            }
        }
  
        Sound s = Array.Find(sounds, sound => sound.name == name);
        //s.source.Play();
        StartCoroutine(FadeIn(s.source, 0.9f));
            
        s.isPlaying = true;
        return;
    }


    public IEnumerator FadeOut(AudioSource source, float fadeTime)
    {
        float startVolume = source.volume;
    
        while(source.volume > 0)
        {
            source.volume -= startVolume * Time.deltaTime / fadeTime;
          
            yield return null;
        }
        source.Stop();

    }


    public IEnumerator FadeIn(AudioSource source, float fadeTime)
    {
        float startVolume = 0.1f;
        source.Play();
        while (source.volume < 1)
        {
            source.volume += startVolume * Time.deltaTime / fadeTime;
            
            yield return null;
        }
    }

    private void Update()
    {
     

    }

    public GameObject GetBullets()
    {
        for(int i= 0; i < BulletPool.Count; i++)
        {
            if (!BulletPool[i].activeInHierarchy)
            {
                BulletPool[i].transform.localScale = Vector3.one;
                return BulletPool[i];
            }
        }
        return null;
    }

   
}
