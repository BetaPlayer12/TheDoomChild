using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.SoulSkills;
using System.Collections.Generic;

namespace DChild.Gameplay.Items
{
    struct ModuleShadowSkillRemover : IDurationItemEffect
    {
        public void StartEffect(IPlayer player)
        {
            List<PrimarySkill> shadowSkills = new List<PrimarySkill>();

            shadowSkills.Add(PrimarySkill.ShadowMorph);
            shadowSkills.Add(PrimarySkill.ShadowSlide);
            shadowSkills.Add(PrimarySkill.ShadowDash);

            foreach(PrimarySkill skill in shadowSkills)
            {
                player.skills.SetSkillStatus(skill, false);
            }
        }

        public void StopEffect(IPlayer player)
        {
            List<PrimarySkill> shadowSkills = new List<PrimarySkill>();

            shadowSkills.Add(PrimarySkill.ShadowMorph);
            shadowSkills.Add(PrimarySkill.ShadowSlide);
            shadowSkills.Add(PrimarySkill.ShadowDash);

            foreach (PrimarySkill skill in shadowSkills)
            {
                player.skills.SetSkillStatus(skill, true);
            }
        }
    }
    public interface IDurationItemEffect
    {
        void StartEffect(IPlayer player);
        void StopEffect(IPlayer player);
    }
}
