using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity.Example;
using YarnSpinner;

public class HaveEquippedItem : MonoBehaviour
{
    public Movement player;
    public int item;

    public NPC npc;

    public string dialogue1;

    public string dialogue2;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.activeitem == item)
            npc.talkToNode = dialogue1;
        else
            npc.talkToNode = dialogue2;
    }
}
