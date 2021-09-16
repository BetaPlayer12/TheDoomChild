using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class AutoStepClimb : MonoBehaviour, IComplexCharacterModule
    {
        [SerializeField]
        private RaySensor m_legForwardEnvironment;
        [SerializeField]
        private RaySensor m_destinationFinder;
        [SerializeField, MinValue(0)]
        private float m_distanceFromLegContactForDestination;
        [SerializeField]
        private RaySensor m_spaceChecker;
        [SerializeField, MinValue(0)]
        private Vector2 m_spaceCheckerOffset;

        private Rigidbody2D m_physics;
        private Character m_character;
        private Animator m_animator;
        private int m_animation;
        private Vector2 m_climbToHere;
        private RaycastHit2D[] m_hitBuffer;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_character = info.character;
            m_physics = info.rigidbody;
            m_animator = info.animator;
            m_animation = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.StepClimb);
        }

        public bool CheckForStepClimbableSurface()
        {
            m_legForwardEnvironment.Cast();
            if (m_legForwardEnvironment.allRaysDetecting)
            {
                m_hitBuffer = m_legForwardEnvironment.GetValidHits();
                var legEnvironmentContact = m_hitBuffer[0].point;
                var destinationPosition = m_destinationFinder.transform.position;
                destinationPosition.x = legEnvironmentContact.x + (m_distanceFromLegContactForDestination * (int)m_character.facing);
                m_destinationFinder.transform.position = destinationPosition;

                m_destinationFinder.Cast();
                if (m_destinationFinder.isDetecting)
                {
                    m_hitBuffer = m_destinationFinder.GetValidHits();
                    var possibleDestination = m_hitBuffer[0].point;
                    Raycaster.SetLayerMask(DChildUtility.GetEnvironmentMask());
                    Raycaster.Cast(possibleDestination + new Vector2(0, 0.5f), Vector2.up, m_character.height, true, out int hitcount, true);
                    if (hitcount == 0)
                    {
                        destinationPosition = possibleDestination;
                        destinationPosition.x += (m_spaceCheckerOffset.x * (int)m_character.facing);
                        destinationPosition.y += m_spaceCheckerOffset.y;
                        m_spaceChecker.transform.position = destinationPosition;
                        m_spaceChecker.Cast();
                        if (m_spaceChecker.isDetecting == false)
                        {
                            m_climbToHere = possibleDestination;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void ClimbSurface()
        {
            //m_animator.SetTrigger(m_animation);
            m_character.transform.position = m_climbToHere;
        }
    }
}
