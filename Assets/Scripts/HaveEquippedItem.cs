using System;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity.Example;
using YarnSpinner;

public class HaveEquippedItem : MonoBehaviour
{
    public Movement player;


    public NPC npc;

    public List<ItemDialogueCombo> items;
    public string defaultdialogue;

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
    }

    // Update is called once per frame
    private void Update()
    {
        var anygooditemequipped = false;
        foreach (var d in items)
            if (player.activeitem == d.item)
            {
                anygooditemequipped = true;
                npc.talkToNode = d.dialogue;
            }

        if (!anygooditemequipped)
            npc.talkToNode = defaultdialogue;
    }
}

[Serializable]
public class ItemDialogueCombo
{
    public int item;
    public string dialogue;

    public ItemDialogueCombo(int i, string s)
    {
        item = i;
        dialogue = s;
    }
}