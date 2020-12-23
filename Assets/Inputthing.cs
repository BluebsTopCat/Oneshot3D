using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;
using Random = UnityEngine.Random;

public class Inputthing : MonoBehaviour
{
    public EventSystem e;
    public TMP_InputField[] inputs;
    public string nums;
    public string correctpassword;
    private bool donebefore = false;
    public InMemoryVariableStorage vars;
    void Start()
    {
        int red = Random.Range(0, 10);
        int green = Random.Range(0, 10);
        int blue = Random.Range(0, 10);
        int yellow = Random.Range(0, 10);
        correctpassword = red.ToString() + green.ToString() + blue.ToString()  + yellow.ToString();

        vars.defaultVariables[0].value = red.ToString();
        vars.defaultVariables[1].value = green.ToString();
        vars.defaultVariables[2].value = blue.ToString();
        vars.defaultVariables[3].value = yellow.ToString();
        
    }
    
    public void highlight(GameObject G)
    {
        e.SetSelectedGameObject(G);
    }

    public void dothething()
    {
        if(!donebefore)
          StartCoroutine(check());
        donebefore = !donebefore;
    }
    public IEnumerator check()
    {
        yield return new WaitForSeconds(.01f);
        nums = null;
        for (int i = 0; i < inputs.Length; i++)
        {
            nums += inputs[i].text;
            inputs[i].text = null;
        }

        if (nums == correctpassword) 
            Debug.Log("Right Password!");
        else
            Debug.Log("Wrong Password :(");

    }
}
