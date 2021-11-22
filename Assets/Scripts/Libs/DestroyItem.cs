using UnityEngine;
using Yarn.Unity;

public class DestroyItem : MonoBehaviour
{
    [YarnCommand("destroy")]
    public void destroy(string name)
    {
        if(GameObject.Find(name))
           Destroy(GameObject.Find(name));
        Debug.Log("Destroyed " + name);
    }
}