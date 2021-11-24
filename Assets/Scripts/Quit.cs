using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Quit : MonoBehaviour
{
    void Awake()
    {
        QuitWipe();
    }

    public void QuitWipe()
    {
        File.Delete(Application.dataPath + "/DialogueVars.json");
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }
}
