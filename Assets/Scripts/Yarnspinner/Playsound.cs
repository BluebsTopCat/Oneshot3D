using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yarn.Unity;

public class Playsound : MonoBehaviour
{
    public DialogueRunner d;

    public AudioSource p;
    public List<SoundThing> soundthinglist;


    [YarnCommand("playsound")]
    public void playsound(string name)
    {
        Debug.Log("Playing Sound");
        AudioClip s = soundthinglist.Where(obj => obj.name == name).SingleOrDefault()?.clip;
        p.clip = s;
        p.Play();
    }
}

[System.Serializable]
public class SoundThing {
    public string name;
    public AudioClip clip;

    public SoundThing(string s, AudioClip ac)
    {
        name = s;
        clip = ac;
    }
}