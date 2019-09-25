using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Characters.Players;
using System.Collections;
using UnityEngine;
using System;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    /// <summary>
    /// Replacement of PlacementTracker
    /// </summary>
    [System.Serializable]
    public class LandHandle : MonoBehaviour
    {
        [SerializeField]
        private float m_velocityTreshold;

        private Animator m_animator;
        private string m_animationParameter;
        private string m_speedXParamater;
        private IGroundednessState m_state;
        private IsolatedPhysics2D m_physics;

        [SerializeField]
        private FXSpawner m_fXSpawner;

        private float m_previousYVelocity;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_animator = info.animator;
            m_animationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.Land);
            m_speedXParamater = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SpeedX);
            m_state = info.state;
            m_physics = info.physics;
            if (info.surfaceDector != null)
            {
                info.surfaceDector.NewSurfaceDetected += OnNewSurfaceDetected;
                m_fXSpawner.SetFX(info.surfaceDector.currentSurface.GetFX(Environment.SurfaceData.FXType.Land));
            }
        }

        private void OnNewSurfaceDetected(object sender, SurfaceDetector.SurfaceDetectedEventArgs eventArgs)
        {
            m_fXSpawner.SetFX(eventArgs.surface.GetFX(Environment.SurfaceData.FXType.Land));
        }

        public void RecordVelocity() => m_previousYVelocity = m_physics.velocity.y;
        public void SetRecordedVelocity(Vector2 velocity) => m_previousYVelocity = velocity.y;

        public void Execute()
        {
            if (m_previousYVelocity <= m_velocityTreshold)
            {
                m_animator.SetTrigger(m_animationParameter);
                m_fXSpawner.SpawnFX();
                m_state.waitForBehaviour = true;
                m_animator.SetInteger(m_speedXParamater, 0);
                m_animator.ResetTrigger(m_animationParameter);
                //When GroundednessHandle is disabled for some reason
                //Landing Slides the Character
                StartCoroutine(ForceStopRoutine());
            }
        }

        private IEnumerator ForceStopRoutine()
        {
            var nextFrame = new WaitForEndOfFrame();
            do
            {
                m_physics.SetVelocity(0);
                yield return nextFrame;
            } while (m_state.waitForBehaviour);
        }
    }

}