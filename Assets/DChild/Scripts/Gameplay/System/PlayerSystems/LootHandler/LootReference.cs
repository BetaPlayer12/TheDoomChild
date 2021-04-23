using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    [CreateAssetMenu(fileName = "LootReference", menuName = "DChild/Gameplay/Loot/Loot Reference")]
    public class LootReference : ScriptableObject
    {
        [SerializeField,ValidateInput("ValidateLoot","GameObject must have Loot component")]
        private GameObject m_loot;

        public GameObject loot => m_loot;

#if UNITY_EDITOR
        public void Initialize(GameObject loot) => m_loot = loot;

        private bool ValidateLoot(GameObject loot)
        {
            return loot?.GetComponent<Loot>() ?? false;
        }
#endif

    }
}