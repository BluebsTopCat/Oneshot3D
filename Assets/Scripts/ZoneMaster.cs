using System;
using System.Collections.Generic;
using UnityEngine;

public class ZoneMaster : MonoBehaviour
{
    public List<Area> zones;
    public GameObject player;
    public GameObject maincam;
    public int playerzone;
    public int oldzone;
    public bool cammoving;
    public float movetime;
    public float currentmovetime;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        for (var i = 0; i < zones.Count; i++)
            if (isinside(player.transform.position, zones[i].area))
            {
                if (playerzone != i)
                    oldzone = playerzone;
                playerzone = i;
            }

        cammoving = maincam.transform.position != zones[playerzone].camerapos.position;

        if (cammoving)
            currentmovetime += Time.deltaTime;
        else
            currentmovetime = 0;

        if (cammoving)
        {
            maincam.transform.position = Vector3.Lerp(zones[oldzone].camerapos.position,
                zones[playerzone].camerapos.position, currentmovetime / movetime);
            maincam.transform.rotation = Quaternion.Lerp(zones[oldzone].camerapos.rotation,
                zones[playerzone].camerapos.rotation, currentmovetime / movetime);
        }
    }

    private void OnDrawGizmos()
    {
        foreach (var b in zones)
        {
            Gizmos.DrawWireCube(
                new Vector3(
                    (b.area.x + b.area.y) / 2,
                    4f,
                    (b.area.z + b.area.w) / 2
                ),
                new Vector3(
                    Mathf.Abs(b.area.x - b.area.y),
                    10f,
                    Mathf.Abs(b.area.z - b.area.w)
                )
            );
            Gizmos.DrawIcon(b.camerapos.position, "Cam.png");
        }
    }

    public bool isinside(Vector3 p, Vector4 b)
    {
        return p.x > Mathf.Min(b.x, b.y) &&
               p.x < Mathf.Max(b.x, b.y) &&
               p.z > Mathf.Min(b.z, b.w) &&
               p.z < Mathf.Max(b.z, b.w);
    }
}

[Serializable]
public class Area
{
    public Vector4 area;
    public Transform camerapos;

    public Area(Transform campos, Vector4 place)
    {
        area = place;
        camerapos = campos;
    }
}