using System;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChildDebug.Gameplay.Characters.AI
{
    public class HoodlessAggroSensor : AggroSensor
    {
        private IHoodless m_hoodlessTarget;
        private IEnemyTarget m_target;

        private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
        {
            var target = collision.GetComponentInParent<IEnemyTarget>();
            if (target != null)
            {
                var hoodlessCharacter = collision.GetComponentInParent<IHoodless>();
                if (hoodlessCharacter == null)
                {
                    m_brain.SetTarget(target);
                    m_target = target;
                }
                else if (hoodlessCharacter.isHoodless == false)
                {
                    m_brain.SetTarget(target);
                    m_hoodlessTarget = hoodlessCharacter;
                    m_hoodlessTarget.HoodlessToggled += OnHoodlessToggled;
                    m_target = target;
                }
                else
                {
                    m_hoodlessTarget = hoodlessCharacter;
                    m_hoodlessTarget.HoodlessToggled += OnHoodlessToggled;
                    m_target = target;
                }
            }
        }

        private void OnHoodlessToggled(object sender, IHoodless eventArgs)
        {
            Debug.Log("Hoodless Toggled Found");
            if (m_hoodlessTarget == eventArgs)
            {
                if (m_hoodlessTarget.isHoodless)
                {
                    m_brain.SetTarget(null);
                }
                else
                {
                    m_brain.SetTarget(m_target);
                }
            }
            else
            {
                eventArgs.HoodlessToggled -= OnHoodlessToggled;
            }
        }
    }
}
