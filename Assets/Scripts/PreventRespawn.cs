using System.Collections.Generic;
using UnityEngine;

public class PreventRespawn : MonoBehaviour
{
    public List<spawnpair> s = new List<spawnpair>();

    // Start is called before the first frame update
    private void Start()
    {
        foreach (spawnpair sp in s)
        {
            if (PlayerPrefs.GetInt(sp.variable) == 1)
            {
                if(sp.turnon)
                    sp.item.SetActive(true);
                else sp.item.SetActive(false);
            }
        }

    }
}


[System.Serializable]
public class spawnpair
{
    public GameObject item;
    public string variable;
    public bool turnon;

    public spawnpair(GameObject g, string v, bool t)
    {
        item = g;
        variable = v;
        turnon = t;
    }
}