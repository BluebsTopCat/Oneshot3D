using UnityEngine;

public class CameraTransferScript : MonoBehaviour
{
    public Transform end2;
    public Transform end;
    public float starttime;
    public float speed;

    public bool moving;
    private Transform goingto;
    private Transform start;

    // Update is called once per frame
    private void Update()
    {
        if (moving)
        {
            transform.position = Vector3.Lerp(start.position, goingto.position, (Time.time - starttime) / speed);
            transform.eulerAngles =
                Vector3.Lerp(start.eulerAngles, goingto.eulerAngles, (Time.time - starttime) / speed);

            if (Vector3.Distance(transform.position, end.transform.position) < 0.1f)
                moving = false;
        }
    }

    public void Gotowards(Transform end)
    {
        start = transform;
        starttime = Time.time;
        moving = true;
        goingto = end;
    }
}