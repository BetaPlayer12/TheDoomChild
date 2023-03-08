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
        [SerializeField]
        private Image m_glow;


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
            SetOrb(orbData.activatedOrb);
        }

        protected void SetOrb(SoulSkillOrbData.OrbInfo info)
        {
            //m_orb.sprite = info.orbSprite;
            m_orb.material = info.orbMaterial ?? null;
            if (m_glow != null)
                m_glow.material = info.glowMaterial ?? null;
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
