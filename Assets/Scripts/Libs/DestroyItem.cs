using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yarn.Unity;
using Yarn.Unity.Example;

public class DestroyItem: MonoBehaviour
{
 
    [YarnCommand("destroy")]
    public void Destroy(string playername){
        Destroy(GameObject.Find(playername).gameObject);
        Debug.Log("Destroyed " + playername);
    }
}
