using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class WallStick : MonoBehaviour, ICancellableBehaviour, IComplexCharacterModule
    {
        [SerializeField]
        private RaySensor m_wallSensor;
        [SerializeField]
        private RaySensor m_heightSensor;
        [SerializeField]
        private float m_wallStickOffset;

        private float m_cacheGravityScale;

        private Rigidbody2D m_rigidbody;
        private IWallStickState m_state;
        private Animator m_animator;
        private int m_animationParameter;

        private Collider2D m_cacheCollider;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_rigidbody = info.rigidbody;
            m_cacheGravityScale = m_rigidbody.gravityScale;
            m_state = info.state;
            m_animator = info.animator;
            m_animationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.WallStick);
        }

        public void Cancel()
        {
            m_state.isStickingToWall = false;
            m_rigidbody.gravityScale = m_cacheGravityScale;
            m_animator.SetBool(m_animationParameter, false);
        }

        public bool IsThereAWall()
        {
            m_wallSensor.Cast();
            bool isValid = false;
            if (m_wallSensor.allRaysDetecting)
            {
                var hits = m_wallSensor.GetUniqueHits();
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
                            if (hits[i].distance < m_wallStickOffset)
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

        public bool IsHeightRequirementAchieved()
        {
            if (m_heightSensor)
            {
                m_heightSensor.Cast();
                return m_heightSensor.allRaysDetecting == false;
            }
            else
            {
                return true;
            }
        }

        public void Execute()
        {
            if (m_state.isStickingToWall == false)
            {
                m_state.isStickingToWall = true;
                m_cacheGravityScale = m_rigidbody.gravityScale;
                m_rigidbody.gravityScale = 0;
                m_rigidbody.velocity = Vector2.zero;
                m_animator.SetBool(m_animationParameter, true);
            }
        }
    }
}
