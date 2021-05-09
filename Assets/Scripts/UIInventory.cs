using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YarnSpinner;

public class UIInventory : MonoBehaviour
{
    public GameObject invprefab;
    public Movement player;
    [CanBeNull] public List<int> items;
    public TextMeshProUGUI name;
    public TextMeshProUGUI desc;
    public ItemLib itlb;

    public List<GameObject> buttons;

    // Start is called before the first frame update
    private void Start()
    {
        items = player.items;
        foreach (var i in items)
        {
            var g = Instantiate(invprefab, transform, true);
            g.transform.GetChild(0).GetComponent<Image>().sprite = itlb.items[i].icon;
            g.name = i.ToString();
            buttons.Add(g);
        }
    }
}