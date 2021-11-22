using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerIniter : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        int firsttime = PlayerPrefs.GetInt("FirstTime");
        if (firsttime != 0)
        {
            
            SceneManager.LoadScene(PlayerPrefs.GetInt("Scene", 3));
            GameObject player2 = Instantiate(this.player);

            Vector3 pos = new Vector3(PlayerPrefs.GetFloat("PlayerPosX", -2), PlayerPrefs.GetFloat("PlayerPosY", 1.4f), PlayerPrefs.GetFloat("PlayerPosZ", 1f));
            Quaternion rot = new Quaternion(PlayerPrefs.GetFloat("PlayerRotX"), PlayerPrefs.GetFloat("PlayerRotY"), PlayerPrefs.GetFloat("PlayerRotZ"), 0f);
            player2.transform.SetPositionAndRotation(pos, rot);
            Destroy(this.gameObject);
            
        }
        else
        {
            PlayerPrefs.SetInt("FirstTime", 1);
            SceneManager.LoadScene(1);
        }
    }
}
