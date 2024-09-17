using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class ArmyBattleLocationInstantiator : MonoBehaviour
    {
        [SerializeField]
        private ArmyBattleLocationData m_data;

        private GameObject m_locationInstance;

        [Button]
        public void InstantiateLocation(ArmyBattleLocation location)
        {
            var template = m_data.GetLocationInstance(location);

            if (m_locationInstance != null)
            {
                Destroy(m_locationInstance);
            }

            if (template != null)
            {
                m_locationInstance = Instantiate(template);
                m_locationInstance.transform.position = Vector3.zero;
            }
            else
            {
                Debug.LogError($"Location {location} has no Instance", this);
            }
        }
    }
}