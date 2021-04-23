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
        private bool m_isAnimationStateReady;
        private bool m_isListening;

        private int m_currentIndex;
        private int m_numberOfRepeatsOnSameIndex;
        private const int MAXANIMATIONREPEATS = 3;

        private void Awake()
        {
            m_animation = GetComponentInChildren<SkeletonAnimation>();
        }

        private void Start()
        {
            m_animation.state.Complete += CallSelectRandomAnimationFromList;
            m_isListening = true;
            SelectRandomAnimationFromList(UnityEngine.Random.Range(0f, 2f));
            m_isAnimationStateReady = true;
        }

        private void SelectRandomAnimationFromList(float delay = 0)
        {
            if (m_animation != null)
            {
                if (m_idlingCreatureData.animationList.Length > 0)
                {
                    int randomIndex = 0;
                    do
                    {
                        randomIndex = UnityEngine.Random.Range(0, m_idlingCreatureData.animationList.Length);
                        if (m_currentIndex == randomIndex)
                        {
                            m_numberOfRepeatsOnSameIndex++;
                        }
                        else
                        {
                            m_currentIndex = randomIndex;
                            m_numberOfRepeatsOnSameIndex = 0;
                        }
                    } while (m_numberOfRepeatsOnSameIndex == MAXANIMATIONREPEATS && m_currentIndex == randomIndex);


                    m_animation.state.SetAnimation(0, m_idlingCreatureData.animationList[m_currentIndex], true).Delay = delay;

                }
            }
        }

        private void OnEnable()
        {
            if (m_isAnimationStateReady)
            {
                SelectRandomAnimationFromList(UnityEngine.Random.Range(0f, 1f));
                if (m_isListening == false)
                {
                    m_animation.state.Complete += CallSelectRandomAnimationFromList;
                }
            }
        }

        private void OnDisable()
        {
            if (m_animation.state != null)
            {
                m_animation.state.Complete -= CallSelectRandomAnimationFromList;
                m_isListening = false;
            }
        }

        private void CallSelectRandomAnimationFromList(TrackEntry trackEntry)
        {
            SelectRandomAnimationFromList();
        }

        //private void FinishAnimation(TrackEntry trackEntry)
        //{
        //    m_animation.state.ClearTracks();
        //    m_animation.state.Complete -= FinishAnimation;
        //}
    }
}
