using DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace Refactor.DChild.Gameplay.Characters.Players
{
    public class PlayerControlledObject : MonoBehaviour
    {
        private IPlayer m_owner;

        public IPlayer owner { get => m_owner;}

        public void SetOwner(IPlayer player) => m_owner = player;
    }
}