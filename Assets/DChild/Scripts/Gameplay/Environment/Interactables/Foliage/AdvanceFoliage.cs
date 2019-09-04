using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Physics;
using DChild.Gameplay.Systems;
using DChild.Gameplay.Systems.WorldComponents;
using UnityEngine;

namespace DChild.Gameplay.Environment
{

    public class AdvanceFoliage : MonoBehaviour, IForceReactant, IInteractiveEnvironment
    {
        private IFoliage[] m_foliage;

        private bool m_canCollideAgain;
        private bool m_reverse;
        private bool m_isReturning;

        public void React(Vector2 origin, Vector2 force)
        {
            throw new System.NotImplementedException();
        }

        public void ResetState()
        {
            GameplaySystem.world.Unregister(this);
            for (int i = 0; i < m_foliage.Length; i++)
            {
                m_foliage[i].SetToDefault();
            }
        }

        public void UpdateState()
        {
            for (int i = 0; i < m_foliage.Length; i++)
            {   
                m_foliage[i].GetPropertyBlock();
                if (m_foliage[i].Reverse)
                {
                    m_foliage[i].ReverseVertices();            
                }
                else
                {
                   
                    m_foliage[i].ReturnToDefault();
                }
                m_foliage[i].ApplyChanges();
            }
        }

        private void Update()
        {
            UpdateState();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var player = collision.transform.gameObject.GetComponentInParent<IPlayer>();
            var monster = collision.transform.gameObject.GetComponentInParent<GrassMonster>();

            if ( player != null || monster != null)
            {
                for (int i = 0; i < m_foliage.Length; i++)
                {       
                    if (m_foliage[i].CanCollideAgain)
                    {
                        m_foliage[i].CanCollideAgain = false;
                        if (collision.transform.position.x < transform.position.x)
                        {
                            m_foliage[i].Reverse = true;
                        }
                        else
                        {
                            if (!m_foliage[i].IsReturning)
                                m_foliage[i].IsReturning = true;
                        }
                    }
                }
            }
        }

        private void Awake()
        {
            m_foliage = GetComponentsInChildren<IFoliage>();

            for (int i = 0; i < m_foliage.Length; i++)
            {
                m_foliage[i].Initialized();
                m_foliage[i].CanCollideAgain = true;
                m_foliage[i].SetToDefault();
            }
        }
    }
}

