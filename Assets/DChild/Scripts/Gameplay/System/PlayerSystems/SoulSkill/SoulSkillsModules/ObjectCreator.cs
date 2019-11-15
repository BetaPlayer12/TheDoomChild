﻿using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public class ObjectCreator : HandledSoulSkillModule
    {
        private class Handle : BaseHandle
        {
            private Character m_playerCharacter;
            private GameObject m_instanceReference;
            private GameObject m_instance;

            public Handle(Character m_playerCharacter, GameObject m_instance)
            {
                this.m_playerCharacter = m_playerCharacter;
                this.m_instanceReference = m_instance;
            }

            public override void Dispose()
            {
                Object.Destroy(m_instance);
            }

            public override void Initialize()
            {
                m_instance = Object.Instantiate(m_instanceReference);
                m_instance.transform.SetParent(m_playerCharacter.transform);
                m_instance.transform.localPosition = Vector3.zero;
            }
        }

        [SerializeField]
        private GameObject m_toCreate;

        protected override BaseHandle CreateHandle(IPlayer player)
        {
            return new Handle(player.character, m_toCreate);
        }
    }
}