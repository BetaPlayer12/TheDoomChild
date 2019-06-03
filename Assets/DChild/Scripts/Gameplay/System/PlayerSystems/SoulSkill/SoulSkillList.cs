using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    [CreateAssetMenu(fileName = "SoulSkillList", menuName = "DChild/Database/Soul Skill List")]
    public class SoulSkillList : DatabaseAssetList<SoulSkill>
    {

    }
}