using DChild.Gameplay.Characters.Players.SoulSkills;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.SoulSkills.UI
{
    public class SoulSkillUI : MonoBehaviour
    {
        [SerializeField]
        protected Image m_orb;
        [SerializeField]
        protected Image m_icon;
        public Sprite soulSkillIcon => m_icon.sprite;
        public int soulSkillID { get; private set; }

        public void DisplayAs(SoulSkill soulSkill)
        {
            if (soulSkill == null)
            {
                soulSkillID = -1;
            }
            else
            {
                soulSkillID = soulSkill.id;
                SetOrb(soulSkill.orbData);
                m_icon.sprite = soulSkill.icon;
            }
        }

        protected virtual void SetOrb(SoulSkillOrbData orbData)
        {
            m_orb.sprite = orbData.activatedOrb;
        }

        public virtual void CopyUI(SoulSkillButton reference)
        {
            soulSkillID = reference.soulSkillID;
            m_icon.sprite = reference.soulSkillIcon;
        }

        public virtual void Show(bool immidiate)
        {
        }

        public virtual void Hide(bool immidiate)
        {
        }
    }
}
