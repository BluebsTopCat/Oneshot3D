using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public int sceneid = 0;
    public void OnTriggerEnter(Collider other)
    {
       SceneManager.LoadScene(sceneid);
    }


}

