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

    private void Awake()
    {
        m_animation = GetComponent<TombAttackAnimation>();
    }

    private void Start()
    {
        //m_animation.SetEmptyAnimation(0, 0);
        int num = Random.Range(0, 2);
        m_animation.DoTombRise(num);
        StartCoroutine(SummonSoul(num));
        StartCoroutine(TombLife());
    }

    public void GetTarget(AITargetInfo target)
    {
        m_target = target;
    }

    private IEnumerator SummonSoul(int num)
    {
        if(num == 0)
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
        
        GameObject soul = Instantiate(m_soul, new Vector2(transform.position.x, transform.position.y+2), Quaternion.identity);
        soul.GetComponent<TombSoul>().GetTarget(m_target);
        yield return null;
    }

    private IEnumerator TombLife()
    {
        yield return new WaitForSeconds(m_lifeTime);
        yield return null;
        Destroy(this.gameObject);
    }
}
