using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [CreateAssetMenu(fileName = "ArmyBattleLocationData", menuName = "DChild/Gameplay/Army/System/Location Data")]
    public class ArmyBattleLocationData : SerializedScriptableObject
    {
        [SerializeField]
        private Dictionary<ArmyBattleLocation, GameObject> m_locationInstancePair;

        public GameObject GetLocationInstance(ArmyBattleLocation location)
        {
            m_locationInstancePair.TryGetValue(location, out var instance);
            return instance;
        }
    }
}