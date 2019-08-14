#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public struct StatusImmunity : ISoulSkillModule
    {
        //[SerializeField]
        //private StatusEffectType m_type;

        public void AttachTo(IPlayer player)
        {
           // player.statusResistance.SetResistance(m_type, StatusResistanceType.Immune);
        }

        public void DetachFrom(IPlayer player)
        {
           // player.statusResistance.SetResistance(m_type, StatusResistanceType.None);
        }
    }
}