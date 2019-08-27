using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public class PlayerControlledObject : MonoBehaviour, IController
    {
        private IPlayer m_owner;

        public IPlayer owner { get => m_owner; }

        public void Disable()
        {
            m_owner.controller.Disable();
        }

        public void Enable()
        {
            m_owner.controller.Enable();
        }

        public void SetOwner(IPlayer player) => m_owner = player;
    }
}