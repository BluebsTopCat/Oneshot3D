using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;

public class VariableEnabler : MonoBehaviour
{
    public GameObject item;
    public string variable;
    public bool enableontrue;

    public InMemoryVariableStorage y;
    
    // Start is called before the first frame update
    void Start()
    {
        y = GameObject.FindObjectOfType<InMemoryVariableStorage>();
    }

    // Update is called once per frame
    void Update()
    {
        if(y.GetValue(variable).AsBool && enableontrue || !enableontrue)
            item.SetActive(true);
        else
            item.SetActive(false);
    }
}
