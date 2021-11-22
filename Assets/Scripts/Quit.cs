using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Quit : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        File.Delete(Application.dataPath + "/DialogueVars.json");
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }
}
