using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class IdlingCreature : MonoBehaviour
    {
        [SerializeField]
        private IdlingCreatureData m_idlingCreatureData;

        private SkeletonAnimation m_animation;

        private void Awake()
        {
            m_animation = GetComponentInChildren<SkeletonAnimation>();
        }

        void Start()
        {
            SelectRandomAnimationFromList();
        }

        private void SelectRandomAnimationFromList()
        {
            if (m_animation != null)
            {
                if (m_idlingCreatureData.animationList.Length > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, m_idlingCreatureData.animationList.Length);

                    m_animation.state.SetAnimation(0, m_idlingCreatureData.animationList[randomIndex], true);
                    m_animation.state.Complete += CallSelectRandomAnimationFromList;
                }
            }
        }

        private void OnEnable()
        {
            SelectRandomAnimationFromList();
        }

        private void OnDisable()
        {
            m_animation.state.Complete += FinishAnimation;
        }

        private void CallSelectRandomAnimationFromList(TrackEntry trackEntry)
        {
            m_animation.state.Complete -= CallSelectRandomAnimationFromList;
            SelectRandomAnimationFromList();
        }

        private void FinishAnimation(TrackEntry trackEntry)
        {
            m_animation.state.ClearTracks();
            m_animation.state.Complete -= FinishAnimation;
        }
    }
}
