using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Characters.Players;
using Holysoft.Event;


namespace PlayerNew
{
    public class Death : PlayerBehaviour, IComplexCharacterModule
    {
        public InputState testState;
        public Damageable m_source;
        public int currentHP;
        public Animator m_animator;
        private string m_deathParameter;

        // Start is called before the first frame update
        void Start()
        {
            m_animator = this.gameObject.GetComponent<Animator>();                          
        }



        // Update is called once per frame
        private void Update()
        {
            currentHP = m_source.health.currentValue;
            if(currentHP <= 0)
            {
                OnDeath(this, EventActionArgs.Empty);
                ToggleScripts(false);
            }

        }


        public void Initialize(ComplexCharacterInfo info)
        {
            m_source = info.damageable;
            m_source.Destroyed += OnDeath;
            m_animator = info.animator;
            m_deathParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.Death);
        }

        private void OnDeath(object sender, EventActionArgs eventArgs)
        {

            Debug.Log("Dead");
            m_source.SetHitboxActive(false);
            m_animator.SetBool("Death", true);

        }
    }
}
