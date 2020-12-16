using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yarn.Unity;
using Yarn.Unity.Example;
using YarnSpinner;

public class GiveItem: MonoBehaviour
{
    public Movement player;
    [YarnCommand("give")]
    public void Give(string itemid)
    {
        Debug.Log("Gave item " + itemid);
        player.items.Add(int.Parse(itemid));
    }
    
}