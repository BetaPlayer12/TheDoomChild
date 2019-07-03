using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Characters.Enemies;
using DChild;

public class TombAttack : MonoBehaviour
{
    [SerializeField]
    private GameObject m_soul;
    [SerializeField]
    private float m_lifeTime;
    private TombAttackAnimation m_animation;
    private Vector2 m_target;

    private void Awake()
    {
        m_animation = GetComponent<TombAttackAnimation>();
    }

    private void Start()
    {
        //m_animation.SetEmptyAnimation(0, 0);
        int num = Random.Range(0, 3);
        m_animation.DoTombRise(num);
        StartCoroutine(SummonSoul(num));
        StartCoroutine(TombLife());
    }

    public void GetTarget(Vector2 target)
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

        var target = m_target;
        Vector2 spitPos = transform.position;
        Vector3 v_diff = (target - spitPos);
        float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);

        GameObject soul = Instantiate(m_soul, transform.position, Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg));

        //GameObject soul = Instantiate(m_soul, transform.position, Quaternion.identity);
        soul.GetComponent<TombSoul>().GetTarget(m_target);
        Debug.Log("Tomb Target: " + Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg));
        yield return null;
    }

    private IEnumerator TombLife()
    {
        yield return new WaitForSeconds(m_lifeTime);
        yield return null;
        Destroy(this.gameObject);
    }
}
