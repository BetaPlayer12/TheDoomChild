using DChild.Gameplay.Environment;
using DChild.Gameplay.NavigationMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMapConfigurator : MonoBehaviour
{
    [SerializeField]
    private Location m_location;
    [SerializeField]
    private Transform m_inGameReferencePoint;
    [SerializeField]
    private Vector3 m_mapReferencePoint;
    [SerializeField]
    private Vector2 m_offset;
    [SerializeField]
    private NavMapManager m_mapManager;

    public Location location => m_location;
    public Transform inGameReferencePoint => m_inGameReferencePoint;
    public Vector3 mapReferencePoint => m_mapReferencePoint;
    public Vector2 offset => m_offset;

    public void Awake()
    {
        m_mapManager.UpdateConfiguration(location, inGameReferencePoint, mapReferencePoint, offset);
    }
}
