using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using UnityEngine;
using Yarn.Unity;
using Newtonsoft.Json.Converters;
using Yarn;

public class SaveDialogue : MonoBehaviour
{
    public InMemoryVariableStorage imvs;
    public float currenttime = 19;
    public bool loadvarsonstart = true;
    public bool savevarsonquit = true;
    void Start()
    {
        if (loadvarsonstart)
            StartCoroutine(loadvars());
    }

    private void OnApplicationQuit()
    {
        if(savevarsonquit)
          Savevars();
    }

    private void Update()
    {
        currenttime += Time.deltaTime;

        if (currenttime >= 20 && savevarsonquit)
        {
            Savevars();
            currenttime = 0;
        }

        
    }

    public void Savevars()
    {
        foreach (KeyValuePair<string, Yarn.Value> s in imvs.variables)
        {
            Debug.Log(s);
        }
        File.WriteAllText(Application.dataPath + "/DialogueVars.json", JsonConvert.SerializeObject(imvs.variables));
    }

    public IEnumerator loadvars()
    { 
     yield return new WaitForSeconds(.05f);
     Debug.Log(File.ReadAllText(Application.dataPath + "/DialogueVars.json"));
     Dictionary<string, yarnimitator> output = JsonConvert.DeserializeObject<Dictionary<string, yarnimitator>>(File.ReadAllText(Application.dataPath + "/DialogueVars.json"));
     
     foreach (KeyValuePair<string, yarnimitator> s in output)
     {
         Debug.Log(s.Key + ": Type=" + s.Value.type + " asbool=" + s.Value.asBool + " asnumber=" + s.Value.asNumber + " asstring=" + s.Value.asString);

         switch (s.Value.type)
         {
             case Value.Type.Bool:
                 imvs.SetValue(s.Key, s.Value.asBool);
                 break;
             case Value.Type.Number:
                 imvs.SetValue(s.Key,s.Value.asNumber);
                 break;
             case Value.Type.String:
                 imvs.SetValue(s.Key,s.Value.asString);
                 break;
             default:
                 Debug.Log("No Valid Type for " + s.Key + "!");
                 break;
         }
         
     } 
     //imvs.variables = output;    

    }
}

[System.Serializable]
public class yarnimitator
{
    public Value.Type type;
    public float asNumber;
    public string asString;
    public bool asBool;
    public yarnimitator(Value.Type Type, float AsNumber, string AsString, bool AsBool)
    {
        type = Type;
        asNumber = AsNumber;
        asString = AsString;
        asBool = AsBool;
    }
}
