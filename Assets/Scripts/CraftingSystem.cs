using System;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using YarnSpinner;

public class CraftingSystem : MonoBehaviour
{
    public Movement player;
    public AudioSource craftsfx;

    public List<CraftingRecipe> recipes;
    // Start is called before the first frame update

    private void start()
    {
        player = GameObject.Find("Player").GetComponent<Movement>();
    }

    public void craft(int item1, int item2)
    {
        var c = recipes.Find(
            x =>
                x.item1 == item1
                && x.item2 == item2
                ||
                x.item1 == item2
                && x.item2 == item1
        );

        if (c == null) return;
        if (c.output != 0)
        {
            player.items.Remove(item1);
            Debug.Log("Removed " + player.items.IndexOf(item1));
            player.items.Remove(item2);
            Debug.Log("Removed " + player.items.IndexOf(item2));
            player.items.Add(c.output);
            if (player.activeitem == item1 || player.activeitem == item2)
                player.activeitem = -1;
            if (c.output2 != 0)
                player.items.Add(c.output2);
            player.saveinventory();
            craftsfx.Play();
        }

        if (c.Dialogue == null) return;
        var dialogueRunner = FindObjectOfType<DialogueRunner>();
        dialogueRunner.Add(c.Dialogue);
        dialogueRunner.StartDialogue(c.node);
    }
}

[Serializable]
public class CraftingRecipe
{
    public int item1;
    public int item2;
    public int output;
    public int output2;

    [Header("Optional")] public YarnProgram Dialogue;

    public string node;

    public void craft(int it1, int it2, int it3, int it4, YarnProgram dialogue, string node1)
    {
        item1 = it1;
        item2 = it2;
        output = it3;
        Dialogue = dialogue;
        node = node1;
        output2 = it4;
    }
}