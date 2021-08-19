using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Environment.VisualConfigurators
{
    [System.Serializable]
    public class SprocketMechanismMovementHandle
    {
        [SerializeField, TabGroup("Gears"), MinValue(0.0001f)]
        private float m_rotationModifier = 1;
        [SerializeField, TabGroup("Gears")]
        private Transform[] m_gears;

        public void MoveChains(float speed)
        {
            speed *= m_rotationModifier;
            for (int i = 0; i < m_gears.Length; i++)
            {
                m_gears[i].Rotate(Vector3.forward, speed);
            }
        }
    }
}