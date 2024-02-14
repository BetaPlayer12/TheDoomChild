using System;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using DChild.Gameplay.Characters.AI;
using UnityEngine;
using Spine;
using Spine.Unity;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using DChild;
using DChild.Gameplay.Characters.Enemies;


namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/PetalStalagmite")]
    public class PetalStalagtite : CombatAIBrain<PetalStalagtite.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            //Animations
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_growAnimation;
            public string growAnimation => m_growAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_death2Animation;
            public string death2Animation => m_death2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinch2Animation;
            public string flinch2Animation => m_flinch2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_wiltAnimation;
            public string wiltAnimation => m_wiltAnimation;

            public override void Initialize()
            {
#if UNITY_EDITOR
                //
#endif
            }
        }

        private enum State
        {
            Grow,
            Idle,
            WaitBehaviourEnd,
        }

        //[SerializeField]
        //private Info m_info;
        /*[SerializeField]
        private SkeletonAnimation m_skeletonAnimation;

        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_growthAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_idleAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_flinchAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_flinchAnimation2;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_deathAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_deathAnimation2;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_wiltAnimation;*/

        /*[SerializeField]
        private GameObject m_colliders;*/
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private Damageable m_damageable;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;

        public void GetTarget(AITargetInfo target)
        {
            m_targetInfo = target;
        }
        private void OnDestroy()
        {
            StopAllCoroutines();
            StartCoroutine(DeathFxRoutine());
        }
        private IEnumerator GrowthRoutine()
        {
            m_stateHandle.Wait(State.Idle);
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            m_animation.SetAnimation(0, m_info.growAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.growAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_hitbox.SetInvulnerability(Invulnerability.None);

            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator DeathFxRoutine()
        {
            m_animation.SetAnimation(0, m_info.deathAnimation, false);
            yield return null;

        }

        private IEnumerator WiltFxRoutine()
        {
            yield return new WaitForSeconds(0.25f);
            m_animation.SetAnimation(0, m_info.wiltAnimation, false);
            yield return null;
        }

        protected override void Awake()
        {
            base.Awake();
            var sizeMult = UnityEngine.Random.Range(119, 120) * .01f;
            transform.localScale = new Vector2(transform.localScale.x * sizeMult, transform.localScale.y * sizeMult);
            m_stateHandle = new StateHandle<State>(State.Grow, State.WaitBehaviourEnd);

        }

        private void Update()
        {
            switch (m_stateHandle.currentState)
            {
                case State.Grow:
                    StartCoroutine(GrowthRoutine());
                    break;
                case State.Idle:
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }
        }

        public override void ReturnToSpawnPoint()
        {
            /*throw new NotImplementedException();*/
        }

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.Grow);
        }
    }

}
