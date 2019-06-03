using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassDetector : MonoBehaviour
{
    [SerializeField]
    private LayerMask m_mask;

    [SerializeField]
    private float m_sensorDistance;

    private Vector2 m_grassLocation;

    private bool m_hasLocated;

    private GrassMonster m_minion;

    private Vector2 dir;

    private RaycastHit2D[] hit;

    public Vector2 grassLocation => m_grassLocation;

    public bool hasLocated => m_hasLocated;

    public bool isGrassDetected()
    {
        Raycaster.SetLayerMask(m_mask.value);
        dir = (m_minion.currentFacingDirection == HorizontalDirection.Left ? Vector2.left : Vector2.right);
        var hitcount = 0;
        hit = Raycaster.Cast(transform.position, -dir, m_sensorDistance, false, out hitcount);

        if (hitcount > 0)
        {
            var foliageGrass = hit[hitcount - 1].transform.gameObject.GetComponentInChildren<IFoliage>();
            m_grassLocation = foliageGrass.Location;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Debug.DrawRay(transform.position, -dir * m_sensorDistance);
    }

    private void Start()
    {
        m_sensorDistance = 0f;
    }

    private void Awake()
    {
        m_minion = GetComponentInParent<GrassMonster>();
    }
}
