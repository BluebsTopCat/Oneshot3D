using UnityEngine;

public class PreventRespawn : MonoBehaviour
{
    public string playerpref;

    // Start is called before the first frame update
    private void Start()
    {
        if (PlayerPrefs.GetInt(playerpref) == 1)
            Destroy(gameObject);
    }
}