﻿using DChild;
using DChild.Gameplay.Characters;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Projectiles
{
    public class PuedisYnnusSpike : SimpleAttackProjectile
    {
        [SerializeField, TabGroup("Reference")]
        protected SpineRootAnimation m_animation;
        //[SerializeField, TabGroup("Reference")]
        //protected float m_duration;
        [SerializeField, TabGroup("Reference")]
        private SkeletonAnimation m_skeletonAnimation;

        [SerializeField, SpineSkin]
        private List<string> m_skins;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private List<string> m_startAnimations;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private List<string> m_idleAnimations;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private List<string> m_endAnimations;
        [SerializeField]
        private List<GameObject> m_hurtboxBB;

        private int m_skinID;
        private bool m_willPool;

        public enum SkinType
        {
            Small,
            Big,
        }

        //protected override void Awake()
        //{
        //    base.Awake();
        //    m_skeletonAnimation.skeleton.SetSkin();
        //}

        public void WillPool(bool willPool)
        {
            m_willPool = willPool;
        }

        public void SetSkin(SkinType skin, bool isRandom)
        {
            if (isRandom)
            {
                m_skinID = UnityEngine.Random.Range(0, m_skins.Count);
                m_skeletonAnimation.skeleton.SetSkin(m_skins[m_skinID]);
            }
            else
            {
                switch (skin)
                {
                    case SkinType.Small:
                        m_skinID = UnityEngine.Random.Range(0, 3);
                        m_skeletonAnimation.skeleton.SetSkin(m_skins[m_skinID]);
                        break;
                    case SkinType.Big:
                        m_skinID = UnityEngine.Random.Range(4, m_skins.Count);
                        m_skeletonAnimation.skeleton.SetSkin(m_skins[m_skinID]);
                        break;
                }
            }
        }

        public void LeftSpikeSpawn(float duration)
        {
            StartCoroutine(LeftSpikeRoutine(duration));
        }

        public void MassiveSpikeSpawn(float duration)
        {
            StartCoroutine(MassiveSpikeRoutine(duration));
        }

        public void RightSpikeSpawn(float duration)
        {
            StartCoroutine(RightSpikeRoutine(duration));
        }
        private IEnumerator LeftSpikeRoutine(float duration)
        {
            m_hurtboxBB[m_skinID].SetActive(true);
            m_animation.SetAnimation(0, m_startAnimations[0], false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_startAnimations[0]);
            m_animation.SetAnimation(0, m_idleAnimations[0], true);
            yield return new WaitForSeconds(duration);
            m_animation.SetAnimation(0, m_endAnimations[0], false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_endAnimations[0]);
            m_animation.SetEmptyAnimation(0, 0);
            m_hurtboxBB[m_skinID].SetActive(false);
            if (m_willPool)
                CallPoolRequest();
            else
                Destroy(this.gameObject);
            yield return null;
        }
        private IEnumerator MassiveSpikeRoutine(float duration)
        {
            m_hurtboxBB[m_skinID].SetActive(true);
            m_animation.SetAnimation(0, m_startAnimations[1], false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_startAnimations[1]);
            m_animation.SetAnimation(0, m_idleAnimations[1], true);
            yield return new WaitForSeconds(duration);
            m_animation.SetAnimation(0, m_endAnimations[1], false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_endAnimations[1]);
            m_animation.SetEmptyAnimation(0, 0);
            m_hurtboxBB[m_skinID].SetActive(false);
            if (m_willPool)
                CallPoolRequest();
            else
                Destroy(this.gameObject);
            yield return null;
        }
        private IEnumerator RightSpikeRoutine(float duration)
        {
            m_hurtboxBB[m_skinID].SetActive(true);
            m_animation.SetAnimation(0, m_startAnimations[2], false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_startAnimations[2]);
            m_animation.SetAnimation(0, m_idleAnimations[2], true);
            yield return new WaitForSeconds(duration);
            m_animation.SetAnimation(0, m_endAnimations[2], false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_endAnimations[2]);
            m_animation.SetEmptyAnimation(0, 0);
            m_hurtboxBB[m_skinID].SetActive(false);
            if (m_willPool)
                CallPoolRequest();
            else
                Destroy(this.gameObject);
            yield return null;
        }
    }
}
