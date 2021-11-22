using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;
using Yarn.Unity;
using YarnSpinner;

public class CutScenePlayer : MonoBehaviour
{
    public UnityEvent onvideostart;
    public UnityEvent onvideoend;
    public bool active;
    public VideoPlayer toplay;

    private Movement player;

    private InventoryManager im;
    // Start is called before the first frame update
    void Start()
    {
        player = Component.FindObjectOfType<Movement>();
    }
    public void play()
    {
        if(active)
           StartCoroutine(PlayCutscene());
    }
 
    IEnumerator PlayCutscene()
    {
        onvideostart.Invoke();
        toplay.Play();
        //do cutscene stuff
        yield return new WaitForSeconds((float)toplay.length);    
        onvideoend.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        play();
    }

    public void freezeplayer()
    {
        player.canmove = false;
    }

    public void unfreezeplayer()
    {
        player.canmove = true;
        FindObjectOfType<DialogueRunner>().StartDialogue("LightbulbCutscene");
    }

}
