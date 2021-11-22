using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueCharacter
{
    [Header("Info")]
    public string Name;
    public bool Left;
    public AudioClip[] Sounds;
    public List<Face> Faces;
    public DialogueCharacter(string n, bool l, AudioClip[] s, List<Face> f)
    {
        Name = n;
        Left = l;
        Faces = f;
        Sounds = s;
    }
}

[System.Serializable]
public class Face
{
    public string name;
    public Sprite image;
    public Face(string n, Sprite i)
    {
        name = n;
        image = i;
    }
}

