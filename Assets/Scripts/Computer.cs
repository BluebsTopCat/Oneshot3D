using Cinemachine;
using TMPro;
using UnityEngine;
using Yarn.Unity;
using YarnSpinner;
using System;
using System.Runtime.InteropServices;
public class Computer : MonoBehaviour
{
    // Start is called before the first frame update
    public int interactionradius;
    public Inputthing passwordscreen;
    public GameObject onscreen;
    public TextMeshPro windowtext;
    public GameObject window;
    public string[] dialogue;
    public bool shutdown;
    public int textline;
    public GameObject tmpinf;
    public GameObject doorway;
    private bool inwindow;
    private Movement player;
    private GameObject playermesh;
    public CinemachineVirtualCamera vc;
    public AudioSource progress;
    private void Start()
    {
        if (PlayerPrefs.GetInt("CompletedRoom1") == 1)
            Destroy(this);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        playermesh = player.PlayerAnim.gameObject;
    }

    // Update is called once per frame
    private void Update()
    {
        if (shutdown)
            return;

        if (!inwindow)
        {
            if (Input.GetKeyDown(KeyCode.Space) &&
                Vector3.Distance(player.gameObject.transform.position, gameObject.transform.position) <
                interactionradius && player.canmove)
            {
                passwordscreen.se.clip = passwordscreen.start;
                FindObjectOfType<Music>().entercutscene(2,.1f);
                passwordscreen.se.Play();
                vc.Priority = 100;
                passwordscreen.gameObject.SetActive(true);
                passwordscreen.highlight(tmpinf);
                player.PlayerAnim.gameObject.SetActive(false);
                playermesh.SetActive(false);
                player.canmove = false;
            }
        }
        else
        {
            if (textline >= dialogue.Length)
            {
                vc.Priority = 0;
                
                player.PlayerAnim.gameObject.SetActive(true);
                playermesh.SetActive(true);
                player.canmove = true;
                
                //FindObjectOfType<DialogueRunner>().StartDialogue("Door");
                
                onscreen.SetActive(false);
                windowtext.gameObject.SetActive(false);
                window.SetActive(false);
                FindObjectOfType<DialogueRunner>().StartDialogue("Door");
                Destroy(doorway);
                
                PlayerPrefs.SetInt("CompletedRoom1", 1);
                
                shutdown = true;
                GetComponent<BoxCollider>().enabled = true;
                FindObjectOfType<Music>().exitcutscene();
                NativeWinAlert.Error("You only have one shot, " + Environment.UserName + ".","...");
                //do other things here on dialogue completion
            }
            else
            {
                windowtext.text = dialogue[textline];
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    textline++;
                    progress.Play();
                }
            }
        }
    }

    public void initiatecomputerdialogue()
    {
        inwindow = true;
        FindObjectOfType<Music>().entercutscene(1,0f);
        onscreen.SetActive(true);
        windowtext.gameObject.SetActive(true);
        window.SetActive(true);
        player.canmove = false;
        GetComponent<BoxCollider>().enabled = false;
    }

    public void wrongpassword()
    {
        playermesh.SetActive(true);
        player.canmove = true;
        vc.Priority = 0; 
        FindObjectOfType<Music>().exitcutscene();
    }
}