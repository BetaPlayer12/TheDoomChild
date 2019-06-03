using System.Collections.Generic;
using Holysoft.Collections;
using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Environment.Obstacles
{
    public class BlackBloodPuddle : BlackBlood
    {
        [SerializeField]
        private GameObject m_smokeFX;

        protected override void Damage(ITarget toDamage, AttackInfo info)
        {
            base.Damage(toDamage, info);
            GameplaySystem.fXManager.InstantiateFX(m_smokeFX, toDamage.position);
        }
    }
}