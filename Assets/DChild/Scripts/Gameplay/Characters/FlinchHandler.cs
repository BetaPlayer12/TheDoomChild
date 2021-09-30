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
        private ConfigurableFlinchData m_flinchData;

        [SerializeField]
        private SpineRootAnimation m_spine;
        [SerializeField]
        private IsolatedPhysics2D m_physics;
        [SerializeField]
        public bool m_autoFlinch;

#if UNITY_EDITOR
        [SerializeField]
        private SkeletonAnimation m_skeletonAnimation;

        public void InitializeField(SpineRootAnimation spineRoot,IsolatedPhysics2D physics, SkeletonAnimation animation)
        {
            m_spine = spineRoot;
            m_physics = physics;
            m_skeletonAnimation = animation;
        }
#endif
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_animation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_idleAnimation;

        private bool m_isFlinching;

        //public event EventAction<EventActionArgs> HitStopStart;
        public event EventAction<EventActionArgs> FlinchStart;
        public event EventAction<EventActionArgs> FlinchEnd;
        public bool autoFlinching => m_autoFlinch;
        public bool isFlinching => m_isFlinching;

        private Coroutine m_flinchRoutine;

        public void SetAnimation(string animation) => m_animation = animation;

        public virtual void Flinch(Vector2 directionToSource, RelativeDirection damageSource, AttackSummaryInfo attackInfo)
        {
         
            Flinch();
        }

        public void Flinch()
        {
            if (m_isFlinching == false)
            {
                StartFlinch();
            }
        }

        private void UpdateFlinchRestrictions()
        {

        }

        private void StartFlinch()
        {
            m_physics?.SetVelocity(Vector2.zero);
            m_flinchRoutine = StartCoroutine(FlinchRoutine());
        }

        private IEnumerator FlinchRoutine()
        {
            FlinchStart?.Invoke(this, new EventActionArgs());
            if (m_autoFlinch)
            {
                m_spine.SetAnimation(0, m_idleAnimation, true);
                m_spine.SetAnimation(0, m_animation, false, 0);
                m_spine.AddAnimation(0, m_idleAnimation, false, 0.2f).TimeScale = 20;

                //m_spine.AddEmptyAnimation(0, 0.2f, 0);
            }
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