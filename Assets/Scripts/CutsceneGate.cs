using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.WellKnownTypes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using YarnSpinner;

public class CutsceneGate : Interactable
{
    public string varnamechecking = "GotBulb";

    public GameObject yesbulb;

    public GameObject nobulb;
    public UnityEvent onvideostart;
    public UnityEvent onvideoendpass;
    public UnityEvent onvideoendfail;

    private Movement player;
    // Update is called once per frame
    private void Start()
    {
        player = FindObjectOfType<Movement>();
    }

    public override void Interact()
    { 
        Cutscenecheck();
    }

    void Cutscenecheck()
    {
        //does the player have the bulb?
        
        //Yes -> Play cutscene and end demo
        
        //No -> Play other cutsene and boot player out;
        
        if (PlayerPrefs.GetInt(varnamechecking) == 1)
            StartCoroutine(PlayCutscene(yesbulb));
        else
        {
            StartCoroutine( PlayCutscene(nobulb));
        }

    }

    IEnumerator PlayCutscene(GameObject vp)
    {
        vp.gameObject.SetActive(true);
        onvideostart.Invoke();
        vp.GetComponent<VideoPlayer>().Play();
        FindObjectOfType<Music>().entercutscene(.5f,0f);
        //do cutscene stuff
        yield return new WaitForSeconds((float)vp.GetComponent<VideoPlayer>().length);
        if(vp == yesbulb)
            onvideoendpass.Invoke();
        else
            onvideoendfail.Invoke();
        vp.gameObject.SetActive(false);
        FindObjectOfType<Music>().exitcutscene();
    }
    
    public void freezeplayer(bool b)
    {
        player.canmove = b;
    }

    public void deactivateplayer()
    {
        player.gameObject.SetActive(false);
    }

    public void loadscene(int i)
    {
        SceneManager.LoadScene(i);
    }
    
}
