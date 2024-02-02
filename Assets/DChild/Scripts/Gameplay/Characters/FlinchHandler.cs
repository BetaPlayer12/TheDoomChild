using DChild.Gameplay;
using DChild.Gameplay.Characters;
using UnityEngine;
using Holysoft.Event;
using System.Collections;
using Spine;
using DChild.Gameplay.Combat;
using Spine.Unity;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace DChild.Gameplay.Characters
{
    public class FlinchHandler : MonoBehaviour, IFlinch
    {
        [SerializeField]
        private SpineRootAnimation m_spine;
        [SerializeField]
        private IsolatedPhysics2D m_physics;
        [SerializeField]
        public bool m_autoFlinch;
        [SerializeField]
        public bool m_enableMixFlinch = true;
        [SerializeField]
        public bool m_enableEmpytyIdle = true;
        [SerializeField, Range(0f, 1f), ShowIf("m_enableMixFlinch")]
        private float m_alphaBlendStrength = 0.5f;
        [SerializeField, Range(0f, 1f), ShowIf("m_enableMixFlinch")]
        private float m_mixDuration = 1f;

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
        [SerializeField]
        public bool m_enableFlinchColor = false;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation"), ShowIf("m_enableFlinchColor")]
        private string m_flinchColorAnimation;

        private bool m_isFlinching;

        //public event EventAction<EventActionArgs> HitStopStart;
        public event EventAction<EventActionArgs> FlinchStart;
        public event EventAction<EventActionArgs> FlinchEnd;
        public bool autoFlinching => m_autoFlinch;
        public bool isFlinching => m_isFlinching;

        private Coroutine m_flinchRoutine;
        private Coroutine m_flinchColorRoutine;

        public void SetAnimation(string animation) => m_animation = animation;

        public void SetIdleAnimation(string idleAnimation) => m_idleAnimation = idleAnimation;

        public void SetFlinchColorAnimation(string flinchColorAnimation) => m_flinchColorAnimation = flinchColorAnimation;

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

        private void StartFlinch()
        {
            m_physics?.SetVelocity(Vector2.zero);
            m_flinchRoutine = StartCoroutine(FlinchRoutine());
            if (!m_autoFlinch && m_enableMixFlinch)
                StartCoroutine(FlinchMixRoutine());
            if (m_enableFlinchColor)
            {
                if (m_flinchColorRoutine == null)
                {
                    m_flinchColorRoutine = StartCoroutine(FlinchColorRoutine());
                }
            }
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

        private IEnumerator FlinchMixRoutine()
        {
            m_spine.SetAnimation(1, m_animation, false, 0).MixBlend = MixBlend.First;
            if (m_enableEmpytyIdle)
            {
                m_spine.AddEmptyAnimation(1, m_mixDuration, 0).Alpha = 0f;
            }
            else
            {
                m_spine.AddAnimation(1, m_idleAnimation, true, 0).Alpha = 0f;
            }
            m_spine.animationState.GetCurrent(1).Alpha = m_alphaBlendStrength;
            m_spine.animationState.GetCurrent(1).MixDuration = 1;
            m_spine.animationState.GetCurrent(1).MixTime = 1;
            yield return null;
        }

        private IEnumerator FlinchColorRoutine()
        {
            m_spine.SetAnimation(2, m_flinchColorAnimation, false);
            yield return new WaitForAnimationComplete(m_spine.animationState, m_flinchColorAnimation);
            m_spine.SetEmptyAnimation(2, 0);
            m_flinchColorRoutine = null;
            yield return null;
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