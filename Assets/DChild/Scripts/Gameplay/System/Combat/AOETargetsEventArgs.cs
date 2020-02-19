using Holysoft.Event;
using System.Collections.Generic;

namespace DChild.Gameplay.Combat
{
    public class AOETargetsEventArgs  : IEventActionArgs
    {
        private List<IDamageable> m_targets;

        public void Initialize(List<IDamageable> m_targets)
        {
            this.m_targets = m_targets;
        }

        public int count => m_targets.Count;
        public IDamageable GetTarget(int index) => m_targets[index];
    }
}