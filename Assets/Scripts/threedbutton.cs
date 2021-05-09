using UnityEngine;

public class threedbutton : MonoBehaviour
{
    public Computer c;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("hit!");
                if (hit.transform.gameObject == gameObject)
                    c.textline++;
            }
        }
    }
}