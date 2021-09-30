using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    [CreateAssetMenu(fileName = "AttackerData", menuName = "DChild/Gameplay/Combat/Attacker Data")]
    public class AttackerData : ScriptableObject
    {
        [SerializeField, HideLabel]
        private AttackDamageInfo m_info;
        [SerializeField]
        private GameObject m_damageFX;
        public AttackDamageInfo info => m_info;
        public GameObject damageFX => m_damageFX;
    }
}