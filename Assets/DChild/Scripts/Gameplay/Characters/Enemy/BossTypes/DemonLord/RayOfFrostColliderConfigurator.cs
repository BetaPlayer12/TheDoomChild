using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayOfFrostColliderConfigurator : MonoBehaviour
{
    [SerializeField]
    private LineRenderer m_reference;
    [SerializeField]
    private EdgeCollider2D m_collider;
    [SerializeField]
    private float m_activateColliderThreshold;

    private MaterialPropertyBlock m_propertyBlock;
    private int m_shraderAlphaProperty2D;
    private List<Vector2> m_colliderPoints;


    public void ReorientCollider()
    {
        m_colliderPoints.Clear();
        for (int i = 0; i < m_reference.positionCount; i++)
        {
            m_colliderPoints.Add(m_reference.GetPosition(i));

        }
        m_collider.SetPoints(m_colliderPoints);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
