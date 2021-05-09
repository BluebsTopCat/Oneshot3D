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
        SceneManager.LoadScene(PlayerPrefs.GetInt("Scene"));
        GameObject player2 = Instantiate(this.player);
        
        Vector3 pos = new Vector3(PlayerPrefs.GetFloat("PlayerPosX"),PlayerPrefs.GetFloat("PlayerPosY"),PlayerPrefs.GetFloat("PlayerPosZ"));
        Quaternion rot = new Quaternion(PlayerPrefs.GetFloat("PlayerRotX"),PlayerPrefs.GetFloat("PlayerRotY"),PlayerPrefs.GetFloat("PlayerRotZ"),0f);
        player2.transform.SetPositionAndRotation(pos,rot);
        Destroy(this.gameObject);
    }
}
