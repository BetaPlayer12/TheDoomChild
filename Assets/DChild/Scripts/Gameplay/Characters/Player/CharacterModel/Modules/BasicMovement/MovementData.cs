using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    [CreateAssetMenu(fileName = "MovementData", menuName = "DChild/Player/Movement Data")]
    public class MovementData : ScriptableObject
    {
        [SerializeField,HideLabel]
        private MovementInfo m_info;

        public MovementInfo info => m_info;
    }

}
