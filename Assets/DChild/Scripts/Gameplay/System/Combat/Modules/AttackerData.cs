using Sirenix.OdinInspector;
using UnityEngine;

namespace Refactor.DChild.Gameplay.Combat
{
    [CreateAssetMenu(fileName = "AttackerData", menuName = "DChild/Gameplay/Combat/Attacker Data")]
    public class AttackerData : ScriptableObject
    {
        [SerializeField, HideLabel]
        private AttackerInfo m_info;

        public AttackerInfo info => m_info;
    }
}