using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YarnSpinner;

public class InventoryManager : MonoBehaviour
{
    public List<int> items = new List<int>();
    public List<GameObject> slots = new List<GameObject>();

    public ItemLib itemlbs;

    public new TextMeshProUGUI name;
    public TextMeshProUGUI description;

    public Movement player;
    public bool combining;
    public GameObject craftmenu;
    public int mostrecentitem;
    public int craftingitem;

    public CraftingSystem cs;

    // Update is called once per frame
    private void Update()
    {
        items = player.items;
        for (var i = 0; i < items.Count; i++)
        {
            slots[i].GetComponent<Image>().sprite = itemlbs.items[items[i]].icon;
            slots[i].SetActive(true);
            slots[i].name = i.ToString();
        }

        for (var i = items.Count; i < slots.Count; i++)
            slots[i].SetActive(false);  
        Debug.Log(PlayerPrefs.GetString("Password"));
    }

    public void Equip(int i)
    {
        if (!combining)
        {
            mostrecentitem = items[i];
            craftmenu.SetActive(true);
            craftmenu.transform.position = Input.mousePosition;
        }
        else if (items[i] != mostrecentitem)
        {
            Debug.Log("Crafting " + mostrecentitem + " And " + items[i]);
            Debug.Log(player.items.IndexOf(mostrecentitem) + ", " + i);
            cs.craft(mostrecentitem, items[i]);
            craftmenu.SetActive(false);
            combining = false;
        }
        else
        {
            combining = false;
        }
      
    }

    public void PlayerEquip()
    {
        player.equipiten(mostrecentitem);
        craftmenu.SetActive(false);
    }

    public void startcombining()
    {
        combining = true;
        craftmenu.SetActive(false);
        name.text = "Combine With What?";
    }

    public void Enterfield(int i)
    {
        name.text = itemlbs.items[items[i]].name;
        description.text = itemlbs.items[items[i]].description;
    }

    public void Exitfield()
    {
        name.text = "...";
        description.text = "...";
    }
}