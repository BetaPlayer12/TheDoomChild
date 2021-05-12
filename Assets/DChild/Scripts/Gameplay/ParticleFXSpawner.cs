using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay
{
    public class ParticleFXSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_FX;
        [SerializeField]
        private Vector2 m_spawnOffset;
        [SerializeField]
        private Vector2 m_spawnScale = Vector2.one;
        [SerializeField,Wrap(0,359)]
        private float m_rotationOffset;
        [SerializeField]
        private bool m_spawnAsChild;
        private FXSpawnHandle<FX> m_fxHandle;

        public void SpawnFX(GameObject fx)
        {
            var instance = m_fxHandle.InstantiateToScene(fx, gameObject.scene);
            var instanceTransform = instance.transform;
            instanceTransform.parent = transform;
            instanceTransform.localPosition = m_spawnOffset;
            instanceTransform.localScale = m_spawnScale;
            instanceTransform.localRotation = Quaternion.Euler(0,0,m_rotationOffset);
            if (m_spawnAsChild == false)
            {
                instanceTransform.parent = null;
            }
        }
    }
}