using Holysoft.Event;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;

namespace DChild.Gameplay.Characters.Enemies
{
    public class DemonLordLightningConstellation : SerializedMonoBehaviour
    {
        public enum Type
        {

            Leo,
            Aries,
            Taurus,
            Cancer,
            Scorpio,
            _COUNT
        }

        [SerializeField]
        private Animator m_animator;
       
        [SerializeField]
        private Dictionary<Type, string> m_typeToAnimatorParameterPair;

        private LightningConstellationColliderConfigurator[] m_lightningConfigurators;

        public event EventAction<EventActionArgs> SpellEnd;
        public Animator animator => m_animator;
        public void ExecuteSpell(Type type)
        {
            this.gameObject.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(SpellRoutine(type));
        }

        private IEnumerator SpellRoutine(Type type)
        {
            m_animator.SetTrigger(m_typeToAnimatorParameterPair[type]);

            ////Wait for Animator to actually do the Animation;
            //yield return new WaitForSeconds(1f);

            //for (int i = 0; i < m_lightningConfigurators.Length; i++)
            //{
            //    m_lightningConfigurators[i].ReorientCollider();
            //}

            while (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                yield return null;
            }


            SpellEnd?.Invoke(this, EventActionArgs.Empty);
            this.gameObject.SetActive(false);
        }

        private void Awake()
        {
            m_lightningConfigurators = GetComponentsInChildren<LightningConstellationColliderConfigurator>();
        }

        private void Start()
        {
            //Deactivate at start so that line configurator will not keep updating
            this.gameObject.SetActive(false);
        }
    }
}