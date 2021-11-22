using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterLib : MonoBehaviour
{
    public List<DialogueCharacter> character;
    public RawImage left;
    public TMP_Text text;
    public Sprite getface(string name, string state)
    {
        return (character.Find((x) => x.Name == name).Faces.Find((y) => y.name == state).image);
    }

    public void Displaychar(string s)
    {
        string CharacterInfo;
        if (s == "")
        {
            left.enabled = false;
            return;
        }
            try
        {
            string name = s.Split('-')[0];
            string status = s.Split('-')[1];

            left.enabled = true;
            left.texture = getface(name, status).texture;
        }
        catch (Exception e)
        {
            left.enabled = false;
            Console.WriteLine(e); throw;
        }
        
    }

    public void emptychar()
    {
        left.enabled = false;
        left.texture = null;
        text.text = "";
    }

}
