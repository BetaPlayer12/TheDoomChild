﻿using Holysoft.Event;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay;
using DChild.Gameplay.Combat;
using UnityEngine;



namespace DChild.Gameplay.Characters.Players.Modules
{
  
    public class PlayerDeath : MonoBehaviour, IComplexCharacterModule
    {
     
        

        public Damageable m_source;
        [SerializeField]
        private Animator m_animator;
        private string m_deathParameter;

        public int hp;


        void Update()
        {
            hp = m_source.health.currentValue;

            if(hp <= 0f)
            {
                OnDeath(this, EventActionArgs.Empty);
                
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