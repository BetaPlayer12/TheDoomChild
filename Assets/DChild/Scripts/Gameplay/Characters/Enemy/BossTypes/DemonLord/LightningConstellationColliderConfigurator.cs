using UnityEngine;
using System.Collections.Generic;

namespace DChild.Gameplay.Characters.Enemies
{
    public class LightningConstellationColliderConfigurator : MonoBehaviour
    {
        [SerializeField]
        private LineRenderer m_reference;
        [SerializeField]
        private EdgeCollider2D m_collider;
        [SerializeField]
        private float m_activeColliderThreshold;

        private MaterialPropertyBlock m_propertyBlock;
        private int m_shaderAlphaPropertyID;
        private List<Vector2> m_colliderPoints;

        public void ReorientCollider()
        {
            m_colliderPoints.Clear();
            for (int i = 0; i < m_reference.positionCount; i++)
            {
                m_colliderPoints.Add(m_reference.GetPosition(i));
            }
            m_collider.SetPoints(m_colliderPoints);
            m_collider.offset = m_reference.transform.position * -1;
        }

        private void SyncColliderActivationWithMaterial()
        {
            m_reference.GetPropertyBlock(m_propertyBlock);
            var enableCollider = m_propertyBlock.GetFloat(m_shaderAlphaPropertyID) >= m_activeColliderThreshold;
            m_collider.enabled = enableCollider;

        }

        private void Awake()
        {
            m_colliderPoints = new List<Vector2>();
            m_propertyBlock = new MaterialPropertyBlock();
            m_shaderAlphaPropertyID = Shader.PropertyToID("_Alpha");
        }

        private void Update()
        {
            SyncColliderActivationWithMaterial();
            ReorientCollider();
        }

    }
}