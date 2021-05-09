using TMPro;
using UnityEngine;
using Yarn.Unity;
using YarnSpinner;

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
    public GameObject othercameracontroller;
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
                othercameracontroller.SetActive(false);
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
                othercameracontroller.SetActive(true);
                
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
                
                //do other things here on dialogue completion
            }

            windowtext.text = dialogue[textline];
            if (Input.GetKeyDown(KeyCode.Space))
                textline++;
        }
    }

    public void initiatecomputerdialogue()
    {
        inwindow = true;
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
        othercameracontroller.SetActive(true);
    }
}