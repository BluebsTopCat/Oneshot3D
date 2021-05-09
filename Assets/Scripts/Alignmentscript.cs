using UnityEngine;

public class Alignmentscript : MonoBehaviour
{
    public RectTransform rct;

    // Update is called once per frame
    private void Update()
    {
        rct.position = new Vector3(rct.position.x, rct.sizeDelta.y, rct.position.z);
    }
}