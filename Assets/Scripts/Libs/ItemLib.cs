using System.Collections.Generic;
using UnityEngine;

public class ItemLib : MonoBehaviour
{
    public List<Inventory> items = new List<Inventory>();

    public Inventory checkitem(int i)
    {
        return items[i];
    }
}