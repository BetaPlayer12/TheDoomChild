using Holysoft.Event;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;

namespace DChild.Gameplay.Characters.Enemies
{
    public class DemonLordSummonDragon : SerializedMonoBehaviour
    {
        public enum Pattern
        {
            A,
            B,
            C,
            D,
            E,
            F,
            _COUNT
        }

        [SerializeField]
        private Animator m_animator;

        [SerializeField]
        private Dictionary<Pattern, string> m_patternToAnimatorParameterPair;

        public event EventAction<EventActionArgs> SpellEnd;
        public Animator animator => m_animator;

        public void ExecuteSpell(params Pattern[] patterns)
        {
            this.gameObject.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(SpellRoutine(patterns));
        }

        private IEnumerator SpellRoutine(params Pattern[] patterns)
        {
            for (int i = 0; i < patterns.Length; i++)
            {
                yield return SpellRoutine(patterns[i]);
            }

            SpellEnd?.Invoke(this, EventActionArgs.Empty);
            Debug.Log("Done Spell Routine");
            m_animator.SetTrigger("GoInactive");
            yield return null;
            this.gameObject.SetActive(false);
        }

        private IEnumerator SpellRoutine(Pattern pattern)
        {
            m_animator.SetTrigger("GoActive");
            m_animator.SetTrigger(m_patternToAnimatorParameterPair[pattern]);
            //
            while (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Inactive"))
            {
                yield return null; 
                Debug.Log("First While");
            }

            while (m_animator.GetCurrentAnimatorStateInfo(0).IsName("ActiveIdle") == true)
            {
                yield return null;
                Debug.Log("Second While");
            }

            while (m_animator.GetCurrentAnimatorStateInfo(0).IsName("ActiveIdle") == false)
            {
                yield return null;
                Debug.Log("Second While");
            }

            
        }

        private void Start()
        {
            //Deactivate at start so that line configurator will not keep updating
            this.gameObject.SetActive(false);
        }
    }
}