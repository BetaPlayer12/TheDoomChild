using Holysoft.Event;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{

    public class ResetSkillRequestEventArgs : IEventActionArgs
    {
        private List<PrimarySkill> m_skillsToReset;

        public ResetSkillRequestEventArgs()
        {
            m_skillsToReset = new List<PrimarySkill>();
        }

        public void SetRequest(PrimarySkill[] skills)
        {
            m_skillsToReset.Clear();
            m_skillsToReset.AddRange(skills);
        }

        public bool IsRequestedToReset(PrimarySkill skill)
        {
            for (int i = 0; i < m_skillsToReset.Count; i++)
            {
               if( m_skillsToReset[i] == skill)
                {
                    return true;
                }
            }

            return false;
        }
    }

    [AddComponentMenu("DChild/Gameplay/Player/Skill Reset Requester")]
    public class SkillResetRequester : MonoBehaviour
    {
        private ResetSkillRequestEventArgs m_eventArgs;
        public event EventAction<ResetSkillRequestEventArgs> SkillResetRequest;

        public void RequestSkillReset(params PrimarySkill[] skills)
        {
            m_eventArgs.SetRequest(skills);
            SkillResetRequest?.Invoke(this, m_eventArgs);
        }

        private void Awake()
        {
            m_eventArgs = new ResetSkillRequestEventArgs();
        }
    }
}
