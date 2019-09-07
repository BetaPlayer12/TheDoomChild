using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Characters.Enemies;
using DChild;
using DChild.Gameplay.Characters.AI;

public class TombAttack : MonoBehaviour
{
    [SerializeField]
    private GameObject m_soul;
    [SerializeField]
    private float m_lifeTime;
    private TombAttackAnimation m_animation;
    private AITargetInfo m_target;

    //Volleys Test
    private float m_volleys;

    private void Awake()
    {
        m_animation = GetComponent<TombAttackAnimation>();
    }

    private void Start()
    {
        //m_animation.SetEmptyAnimation(0, 0);
        m_volleys *= 2;
        int num = Random.Range(0, 2);
        m_animation.DoTombRise(num);
        StartCoroutine(SummonSoulIntro(num));
    }

    public void GetTarget(AITargetInfo target, int volleys)
    {
        m_target = target;
        m_volleys = volleys;
    }

    private IEnumerator SummonSoulIntro(int num)
    {
        if (num == 0)
        {
            yield return new WaitForAnimationComplete(m_animation.animationState, TombAttackAnimation.ANIMATION_TOMBA_RISE);
        }
        else if (num == 1)
        {
            yield return new WaitForAnimationComplete(m_animation.animationState, TombAttackAnimation.ANIMATION_TOMBB_RISE);
        }
        else if (num == 2)
        {
            yield return new WaitForAnimationComplete(m_animation.animationState, TombAttackAnimation.ANIMATION_TOMBC_RISE);
        }
        yield return null;
        StartCoroutine(SummonSoul());
    }

    private IEnumerator SummonSoul()
    {
        
        GameObject soul = Instantiate(m_soul, new Vector2(transform.position.x, transform.position.y+2), Quaternion.identity);
        soul.GetComponent<TombSoul>().GetTarget(m_target);
        soul.transform.SetParent(this.transform);
        StartCoroutine(TombLife(soul.name));
        yield return null;
    }

    private IEnumerator TombLife(string name)
    {
        yield return new WaitUntil(() => !transform.Find(name).gameObject.activeInHierarchy);
        Destroy(transform.Find(name).gameObject);
        m_volleys--;
        if (m_volleys == 0)
        {
            Destroy(this.gameObject);
        }
        else
        {
            StartCoroutine(SummonSoul());
        }
        yield return null;
        Debug.Log("Explod Soul");
    }
}
