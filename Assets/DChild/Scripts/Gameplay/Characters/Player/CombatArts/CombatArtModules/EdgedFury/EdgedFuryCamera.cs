using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgedFuryCamera : MonoBehaviour
{
    [SerializeField]
    private Camera m_camera;
    [SerializeField]
    private LayerMask m_mask;
    [SerializeField]
    private ParticleSystem m_fx;

    public void ActivateCullingMask()
    {
        m_camera.cullingMask = m_mask;
    }

    //private IEnumerator CullingCameraRoutine()
    //{
    //    var time = 5f;
    //    while (time > 0)
    //    {
    //        m_camera.cullingMask = m_mask;
    //        time -= Time.deltaTime;
    //        yield return null;
    //    }
    //    yield return null;
    //}
}
