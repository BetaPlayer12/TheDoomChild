using System.Collections;
using UnityEngine;
using DChild.Gameplay.Characters.AI;
using DGameplay = DChild.Gameplay;
using Holysoft.Event;
using SUnity = Spine.Unity;
using Spine.Unity;
using Spine;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Projectiles;
//using System;

public class TombAttack : MonoBehaviour
{
    [System.Serializable]
    private class AnimationInfo
    {
        [SerializeField, SUnity.SpineAnimation]
        private string m_risingAnimation;
        [SerializeField, SUnity.SpineAnimation]
        private string m_hidingAnimation;

        public void PlayRising(SpineRootAnimation animation)
        {
            animation.SetAnimation(0, m_risingAnimation, false);
        }

        public void PlayHiding(SpineRootAnimation animation)
        {
            animation.SetAnimation(0, m_hidingAnimation, false);
        }
    }

    [SerializeField]
    private GameObject m_soul;
    [SerializeField]
    private float m_lifeTime;
    [SerializeField]
    private AnimationInfo[] m_spineAnims;
    [SerializeField]
    private SpineRootAnimation m_animation;
    [SerializeField]
    private SkeletonAnimation m_skeletonAnimation;
    private AITargetInfo m_target;
    [SerializeField]
    private DGameplay.ParticleFX m_tombFX;

    private float m_volleys;
    private float m_delay;
    public float delay => m_delay;

    public event EventAction<EventActionArgs> TombStart;
    public event EventAction<EventActionArgs> TombEnd;
    private AnimationInfo m_currentAnimation;

    private void Start()
    {
        m_currentAnimation = m_spineAnims[UnityEngine.Random.Range(0, 2/*m_spineAnims.Length*/)];
        //m_animation.SetAnimation(0, m_currentAnimation, false);
        m_currentAnimation.PlayRising(m_animation);
        TombStart?.Invoke(this, new EventActionArgs());
        m_skeletonAnimation.state.Complete += SoulSummon;
    }

    private void SoulSummon(TrackEntry trackEntry)
    {
        StartCoroutine(SummonSoul());
    }

    private void TombBurrow(TrackEntry trackEntry)
    {
        TombEnd?.Invoke(this, new EventActionArgs());
        Destroy(this.gameObject);
    }

    private void Impacted(object sender, EventActionArgs eventArgs)
    {
        m_volleys--;
        if(m_volleys == 0)
        {
            m_skeletonAnimation.state.Complete -= SoulSummon;
            m_skeletonAnimation.state.Complete += TombBurrow;
            m_tombFX.Stop();
            m_currentAnimation.PlayHiding(m_animation);
        }
        else
        {
            StartCoroutine(SummonSoul());
        }
    }

    public void GetTarget(AITargetInfo target, int volleys, float delay)
    {
        m_target = target;
        m_volleys = volleys;
        m_delay = delay;
    }

    private IEnumerator SummonSoul()
    {
        //yield return new WaitForSeconds(m_delay);
        GameObject soul = Instantiate(m_soul, new Vector2(transform.position.x, transform.position.y + 2), Quaternion.identity);
        soul.GetComponent<TombSoul>().GetTarget(m_target);
        soul.transform.SetParent(this.transform);
        soul.GetComponent<Projectile>().Impacted += Impacted;

        yield return null;
    }
}
