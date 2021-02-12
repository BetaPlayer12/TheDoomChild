using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class Crouch : MonoBehaviour, ICancellableBehaviour, IComplexCharacterModule
    {
        [SerializeField]
        private RaySensor m_ceilingSensor;
        [SerializeField]
        private RaySensor m_crouchHeightSensor;

        private Animator m_animator;
        private int m_animationParameter;
        private Collider2D m_cacheCollider;
        private ICrouchState m_state;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_animator = info.animator;
            m_animationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsCrouched);
            m_state = info.state;
        }

        public void Cancel()
        {
            m_animator.SetBool(m_animationParameter, false);
            m_state.isCrouched = false;
        }

        public bool IsThereNoCeiling()
        {
            m_ceilingSensor.Cast();
            bool isValid = true;
            if (m_ceilingSensor.allRaysDetecting)
            {
                var hits = m_ceilingSensor.GetUniqueHits();
                for (int i = 0; i < hits.Length; i++)
                {
                    m_cacheCollider = hits[i].collider;
                    if (m_cacheCollider.isTrigger)
                    {
                        return true;
                    }
                    else
                    {
                        if (m_cacheCollider.CompareTag("InvisibleWall") == true)
                        {
                            isValid = false;
                        }
                        else if (m_cacheCollider.CompareTag("Droppable") == true)
                        {
                            isValid = true;
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

        public bool IsCrouchingPossible()
        {
            bool canCrouch = true;
            m_crouchHeightSensor.Cast();

            if (m_crouchHeightSensor.allRaysDetecting)
            {
                var hits = m_ceilingSensor.GetUniqueHits();
                for (int i = 0; i < hits.Length; i++)
                {
                    m_cacheCollider = hits[i].collider;
                    if (m_cacheCollider.isTrigger)
                    {
                        canCrouch = true;
                    }
                    else
                    {
                        if (m_cacheCollider.CompareTag("InvisibleWall") == false)
                        {
                            canCrouch = false;
                        }
                        else
                        {
                            canCrouch = true;
                        }
                    }
                }
            }

            return canCrouch;
        }

        public void Execute()
        {
            m_animator.SetBool(m_animationParameter, true);
            m_state.isCrouched = true;
        }
    }
}
