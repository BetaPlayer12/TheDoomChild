using DChild.Gameplay.Environment;
using UnityEngine;
using static DChild.Gameplay.Characters.Players.PlayerModuleActivator;

namespace DChild.Gameplay.Characters.Players
{
    [System.Serializable]
    public class PlayerPassiveModule
    {
        [SerializeField]
        private BlackBloodImmunity m_blackBloodImmunity;

        public void SetModuleActive(PrimarySkill module, bool isActive)
        {
            switch (module)
            {
                case PrimarySkill.BlackBloodImmunity:
                    m_blackBloodImmunity.EnableImmunity(isActive);
                    break;
                default:
                    break;
            }
        }
    }
}