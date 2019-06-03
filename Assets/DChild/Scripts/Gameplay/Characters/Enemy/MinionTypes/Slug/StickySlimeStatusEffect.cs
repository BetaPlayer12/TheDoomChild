using DChild.Gameplay.Pooling;
using Sirenix.OdinInspector;
using UnityEngine;
using DChild.Gameplay.Systems.WorldComponents;
using Spine.Unity;

public class StickySlimeStatusEffect : MonoBehaviour
{
    [SerializeField]
    private float m_lifeTime;
    private float m_currentTime;
    [SerializeField]
    private float m_slowPercentage;
    [SerializeField]
    private float m_slowDuration;
    private float m_currentDuration;
    private Transform m_cacheTF;

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("SUCC");
        if (m_cacheTF == null && collision.transform.GetComponentInParent<IsolatedObject>() != null && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //Debug.Log("SIOMAI RICE");
            m_cacheTF = collision.transform;
            if (m_cacheTF.GetComponentInParent<IsolatedObject>().slowFactor == 0)
            {
                //Debug.Log("YUUUH");
                m_cacheTF.GetComponentInParent<IsolatedObject>().Slower(m_slowPercentage);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (m_cacheTF != null)
        {
            m_cacheTF.GetComponentInParent<IsolatedObject>().Slower(0);
            m_cacheTF = null;
        }
    }

    private void Update()
    {
        if (m_currentTime <= m_lifeTime)
        {
            m_currentTime += Time.deltaTime;
        }
        else
        {
            if (m_cacheTF == null)
            {
                Destroy(transform.gameObject);
            }
        }
    }
}
