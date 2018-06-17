using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



[System.Serializable]
public class StoryElement
{

    public string message;
    public bool ActionRequired;
    public PlacedSpawn mySpawn;



}


public enum Chapter { intro, chapter1, chapter2, chapter3, chapter4, finale };



public class GM : MonoBehaviour {

    public static GM instance = null;

    public Chapter myChapter;

    public List<StoryElement> myStory = new List<StoryElement>();
    public GameObject textPanel;
    public Text storyText;
    public int storyPos;

    public GameObject namePanel;
    public InputField nameInput;

    string playerName;

    public int enemyCount;
    public Text t_enemyCount;

    public AudioSource mainSource;

    public List<AudioClip> clips = new List<AudioClip>();
    public List<AudioClip> fxClips = new List<AudioClip>();

    public float timer;
    float tick;

    public List<GameObject> Spawners = new List<GameObject>();

    public List<GameObject> BulletPool = new List<GameObject>();
    public GameObject bullet;
    public int poolCount;


    public Text WaveAnnouncer;
    public int WaveCount;
    public Image Wave_Announce_BKG;

    public GameObject fadescreen;
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
        myChapter = Chapter.intro;

        WaveCount = 0;
        WaveAnnouncer.gameObject.SetActive(false);
        Wave_Announce_BKG.gameObject.SetActive(false);
        timer = 0;

        enemyCount = GameObject.FindGameObjectsWithTag("enemies").Length - 1;
        t_enemyCount.text = "x" + enemyCount;

        //pool our bullets
        for(int i=0; i<poolCount; i++)
        {
            GameObject obj = (GameObject)Instantiate(bullet);
            obj.SetActive(false);
            BulletPool.Add(obj);
        }

        namePanel.SetActive(false);
        //PopulateText();
       // AnnounceWave();
    }
	

    void PopulateText(){
        PlayerScript.instance.fireLock = true;
        textPanel.SetActive(true);
        storyText.text = myStory[storyPos].message;
    }

    public void FadeToBlack(float t)
    {
        fadescreen.GetComponent<Image>().CrossFadeAlpha(1, t, false);
    }

    public void Listen(){
        Debug.Log(storyPos);
        if(storyPos == 0){
            PlayerScript.instance.fireLock = true;
            textPanel.SetActive(false);
            namePanel.SetActive(true);
        }

        if(storyPos == 1){

            storyPos++;
            PopulateText();
            return;
        }

        if(storyPos == 2){
            
            storyPos++;
            PopulateText();
            PlayerScript.instance.fireLock = false;
            return;
        }
        if (storyPos == 3)
        {
            textPanel.SetActive(false);
            //spawn a blob and pacify it
            storyPos++;

        }


    }


    IEnumerator DelayFunction(float t){
        yield return new WaitForSeconds(t);
        PopulateText();
    }

    public void SubmitName(){
        playerName = nameInput.text;
        storyPos = 1;
        namePanel.SetActive(false);
        Debug.Log(playerName);
        IEnumerator tempCoroutine = DelayFunction(1.5f);
        StartCoroutine(tempCoroutine);

    }

    public void IntroRun(){
        /*see that blob, he's a problem blob, he don't see you just yet but when you does you're gonna need to put him in the dirt
         *Give Control
         *Player dies to blob: wow, now that's a bit embarassing, here let me help you with that
         *Player shoots blob:
         *

        */
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

    public void AnnounceWave(){
        //start the fade in coroutine
        //run an animation?
        WaveCount++;

        WaveAnnouncer.gameObject.SetActive(true);
        Wave_Announce_BKG.gameObject.SetActive(true);
        WaveAnnouncer.text = "Wave " + WaveCount;
        WaveAnnouncer.GetComponent<Animator>().Play("waveAnnouncer");
        Wave_Announce_BKG.GetComponent<Animator>().Play("waveAnnounceBKG");
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
