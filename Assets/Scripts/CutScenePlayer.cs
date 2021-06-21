using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;
using YarnSpinner;

public class CutScenePlayer : MonoBehaviour
{
    public UnityEvent onvideostart;
    public UnityEvent onvideoend;
    public bool active;
    public VideoPlayer toplay;

    private Movement player;

    private InventoryManager im;

    public bool destroyonend;
    // Start is called before the first frame update
    void Start()
    {
        player = Component.FindObjectOfType<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        if(destroyonend)
            Destroy(this.gameObject);
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
        
    }

    public void additem(int itemid)
    {
        player.items.Add(itemid);
    }

    public void removeitem(int itemid)
    {
        player.items.Remove(itemid);
    }

    public void Destroy(GameObject g)
    {
        Destroy(g);
    }
}
