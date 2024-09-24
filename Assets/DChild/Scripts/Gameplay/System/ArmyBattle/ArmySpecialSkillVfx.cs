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
            public SkeletonDataAsset m_fxSpineSystem;
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
            [SerializeField, Min(0f)]
            private List<float> m_delaytime;

            public List<string> animation => m_animation;
            public List<float> animationDelayTime => m_delaytime;
        }
        [Serializable]
        public class BasicAnimationInfo : VfxSpineTurnManager
        {
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_animation;
            [SerializeField, Min(0f)]
            private float m_delaytime = 1;

            public string animation => m_animation;
            public float animationDelayTime => m_delaytime;
        }
        [SerializeField]
        private int m_currentturn=1;
        
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
