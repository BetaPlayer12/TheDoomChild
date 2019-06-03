using DChild.Gameplay.Characters.Players.Equipments;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    [System.Serializable, Title("Equipments"),HideLabel]
    public class Equipment : IEquipment
    {
        [SerializeField, HideLabel, Indent]
        private Armor m_armor;
        [SerializeField, HideLabel, Indent]
        private Weapon m_weapon;

        public IArmor armor => m_armor;
        public IWeapon weapon => m_weapon;
    }
}