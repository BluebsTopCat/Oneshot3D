using UnityEngine;
using Yarn.Unity;
using YarnSpinner;

public class GiveItem : MonoBehaviour
{
    public Movement player;
    public AudioSource getitem;
    [YarnCommand("give")]
    public void Give(string itemid)
    {
        Debug.Log("Gave item " + itemid);
        player.items.Add(int.Parse(itemid));
        player.saveinventory();
        getitem.Play();
    }
}