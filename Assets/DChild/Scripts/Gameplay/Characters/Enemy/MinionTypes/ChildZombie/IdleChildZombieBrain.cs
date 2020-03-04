using System.Collections;
using System.Collections.Generic;

namespace DChild.Gameplay.Characters.Enemies
{
    public class IdleChildZombieBrain : ChildZombieBrain
    {
        public override void Enable(bool value)
        {
            m_target = null;
            m_minion.Idle();
        }

        protected override void Idle()
        {
            m_minion.Idle();
        }
    }
}