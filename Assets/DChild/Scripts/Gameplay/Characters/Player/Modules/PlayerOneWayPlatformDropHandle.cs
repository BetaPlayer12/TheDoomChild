using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class PlayerOneWayPlatformDropHandle : MonoBehaviour, IComplexCharacterModule
    {
        [SerializeField]
        private GroundednessHandle m_groundedHandle;
        [SerializeField]
        private RaySensor m_droppablePlatformSensor;
        [SerializeField]
        private float m_waitTime = 0.5f;

        private CharacterColliders m_playerCollider;
        private Collider2D m_cacheCollider;
        private float m_timer;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_playerCollider = info.character.colliders;
        }

        public void Execute()
        {
            m_playerCollider.IgnoreCollider(m_cacheCollider);
            m_timer = m_waitTime;
            m_groundedHandle.Enabled = false;
            m_groundedHandle.ChangeValue(false);
        }

        public void HandleDroppablePlatformCollision()
        {
            if (m_timer > 0)
            {
                m_timer -= GameplaySystem.time.deltaTime;
                if (m_timer <= 0)
                {
                    m_playerCollider.ClearIgnoredCollider(m_cacheCollider);
                    m_groundedHandle.Enabled = true;
                }
            }
        }

        public bool IsThereADroppablePlatform()
        {
            bool isValid = false;

            m_droppablePlatformSensor.Cast();

            if (m_droppablePlatformSensor.allRaysDetecting)
            {
                var hits = m_droppablePlatformSensor.GetUniqueHits();

                for (int i = 0; i < hits.Length; i++)
                {
                    m_cacheCollider = hits[i].collider;
                    if (m_cacheCollider.isTrigger)
                    {
                        return false;
                    }
                    else
                    {
                        if (m_cacheCollider.CompareTag("InvisibleWall") == false)
                        {
                            if (m_cacheCollider.CompareTag("Droppable") == true)
                            {
                                isValid = true;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }

            return isValid;
        }
    }
}
