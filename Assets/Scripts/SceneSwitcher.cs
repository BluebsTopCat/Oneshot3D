using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public int scene;

    public void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(scene);
    }
}