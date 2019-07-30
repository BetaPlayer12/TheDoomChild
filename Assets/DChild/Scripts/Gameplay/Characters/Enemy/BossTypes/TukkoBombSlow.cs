using DChild.Gameplay.Pooling;
using Sirenix.OdinInspector;
using UnityEngine;
using DChild.Gameplay.Systems.WorldComponents;
using Spine.Unity;
using DChild.Gameplay.Characters;

namespace DChild.Gameplay
{
    public class TukkoBombSlow : FX
    {
        [SerializeField]
        private float m_lifeTime;
        private float m_currentTime;
        [SerializeField]
        private float m_slowPercentage;
        [SerializeField]
        private float m_slowDuration;
        [SerializeField, Spine.Unity.SpineAnimation]
        private string[] m_animNames;
        private SkeletonAnimation m_animation;
        private int m_animTrack;
        private float m_currentDuration;
        private Transform m_cacheTF;
        private bool m_hasSlowed;

        private void Start()
        {
            if(m_animation != null)
            {
                m_animation.state.SetAnimation(0, m_animNames[m_animTrack], false);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Debug.Log("SUCC");
            if (m_currentDuration == 0 && !m_hasSlowed && collision.transform.GetComponentInParent<IsolatedObject>() != null && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                //Debug.Log("SIOMAI RICE");
                m_cacheTF = collision.transform;
                m_hasSlowed = true;
                if (m_cacheTF.GetComponentInParent<IsolatedObject>().slowFactor == 0)
                {
                    //Debug.Log("YUUUH");
                    m_cacheTF.GetComponentInParent<IsolatedObject>().Slower(m_slowPercentage);
                }
            }
        }

        private void Awake()
        {
            m_animation = GetComponent<SkeletonAnimation>();
            m_animTrack = Random.Range(0, m_animNames.Length);
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

            if (m_hasSlowed)
            {
                if (m_currentDuration <= m_slowDuration)
                {
                    //Debug.Log("WYD FAM");
                    m_currentDuration += Time.deltaTime;
                }
                else
                {
                    //Debug.Log("WAAAAT FAM");
                    m_cacheTF.transform.GetComponentInParent<IsolatedObject>().Slower(0);
                    m_cacheTF = null;
                }
            }

            if (m_animation != null && m_animation.state.GetCurrent(0).IsComplete)
            {
                GetComponent<CircleCollider2D>().enabled = false;
            }
        }

        //CTRL + .

        public override void Play()
        {
            Debug.Log("Play EFFCTS");
        }

        public override void SetFacing(HorizontalDirection horizontalDirection)
        {
            throw new System.NotImplementedException();
        }

        public override void Stop()
        {
            throw new System.NotImplementedException();
        }

        public override void Pause()
        {
            throw new System.NotImplementedException();
        }
    }
}