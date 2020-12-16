using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public GameObject window;
    public void inventory()
    {
        
    }

    public void settings()
    {
        
    }

    public void fasttravel()
    {
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            window.gameObject.SetActive(!window.gameObject.active);
    }
}
