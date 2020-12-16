using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Inventory
{
 public string name;
 public string description;
 public Sprite icon;
 public Inventory(string _name, string _description, Sprite _icon)
 {
  name = _name;
  description = _description;
  icon = _icon;
 }
}
