using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public int menuscene = 0;

    public Animator a;

    public Slider volume;
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        a.SetInteger("Screen" , menuscene);
    }

    public void playgame()
    {
        SceneManager.LoadScene(0);
    }

    public void quit()
    {
        Application.Quit();
    }

    public void menuset(int i)
    {
        menuscene = i;
    }

    public void vsync(Toggle t)
    {
        PlayerPrefs.SetInt("VS", bool2int(t.isOn));
        QualitySettings.vSyncCount = PlayerPrefs.GetInt("VS");
    }

    public void antialiasing(Toggle t)
    {
        PlayerPrefs.SetInt("AA", bool2int(t.isOn));
        QualitySettings.antiAliasing = PlayerPrefs.GetInt("AA");
    }

    public void resolution(TMP_Dropdown d)
    {
        string s = d.options[d.value].text;
        PlayerPrefs.SetString("RES", s);
        var res = s.Split('x');
        int x = int.Parse(res[0]);
        int y = int.Parse(res[1]);
        //Screen.SetResolution();
    }

    public void setvol(Slider s)
    {
        PlayerPrefs.SetFloat("Vol", s.value);
        AudioListener.volume = s.value;
    }
    int bool2int(bool b)
    {
        switch (b)
        {
            case true:
                return 1;
            case false:
                return 0;
        }
    }

}
