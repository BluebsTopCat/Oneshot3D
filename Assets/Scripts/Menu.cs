using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Menu : MonoBehaviour
{
    public int scene;
    public VideoPlayer m_VideoPlayer;

    IEnumerator Start() 
    {
        m_VideoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath,"FBIntro.mp4");
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(scene);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene(scene);
    }
    
}
