using UnityEngine;
using Sirenix.OdinInspector;
using Holysoft.Event;
using System.Collections;
using Spine;

namespace DChild.Gameplay.Characters
{
    public class DisplacementHandler : MonoBehaviour
    {
        [SerializeField]
        private SpineRootAnimation m_spine;
        [SerializeField]
        private IsolatedPhysics2D m_physics;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_animation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_idleAnimation;
        public bool enable = true;

        private bool m_isKnockedBack;

        public event EventAction<EventActionArgs> Start;
        public event EventAction<EventActionArgs> End;

        public void Execute(Vector2 force)
        {
            if (enable)
            {
                StopAllCoroutines();
                m_spine.animationState.Complete -= OnAnimationComplete;
                StartCoroutine(Routine(force)); 
            }
        }

        public void Stop()
        {
            if (enable)
            {
                StopAllCoroutines();
                m_isKnockedBack = false;
                m_physics.SetVelocity(Vector2.zero);
                m_spine.SetAnimation(0, m_idleAnimation, true);
                End?.Invoke(this, EventActionArgs.Empty);
            }
        }

        private IEnumerator Routine(Vector2 force)
        {
            Start?.Invoke(this, EventActionArgs.Empty);
            m_physics.SetVelocity(Vector2.zero);
            m_physics.AddForce(force, ForceMode2D.Impulse);

            m_spine.SetAnimation(0, m_idleAnimation, true);
            m_spine.SetAnimation(0, m_animation, false, 0);
            m_spine.AddAnimation(0, m_idleAnimation, false, 0.2f).TimeScale = 20;

            m_isKnockedBack = true;
            m_spine.animationState.Complete += OnAnimationComplete;
            while (m_isKnockedBack)
            {
                yield return null;
            }
            m_physics.SetVelocity(Vector2.zero);
            m_spine.animationState.Complete -= OnAnimationComplete;
            End?.Invoke(this, EventActionArgs.Empty);
        }

        private void OnAnimationComplete(TrackEntry trackEntry)
        {
            m_isKnockedBack = false;
        }
    }
}