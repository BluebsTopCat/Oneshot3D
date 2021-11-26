using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using YarnSpinner;

public class NewCommandLib : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public Movement player;
    public AudioSource getitem;
    // Start is called before the first frame update
    
    public void Awake() {
        
        dialogueRunner.AddCommandHandler("SetVar", setvar);
        dialogueRunner.AddCommandHandler("Take", take);
        dialogueRunner.AddCommandHandler("Give", Give);
        dialogueRunner.AddCommandHandler("Destroy", Destroy);
    }

    public void setvar(string[] parameters)
    {
        string name = parameters[0];
        string val = parameters[1];
        bool b;
        float f;
        if (float.TryParse(val, out f))
            PlayerPrefs.SetFloat(name, f);
        else if (bool.TryParse(val, out b))
            PlayerPrefs.SetInt(name, toint(b));
    }
    
    public void take(string[] parameters)
    {
        string itemid = parameters[0];
        Debug.Log("Removed item " + itemid);
        player.items.Remove(int.Parse(itemid));
        if (player.activeitem == int.Parse(itemid))
            player.activeitem = -1;
        player.saveinventory();
    }

    public void Give(string[] parameters)
    {
        string itemid = parameters[0];
        Debug.Log("Gave item " + itemid);
        player.items.Add(int.Parse(itemid));
        player.saveinventory();
        getitem.Play();
    }

    public void Destroy(string[] parameters)
    {
        string name = parameters[0];
        if(GameObject.Find(name))
            Destroy(GameObject.Find(name));
        Debug.Log("Destroyed " + name);
    }
    private static int toint(bool b)
    {
        switch (b)
        {
            case true:
                return 1;
            
            case false:
                return 0;
             
        }
    }
}
