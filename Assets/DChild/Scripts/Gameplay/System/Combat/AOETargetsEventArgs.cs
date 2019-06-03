using Holysoft.Event;
using System.Collections.Generic;

namespace DChild.Gameplay.Combat
{
    public struct AOETargetsEventArgs  : IEventActionArgs
    {
        private List<ITarget> m_targets;

        public AOETargetsEventArgs(List<ITarget> m_targets)
        {
            this.m_targets = m_targets;
        }

        public int count => m_targets.Count;
        public ITarget GetTarget(int index) => m_targets[index];
    }
}