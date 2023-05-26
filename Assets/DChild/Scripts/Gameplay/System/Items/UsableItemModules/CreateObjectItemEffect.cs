using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Combat.StatusAilment;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Items
{
    [System.Serializable]

    public struct CreateObjectItemEffect : IUsableItemModule
    {
        [SerializeField]
        private GameObject m_toCreate;

        public bool CanBeUse(IPlayer player)
        {
            return true;
        }

        public void Use(IPlayer player)
        {
            m_toCreate = Object.Instantiate(m_toCreate);
            m_toCreate.transform.SetParent(player.character.transform);
            m_toCreate.transform.localPosition = Vector3.zero;
        }
    }
}
