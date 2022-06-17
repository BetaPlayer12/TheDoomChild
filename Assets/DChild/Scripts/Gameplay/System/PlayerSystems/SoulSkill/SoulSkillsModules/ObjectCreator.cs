using UnityEngine;
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

            private bool m_bindToPlayer;

            public Handle(Character m_playerCharacter, GameObject m_instance, bool bindToPlayer) : base(null)
            {
                this.m_playerCharacter = m_playerCharacter;
                this.m_instanceReference = m_instance;
                m_bindToPlayer = bindToPlayer;
            }

            public override void Dispose()
            {
                Object.Destroy(m_instance);
            }

            public override void Initialize()
            {
                m_instance = Object.Instantiate(m_instanceReference);
                if (m_bindToPlayer)
                {
                    m_instance.transform.SetParent(m_playerCharacter.transform);
                }
                m_instance.transform.localPosition = Vector3.zero;
            }
        }

        [SerializeField]
        private GameObject m_toCreate;
        [SerializeField]
        private bool m_bindToPlayer;

        protected override BaseHandle CreateHandle(IPlayer player)
        {
            return new Handle(player.character, m_toCreate, m_bindToPlayer);
        }
    }
}