using UnityEngine;
using Yarn.Unity;
using YarnSpinner;

public class ItemScript : MonoBehaviour
{
    public Movement player;

    public void clicked()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        if (gameObject.name == "Unequip")
        {
            player.activeitem = -1;
            FindObjectOfType<InMemoryVariableStorage>().SetValue("$Equipped", -1);
            return;
        }

        FindObjectOfType<InMemoryVariableStorage>().SetValue("$Equipped", int.Parse(name));
        player.equipiten(int.Parse(name));
    }
}