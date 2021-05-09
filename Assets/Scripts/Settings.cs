using UnityEngine;

public class Settings : MonoBehaviour
{
    public GameObject window;
    public AudioSource opennoise;
    public AudioSource closenoise;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            window.gameObject.SetActive(!window.gameObject.activeSelf);
            if(window.gameObject.activeSelf)
                opennoise.Play();
            else 
                closenoise.Play();
        }
    }
}