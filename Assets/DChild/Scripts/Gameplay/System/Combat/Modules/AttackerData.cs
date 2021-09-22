using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    [CreateAssetMenu(fileName = "AttackerData", menuName = "DChild/Gameplay/Combat/Attacker Data")]
    public class AttackerData : ScriptableObject
    {
        [SerializeField, HideLabel]
        private AttackInfo m_info;

        public AttackInfo info => m_info;
    }
}