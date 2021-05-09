using UnityEngine;
using Yarn.Unity;
using YarnSpinner;

public class TakeItem : MonoBehaviour
{
    public Movement player;

    [YarnCommand("take")]
    public void take(string itemid)
    {
        Debug.Log("Removed item " + itemid);
        player.items.Remove(int.Parse(itemid));
        if (player.activeitem == int.Parse(itemid))
            player.activeitem = -1;
        player.saveinventory();
    }
}