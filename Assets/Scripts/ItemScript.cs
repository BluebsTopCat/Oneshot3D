using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity.Example;
using YarnSpinner;

public class ItemScript : MonoBehaviour
{
    public Movement player;
    public void clicked()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        if (this.gameObject.name == "Unequip")
        {
            player.activeitem = -1;
            return;
        }
        player.equipiten(int.Parse(this.name));
    }
}
