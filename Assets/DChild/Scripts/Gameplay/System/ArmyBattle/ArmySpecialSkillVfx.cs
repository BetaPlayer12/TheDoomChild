using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holysoft.Event;
using Sirenix.OdinInspector;
using static MinionMaterialConfigurator;
using System;
using static DChild.Gameplay.ArmyBattle.ArmySpecialSkillVfx;
using Spine.Unity;
using DChild.Gameplay.Characters.AI;
using Spine;
using DChild.Gameplay.Characters;


namespace DChild.Gameplay.ArmyBattle
{
    public class ArmySpecialSkillVfx : MonoBehaviour
    {
        [SerializeField]
        private List<VfxParticleTurnManager> m_vfxParticleTurnManager = new List<VfxParticleTurnManager>();
        [SerializeField]
        private List<VfxSpineTurnManager> m_VfxSpineTurnManager = new List<VfxSpineTurnManager>();

        [Serializable]
        public class VfxParticleTurnManager
        {
            [SerializeField]
            public List<ParticleFX> m_fxPartcileSystem;
        }
        [Serializable]
        public class VfxSpineTurnManager
        {
            [SerializeField]
            public GameObject m_SpineModel;
            [SerializeField]
            public SkeletonDataAsset m_fxSpineSystem;
            protected IEnumerable GetEvents()
            {
                ValueDropdownList<string> list = new ValueDropdownList<string>();
                var reference = m_fxSpineSystem.GetAnimationStateData().SkeletonData.Events.ToArray();
                for (int i = 0; i < reference.Length; i++)
                {
                    list.Add(reference[i].Name);
                }
                return list;
            }
            protected IEnumerable GetAnimations()
            {
                ValueDropdownList<string> list = new ValueDropdownList<string>();
                var reference = m_fxSpineSystem.GetAnimationStateData().SkeletonData.Animations.ToArray();
                for (int i = 0; i < reference.Length; i++)
                {
                    list.Add(reference[i].Name);
                }
                return list;
            }
            [SerializeField, ValueDropdown("GetAnimations")]
            private List<string> m_animation;
            [SerializeField, ValueDropdown("GetEvents")]
            public List<string> m_launchOnEvent;
            [SerializeField]
            public SpineEventListener m_spineListener;
            [SerializeField]
            public List<ParticleFX> m_eventPartcileSystem;
            [SerializeField, Min(0f)]
            private List<float> m_delaytime;

            public List<string> animation => m_animation;
            public List<float> animationDelayTime => m_delaytime;
            public List<string> launchOnEvent => m_launchOnEvent;
        }
        [Serializable]
        public class BasicAnimationInfo : VfxSpineTurnManager, IAIAnimationInfo
        {
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_animation;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_launchOnEvent;
            [SerializeField, Min(0f)]
            private float m_timeScale = 1;
            [SerializeField, Min(0f)]
            private float m_delaytime = 1;

            public string animation => m_animation;
            public float animationDelayTime => m_delaytime;
            public float animationTimeScale => m_timeScale;
            public string launchOnEvent => m_launchOnEvent;
        }
        [SerializeField]
        private int m_currentturn=1;
        private SkeletonAnimation skeletonAnimation;
        [Button, HideInPrefabAssets]
        private void PlayEffects()
        {
            for (int i = 0; i < m_vfxParticleTurnManager.Count; i++)
            {

                if (i == m_currentturn - 1)
                {
                    for (int x = 0; x < m_vfxParticleTurnManager[i].m_fxPartcileSystem.Count; x++)
                    {
                        m_vfxParticleTurnManager[i].m_fxPartcileSystem[x].Play();
                    }
                }
                else
                {
                    for (int x = 0; x < m_vfxParticleTurnManager[i].m_fxPartcileSystem.Count; x++)
                    {
                        m_vfxParticleTurnManager[i].m_fxPartcileSystem[x].Stop();
                    }

                }


            }
            for (int i = 0; i < m_VfxSpineTurnManager.Count; i++)
            {

                if (i == m_currentturn - 1)
                {
                    for (int x = 0; x < m_VfxSpineTurnManager[i].animation.Count; x++)
                    {
                        for (int y = 0; y < m_VfxSpineTurnManager[i].m_eventPartcileSystem.Count; y++)
                        {
                            m_VfxSpineTurnManager[i].m_eventPartcileSystem[y].Stop();
                        }
                            StartCoroutine(PlayRoutine(m_VfxSpineTurnManager[i].m_SpineModel, m_VfxSpineTurnManager[i].animation[x], m_VfxSpineTurnManager[i].animationDelayTime[x],
                                m_VfxSpineTurnManager[i].m_spineListener, m_VfxSpineTurnManager[i].m_launchOnEvent[x], m_VfxSpineTurnManager[i].m_eventPartcileSystem[x]));
                        
                    }
                }
                
            }

        }
      
        private IEnumerator PlayRoutine(GameObject Spineasset, String animation,float delaytime, SpineEventListener spineListener, string particleevent, ParticleFX eventPartcileSystem)
        {

            yield return new WaitForSeconds(delaytime);

            skeletonAnimation = Spineasset.GetComponent<SkeletonAnimation>(); 
            skeletonAnimation.AnimationState.SetAnimation(0, animation, false);
            spineListener.Subscribe(particleevent, eventPartcileSystem.Play);
            yield return new WaitForAnimationComplete(skeletonAnimation.AnimationState, animation);
            yield return null;
        }
        private void Start()
        {
            for (int i = 0; i < m_vfxParticleTurnManager.Count; i++)
            {
                    for (int x = 0; x < m_vfxParticleTurnManager[i].m_fxPartcileSystem.Count; x++)
                    {
                    m_vfxParticleTurnManager[i].m_fxPartcileSystem[x].Stop();
                    }

            }

        }

    }
       
}
