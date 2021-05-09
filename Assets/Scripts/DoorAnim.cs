using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnim : MonoBehaviour
{
    public Animator a;
    private void OnTriggerEnter(Collider other)
    {
        a.SetBool("Open", true);
    }

    private void OnTriggerExit(Collider other)
    {
        a.SetBool("Open", false);
    }

 
}
