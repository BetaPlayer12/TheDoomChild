using DChild.Gameplay;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug.Window
{
    public class ModifyDamage : MonoBehaviour
    {
        [SerializeField]
        public float m_damage = 999999;
        [Button]
        public void AddDamage()
        {
            GameplaySystem.playerManager.player.modifiers.Add(DChild.Gameplay.Characters.Players.PlayerModifier.AttackDamage, m_damage);

        }
        [Button]
        public void ReduceDamage()
        {
            GameplaySystem.playerManager.player.modifiers.Add(DChild.Gameplay.Characters.Players.PlayerModifier.AttackDamage, -m_damage);

        }
    }
}
