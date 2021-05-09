using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using YarnSpinner;

public class CinemachineTargetPlayer : MonoBehaviour
{
    private CinemachineVirtualCamera Cvc;
    private CinemachinePath cp;
    private CinemachineDollyCart cdc;
    private Movement m;
    void Start()
    {
        Cvc = FindObjectOfType<CinemachineVirtualCamera>();
        cp = FindObjectOfType<CinemachinePath>();
        m = FindObjectOfType<Movement>();
        cdc = FindObjectOfType<CinemachineDollyCart>();
        Cvc.LookAt = m.transform;
    }

    private void Update()
    {
        cdc.m_Position = cp.FindClosestPoint(m.transform.position, 0, 10, 10);
    }
}
