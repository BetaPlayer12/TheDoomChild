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
            private string m_sproutAnimation;
            public string sproutAnimation => m_sproutAnimation;
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
            Sprout,
            //Grow,
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
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_disturbedGrass;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_collider;
        [SerializeField, TabGroup("Reference")]
        private Damageable m_damageable;
        public GameObject m_motherMantisAI;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        private bool m_isPetalRain;
        public bool m_hasMantisLanded;
        public bool m_checker;

        public EventAction<EventActionArgs> Growing;

        public void GetTarget(AITargetInfo target)
        {
            m_targetInfo = target;
        }
        private void OnDestroy()
        {
            StopAllCoroutines();
            //m_motherMantisAI.OnPetalRain -= OnPetalRain;
            StartCoroutine(DeathFxRoutine());
        }
        private IEnumerator SproutRoutine()
        {
            m_checker = false;
            m_stateHandle.Wait(State.Idle);
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            m_collider.enabled = false;
            m_disturbedGrass.Play();
            Growing?.Invoke(this, EventActionArgs.Empty);
            m_animation.SetAnimation(0, m_info.sproutAnimation, false);
            //yield return new WaitForSeconds(1f);
            //m_motherMantisAI.OnMantisLand += OnMantisLand;
            //m_motherMantisAI.GetComponent<MotherMantisAI>().OnMantisLand += OnMantisLand;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        private IEnumerator GrowthRoutine()
        {
            m_stateHandle.Wait(State.Idle);
            //yield return new WaitForSeconds(.2f);
            //m_motherMantisAI.GetComponent<MotherMantisAI>().OnMantisLand += OnMantisLand;
            m_animation.SetAnimation(0, m_info.growAnimation, false);
            yield return new WaitForSeconds(1f);
            m_collider.enabled = true;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.growAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_hitbox.SetInvulnerability(Invulnerability.None);
            this.GetComponent<Damageable>().Destroyed += OnDestroyed;
            yield return new WaitForSeconds(2f);
            m_hasMantisLanded = false;
            //m_motherMantisAI.GetComponent<MotherMantisAI>().OnMantisLand -= OnMantisLand;
            if (m_isPetalRain == false && m_checker == false)
            {
                StartCoroutine(WiltFxRoutine());
            }
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        private void OnMantisLand(object sender, EventActionArgs eventActionArgs)
        {
            m_hasMantisLanded = true;
        }

        private IEnumerator DeathFxRoutine()
        {
            m_animation.SetAnimation(0, m_info.deathAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathAnimation);
            //m_isPetalRain = true;
            //yield return new WaitForSeconds(1f);
            Destroy(this.gameObject);
            yield return null;

        }

        private IEnumerator WiltFxRoutine()
        {
            m_animation.SetAnimation(0, m_info.wiltAnimation, false);
            m_collider.enabled = false;
            m_hitbox.enabled = false;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.wiltAnimation);
            Destroy(this.gameObject);
            //m_motherMantisAI.OnPetalRain += OnPetalRain;
            yield return null;
        }

        protected override void Awake()
        {
            base.Awake();
            //m_motherMantisAI = GameObject.Find("MotherMantis");
            m_motherMantisAI.GetComponent<MotherMantisAI>().OnMantisLand += OnMantisLand;
            m_motherMantisAI.GetComponent<MotherMantisAI>().OnPetalRain += OnPetalRain;
            var sizeMult = UnityEngine.Random.Range(119, 120) * .01f;
            transform.localScale = new Vector2(transform.localScale.x * sizeMult, transform.localScale.y * sizeMult);
            m_stateHandle = new StateHandle<State>(State.Sprout, State.WaitBehaviourEnd);

        }

        /*private void Start()
        {
            base.Start();
            *//*m_motherMantisAI.GetComponent<MotherMantisAI>().OnMantisLand += OnMantisLand;
            m_motherMantisAI.GetComponent<MotherMantisAI>().OnPetalRain += OnPetalRain;*//*
        }*/
        private void OnPetalRain(object sender, EventActionArgs eventActionArgs )
        {
            m_isPetalRain = false;
        }
        private void OnDestroyed(object sender, EventActionArgs eventActionArgs )
        {
            m_checker = true;
        }
        /*public void CallGrowthRoutine()
        {
            StartCoroutine(GrowthRoutine());
        }*/
        /*private void OnMantisLand(object sender, EventActionArgs eventActionArgs)
        {
            stalagmiteNotGrowing = false;
            m_isMantisGrounded = true;
        }*/
        private void Update()
        {
            switch (m_stateHandle.currentState)
            {
                case State.Sprout:
                    StartCoroutine(SproutRoutine());
                    break;
                case State.Idle:
                    if (m_hasMantisLanded == true)
                    {
                        StartCoroutine(GrowthRoutine());
                    }
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
            m_stateHandle.OverrideState(State.Sprout);
        }
    }

}
