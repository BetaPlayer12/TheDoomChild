using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectFollower
{
    private int m_referenceID;
    [ShowInInspector]
    private Transform m_toFollow;
    [ShowInInspector, DisableInPlayMode]
    private bool m_useScreenPosition;
    private Transform m_transform;
    private RectTransform m_rectTrasform;
    private Camera m_camera;

    public int referenceID => m_referenceID;

    public void SetTarget(Transform transform)
    {
        m_toFollow = transform;
    }

    public void Initialize(int refrenceID, GameObject objectInstance)
    {
        m_referenceID = refrenceID;
        m_useScreenPosition = false;
        m_transform = objectInstance.transform;
        m_rectTrasform = null;
        m_camera = null;
    }


    public void InitializeUsingScreenPosition(int refrenceID, GameObject objectInstance, Camera cameraReference)
    {
        m_referenceID = refrenceID;
        m_useScreenPosition = true;
        m_transform = null;
        m_rectTrasform = objectInstance.GetComponent<RectTransform>();
        m_camera = cameraReference;
    }

    public void FollowTarget(Vector3 offset)
    {
        if (m_useScreenPosition)
        {
            m_rectTrasform.anchoredPosition = m_camera.WorldToScreenPoint(m_toFollow.position) + offset;
        }
        else
        {
            m_transform.position = m_toFollow.position + offset;
        }
    }
}
