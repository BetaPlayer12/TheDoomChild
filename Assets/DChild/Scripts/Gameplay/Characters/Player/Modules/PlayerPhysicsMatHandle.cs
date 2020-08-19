using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class PlayerPhysicsMatHandle : MonoBehaviour, IComplexCharacterModule
    {
        public enum Type
        {
            None,
            Ground,
            Midair
        }

        [System.Serializable]
        private struct Info
        {
            [SerializeField]
            private float m_friction;
            [SerializeField]
            private float m_bounciness;

            public float friction => m_friction;
            public float bounciness => m_bounciness;
        }

        [SerializeField]
        private PhysicsMaterial2D m_grounded;
        [SerializeField]
        private PhysicsMaterial2D m_midair;

        private Rigidbody2D m_rigidBody;
        private Type m_currentType;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_rigidBody = info.rigidbody;
            m_currentType = Type.None;
        }

        public void SetPhysicsTo(Type type)
        {
            if(m_currentType != type)
            {
                if (m_rigidBody.sharedMaterial != null)
                {
                    var material = m_rigidBody.sharedMaterial;
                    var info = m_grounded;

                    switch (type)
                    {
                        case Type.Ground:
                            info = m_grounded;
                            break;
                        case Type.Midair:
                            info = m_midair;
                            break;
                        default:
                            info = null;
                            break;
                    }

                    //Because sharedMaterial is a class
                    m_rigidBody.sharedMaterial = info;

                    m_currentType = type;
                }
            }
        }
    }
}