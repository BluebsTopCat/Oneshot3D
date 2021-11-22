using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.WellKnownTypes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;
using YarnSpinner;

public class CutsceneGate : MonoBehaviour
{
    public string varnamechecking = "GotBulb";

    public VideoPlayer yesbulb;

    public VideoPlayer nobulb;
    public UnityEvent onvideostart;
    public UnityEvent onvideoend;

    private Movement player;
    // Update is called once per frame
    private void Start()
    {
        player = FindObjectOfType<Movement>();
        throw new NotImplementedException();
    }

    private void Update()
    {
        //okay because yarnspinner is a pissbaby sometimes, I'm faking the way interaction works in it
        if(Vector3.Distance(player.gameObject.transform.position, this.transform.position) < player.interactionRadius && player.canmove && Input.GetKeyDown(KeyCode.Space))
            Cutscenecheck();
    }

    void Cutscenecheck()
    {
        //does the player have the bulb?
        
        //Yes -> Play cutscene and end demo
        
        //No -> Play other cutsene and boot player out;
        
        if (PlayerPrefs.GetInt("Gotbulb") == 1)
            PlayCutscene(yesbulb);
        else
        {
            PlayCutscene(nobulb);
        }

    }

    IEnumerator PlayCutscene(VideoPlayer vp)
    {
        onvideostart.Invoke();
        vp.Play();
        //do cutscene stuff
        yield return new WaitForSeconds((float)vp.length);    
        onvideoend.Invoke();
    }
}
