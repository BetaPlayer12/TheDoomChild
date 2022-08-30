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
using DChild.Gameplay.Environment;

namespace DChild.Gameplay.Characters.Enemies
{
    public class VisualShowcaseAI : MonoBehaviour
    {
        private List<string> m_animations;
        private SpineRootAnimation m_animation;

        private void Awake()
        {
            m_animation = GetComponent<SpineRootAnimation>();
            m_animations = new List<string>();
        }

        private void Start()
        {
            var animList = GetComponentInChildren<SkeletonAnimation>().skeleton.Data.Animations.ToArray();
            for (int i = 0; i < animList.Length; i++)
            {
                Debug.Log("Animation is " + animList[i].ToString());
                m_animations.Add(animList[i].ToString());
            }

            StartCoroutine(SpamAnimationRoutine());
        }

        private IEnumerator SpamAnimationRoutine()
        {
            while (true)
            {
                for (int i = 0; i < m_animations.Count; i++)
                {
                    m_animation.SetAnimation(0, m_animations[i], false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_animations[i]);
                }
                yield return null;
            }
        }
    }
}
