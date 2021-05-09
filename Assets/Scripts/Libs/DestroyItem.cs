using UnityEngine;
using Yarn.Unity;

public class DestroyItem : MonoBehaviour
{
    [YarnCommand("destroy")]
    public void Destroy(string playername)
    {
        Destroy(GameObject.Find(playername).gameObject);
        Debug.Log("Destroyed " + playername);
    }
}