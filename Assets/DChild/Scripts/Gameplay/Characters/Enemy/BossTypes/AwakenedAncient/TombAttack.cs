using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Characters.Enemies;
using DChild;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay;

public class TombAttack : MonoBehaviour
{
    [SerializeField]
    private GameObject m_soul;
    [SerializeField]
    private float m_lifeTime;
    private TombAttackAnimation m_animation;
    private AITargetInfo m_target;
    [SerializeField]
    private ParticleFX m_tombFX;

    //Volleys Test
    private float m_volleys;
    private int m_tombSkin;
    private GameObject m_boss;
    private float m_delay;

    private void Awake()
    {
        m_animation = GetComponent<TombAttackAnimation>();
    }

    private void Start()
    {
        //m_animation.SetEmptyAnimation(0, 0);
        m_volleys *= 2;
        m_tombSkin = Random.Range(0, 2);
        m_animation.DoTombRise(m_tombSkin);
        StartCoroutine(SummonSoulIntro());
    }

    public void GetTarget(AITargetInfo target, int volleys, GameObject boss, float delay)
    {
        m_target = target;
        m_volleys = volleys;
        m_boss = boss;
        m_delay = delay;
    }

    private IEnumerator SummonSoulIntro()
    {
        if (m_tombSkin == 0)
        {
            yield return new WaitForAnimationComplete(m_animation.animationState, TombAttackAnimation.ANIMATION_TOMBA_RISE);
        }
        else if (m_tombSkin == 1)
        {
            yield return new WaitForAnimationComplete(m_animation.animationState, TombAttackAnimation.ANIMATION_TOMBB_RISE);
        }
        else if (m_tombSkin == 2)
        {
            yield return new WaitForAnimationComplete(m_animation.animationState, TombAttackAnimation.ANIMATION_TOMBC_RISE);
        }
        yield return null;
        StartCoroutine(SummonSoul());
    }

    private IEnumerator SummonSoul()
    {
        //yield return new WaitForSeconds(m_delay);
        GameObject soul = Instantiate(m_soul, new Vector2(transform.position.x, transform.position.y + 2), Quaternion.identity);
        soul.GetComponent<TombSoul>().GetTarget(m_target);
        soul.transform.SetParent(this.transform);
        m_boss.GetComponent<AwakenedAncientAI>().AddSoul(soul);
        StartCoroutine(TombLife(soul.name));
        yield return null;
    }
    private IEnumerator TombDeathRoutine()
    {
        m_tombFX.Stop();
        m_animation.DoTombBurrow(m_tombSkin);
        if (m_tombSkin == 0)
        {
            yield return new WaitForAnimationComplete(m_animation.animationState, TombAttackAnimation.ANIMATION_TOMBA_BURROW);
        }
        else if (m_tombSkin == 1)
        {
            yield return new WaitForAnimationComplete(m_animation.animationState, TombAttackAnimation.ANIMATION_TOMBB_BURROW);
        }
        else if (m_tombSkin == 2)
        {
            yield return new WaitForAnimationComplete(m_animation.animationState, TombAttackAnimation.ANIMATION_TOMBC_BURROW);
        }
        yield return null;
        Destroy(this.gameObject);
    }

    private IEnumerator TombLife(string name)
    {
        yield return new WaitUntil(() => !transform.Find(name).gameObject.activeSelf);
        Destroy(transform.Find(name).gameObject);
        m_volleys--;
        if (m_volleys == 0)
        {
            StartCoroutine(TombDeathRoutine());
        }
        else
        {
            StartCoroutine(SummonSoul());
        }
        yield return null;
        Debug.Log("Explod Soul");
    }
}
