using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class EnableObject : MonoBehaviour
{
    public GameObject[] g;
    [YarnCommand("enableobject")]
    public void enableobject()
    {
        foreach (GameObject go in g)
        {
            go.SetActive(true);
        }
    }
}
