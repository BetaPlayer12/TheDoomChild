using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using Spine.Unity;
using System;

namespace DChild.Gameplay.Characters.Enemies
{
    public class PuedisYnnusFleshSpikeBomb : MonoBehaviour
    {
        [SerializeField]
        private SpineAnimation m_animation;

        [SerializeField]
        private HeartBeatHandle m_source;

        [SerializeField, Spine.Unity.SpineAnimation, TabGroup("Summon")]
        private string m_summonAnimation;
        [SerializeField, Spine.Unity.SpineAnimation, TabGroup("Summon")]
        private string m_idleAnimation;

        [SerializeField, TabGroup("Growing")]
        private Vector3 m_startingScale;
        [SerializeField, TabGroup("Growing")]
        private Vector3[] m_growScale;

        [SerializeField, Spine.Unity.SpineAnimation, TabGroup("Explosion")]
        private string m_explodeAnimation;
        [SerializeField, Spine.Unity.SpineEvent, TabGroup("Explosion")]
        private string m_explodeEvent;
        [SerializeField, TabGroup("Explosion")]
        private PuedisYnnusFleshSpikeProjectileHandle m_projectileHandle;

        
        public IEnumerator BeSummoned()
        {
            transform.localScale = m_startingScale;
            gameObject.SetActive(true);
            m_animation.SetAnimation(0, m_summonAnimation, false);
            var idleTrack = m_animation.AddAnimation(0, m_idleAnimation, true, 0);
            yield return new WaitForSpineAnimation(idleTrack, WaitForSpineAnimation.AnimationEventTypes.Start);
        }

        public IEnumerator ExplodeRoutine()
        {
            var heartBeat = new WaitForHeartBeat(m_source, 1);
            for (int i = 0; i < m_growScale.Length; i++)
            {
                yield return heartBeat;
                transform.localScale = m_growScale[i];
            }
            yield return heartBeat;
            yield return new WaitForHeartBeat(m_source, 2);
            var explosionTrack = m_animation.SetAnimation(0, m_explodeAnimation, false);
            yield return new WaitForSpineEvent(m_animation.skeletonAnimation, m_explodeEvent);
            m_projectileHandle.SpawnProjectiles();
            yield return new WaitForSpineAnimationComplete(explosionTrack);
        }


        [Button]
        private void ExecuteBeSummonedRoutine()
        {
            gameObject.SetActive(true);
            StopAllCoroutines();     
            StartCoroutine(BeSummoned());
        }

        [Button]
        private void ExecuteExplodeRoutine()
        {
            gameObject.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(ExplodeRoutine());
        }

        private void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}