#if UNITY_EDITOR
using DChild;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.SoulSkills;
using Sirenix.OdinInspector;
using System.Collections;

namespace DChildDebug
{

    public class SoulSkillInjector : SerializedMonoBehaviour
    {
        //[HideInPlayMode]
        //public IPlayer m_player;
        //[HideInPlayMode]
        ////public SoulSkillDatabase m_factory;

        //[HideInEditorMode, ValueDropdown("GetSkills"), LabelText("Skill Name")]
        //public int skillID;

        //[HideInEditorMode, Button]
        //private void Inject()
        //{
        //    var skill = m_factory.CreateInstance(skillID);
        //    m_player.soulSkillManager.AttachSkill(skill, skill.info.type, 0);
        //}

        //private IEnumerable GetSkills()
        //{
        //    return DChildUtility.GetSoulSkills();
        //}
    }
}
#endif