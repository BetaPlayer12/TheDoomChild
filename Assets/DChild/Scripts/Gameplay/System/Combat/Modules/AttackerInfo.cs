using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    [System.Serializable]
    public class AttackerInfo
    {
        public AttackerInfo()
        {
            damage = new List<AttackDamage>();
            critChance = 0;
            critDamageModifier = 1;
            ignoreInvulnerability = Invulnerability.None;
            ignoresBlock = false;
        }

        public List<AttackDamage> damage;
        [Range(0, 100)]
        public int critChance;
        [MinValue(0), ShowIf("CanCrit")]
        public int critDamageModifier = 1;
        public Invulnerability ignoreInvulnerability;
        public bool ignoresBlock;

        public void Copy(AttackerInfo source)
        {
            damage.Clear();
            damage.AddRange(source.damage);
            critChance = source.critChance;
            critDamageModifier = source.critDamageModifier;
            ignoreInvulnerability = source.ignoreInvulnerability;
            ignoresBlock = source.ignoresBlock;
        }

#if UNITY_EDITOR
        private bool CanCrit() => critChance != 0;
#endif
    }
}