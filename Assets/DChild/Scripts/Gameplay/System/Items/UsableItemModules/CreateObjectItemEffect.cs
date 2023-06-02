using System;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Combat.StatusAilment;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;
using DChild.Gameplay.Items;

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
        private int magic;

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
            magic = player.magic.maxValue;
            int tempmagic= magic - m_shadowgaugereduction;
            player.magic.SetMaxValue(tempmagic);
            if (m_instanceReference.GetComponent<AttackProjectile>() != null)
            {
                //var instanceProjectile = m_instanceReference.GetComponent<Projectile>();
                var instanceProjectile = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(m_instanceReference);
                m_instance = instanceProjectile.gameObject;
                m_instance.transform.SetParent(player.character.transform);
                m_instance.transform.position = player.character.centerMass.position;
                m_instance.GetComponent<Attacker>().SetParentAttacker(player.character.GetComponent<Attacker>());
                ShadowPetHandler listener = m_instance.GetComponent<ShadowPetHandler>();
                listener.Desummoning += Desummoning;
            }
            else
            {
                m_instance = GameObject.Instantiate(m_instanceReference);
                m_instance.transform.SetParent(player.character.transform);
                m_instance.transform.localPosition = Vector3.zero;
            }
        }

        private void Desummoning(object sender, EventActionArgs eventArgs)
        {
            RestoreShadow();
        }

        public void RestoreShadow()
        {

            GameplaySystem.playerManager.player.magic.SetMaxValue(magic);
        }
    }
}
