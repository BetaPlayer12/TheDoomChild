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
        [SerializeField]
        private int m_shadowgaugereduction;
        private GameObject m_instanceReference;
        private GameObject m_instance;

        public bool CanBeUse(IPlayer player)
        {
            int m_currentmagic = player.magic.currentValue;
            if (m_currentmagic >= m_shadowgaugereduction)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public void Use(IPlayer player)
        {
            this.m_instanceReference = m_toCreate;
            player.magic.ReduceCurrentValue(m_shadowgaugereduction);
            int magic = player.magic.maxValue;
            magic= magic - m_shadowgaugereduction;
            player.magic.SetMaxValue(magic);
            m_instance = Object.Instantiate(m_instanceReference);
            m_instance.transform.SetParent(player.character.transform);
            m_instance.transform.localPosition = Vector3.zero;
        }
    }
}
