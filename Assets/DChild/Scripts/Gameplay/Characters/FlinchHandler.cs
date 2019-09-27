using DChild.Gameplay;
using DChild.Gameplay.Characters;
using UnityEngine;
using Holysoft.Event;
using System.Collections;
using Spine;
using DChild.Gameplay.Combat;
using Spine.Unity;
using System.Collections.Generic;

namespace DChild.Gameplay.Characters
{
    public class FlinchHandler : MonoBehaviour, IFlinch
    {
        [SerializeField]
        private SpineRootAnimation m_spine;
        [SerializeField]
        private IsolatedPhysics2D m_physics;
#if UNITY_EDITOR
        [SerializeField]
        private SkeletonAnimation m_skeletonAnimation;
#endif
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_animation;

        private bool m_isFlinching;

        public event EventAction<EventActionArgs> FlinchStart;
        public event EventAction<EventActionArgs> FlinchEnd;

        public void SetAnimation(string animation) => m_animation = animation;

        public virtual void Flinch(Vector2 directionToSource, RelativeDirection damageSource, IReadOnlyCollection<AttackType> damageTypeRecieved)
        {
            Flinch();
        }

        public void Flinch()
        {
            if (m_isFlinching == false)
            {
                //StopAllCoroutines(); //Gian Editz
                m_physics?.SetVelocity(Vector2.zero);
                StartCoroutine(FlinchRoutine());
            }
        }

        private IEnumerator FlinchRoutine()
        {
            FlinchStart?.Invoke(this, new EventActionArgs());
            m_spine.SetAnimation(0, m_animation, false, 0);
            m_spine.AddEmptyAnimation(0, 0.2f, 0);
            m_isFlinching = true;
            m_spine.AnimationSet += OnAnimationSet;
            m_spine.animationState.Complete += OnAnimationComplete;
            while (m_isFlinching)
            {
                yield return null;
            }
            m_spine.AnimationSet -= OnAnimationSet;
            m_spine.animationState.Complete -= OnAnimationComplete;
            FlinchEnd?.Invoke(this, new EventActionArgs());
        }

        private void OnAnimationComplete(TrackEntry trackEntry)
        {
            m_isFlinching = false;
        }

        private void OnAnimationSet(object sender, AnimationEventArgs eventArgs)
        {
            m_isFlinching = false;
        }


    }
}