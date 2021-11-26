using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour
{
    public List<zone> z;
    public AudioMixerGroup amg;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        SceneManager.activeSceneChanged += ChangedActiveScene;
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        int newscene = next.buildIndex;
        refresh(newscene);
    }

   public void refresh(int news)
   {
       foreach (zone zed in z)
       {
           if (!zed.areas.Contains(news))
           {

               foreach (audvolpair ads in zed.tracks)
               {
                   Destroy(ads.audsor);
               }
           }
       }

       foreach (zone zed in z)
        {
            if (zed.areas.Contains(news))
            {

                foreach (audvolpair asclip in zed.tracks)
                {
                    if (asclip.audsor == null)
                    {
                        AudioSource newaudio = gameObject.AddComponent<AudioSource>();
                        newaudio.clip = asclip.track;
                        newaudio.volume = asclip.volume;
                        newaudio.outputAudioMixerGroup = amg;
                        newaudio.loop = true;
                        asclip.audsor = newaudio;
                        newaudio.Play();
                    }
                }
            }
        }
    }

   public void entercutscene(float speed, float volume)
   {
       foreach (zone zed in z)
       {
           foreach (audvolpair ads in zed.tracks)
           {
               if (ads.audsor != null)
                   StartCoroutine(FadeTo(ads.audsor, speed,volume));
           }
       }
   }

   public static IEnumerator FadeTo (AudioSource audioSource, float FadeTime, float targetvol)
   {
       float started = 0;
       while (Mathf.Abs(audioSource.volume - targetvol) > .02f)
       {
           if (started > FadeTime) break;
           audioSource.volume = Mathf.Lerp(audioSource.volume, targetvol, Time.deltaTime / FadeTime);
           started += Time.deltaTime;
           yield return null;
       }
       audioSource.volume = targetvol;
       Debug.Log("Fadedone");
   }

   public void exitcutscene()
   {
       foreach (zone zed in z)
       {
           foreach (audvolpair ads in zed.tracks)
           {
               if (ads.audsor != null)
                   StartCoroutine(FadeTo(ads.audsor, 1f, ads.volume));
           }
       }
   }
}




[System.Serializable]
public class zone
{
    public string name;
    public List<audvolpair> tracks;
    public List<int> areas;

    public zone(List<audvolpair> t, List<int> a, string str)
    {
        tracks = t;
        areas = a;
        name = str;
    }
}
[System.Serializable]
public class audvolpair
{
    public AudioClip track;
    public AudioSource audsor;
    public float volume;

    public audvolpair(AudioClip t, float i)
    {
        track = t;
        volume = i;
    }
}

