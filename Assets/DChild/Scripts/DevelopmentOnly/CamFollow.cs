using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    private CinemachineVirtualCamera m_cinemachineVirtualCamera;

    private void Start()
    {
        StartCoroutine(CamFollowActivateRoutine());
    }

    private void Awake()
    {
        m_cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private IEnumerator CamFollowActivateRoutine()
    {
        while (!m_cinemachineVirtualCamera.enabled)
        {
            m_cinemachineVirtualCamera.enabled = true;
            yield return null;
        }
        yield return null;
    }
}
