using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemLib : MonoBehaviour
{

    public List<Inventory> items = new List<Inventory>(1);
    public Inventory checkitem(int i)
    {
        return items[i];
    }
    
}
