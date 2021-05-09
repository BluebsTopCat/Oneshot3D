using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YarnSpinner;

public class Carpetsound : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Movement>())
        {
            Movement g = other.gameObject.GetComponent<Movement>();
            g.footsteps.clip = g.carpet;
            g.footsteps.Play();
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Movement>())
        {
            Movement g = other.gameObject.GetComponent<Movement>();
            g.footsteps.clip = g.wood;
            g.footsteps.Play();
        }
    }
}
