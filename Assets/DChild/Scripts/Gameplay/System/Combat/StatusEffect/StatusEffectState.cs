using Holysoft.Event;

namespace DChild.Gameplay.Combat.StatusInfliction
{
    [System.Serializable]
    public class StatusEffectState : IStatusEffectState
    {
        private bool[] m_stateList;
        public event EventAction<StatusEffectAfflictionEventArgs> StateChange;

        public StatusEffectState()
        {
            m_stateList = new bool[(int)StatusEffectType._Count];
        }

        public void ChangeStatus(StatusEffectType type, bool isAffected)
        {
            m_stateList[(int)type] = isAffected;
            StateChange?.Invoke(this, new StatusEffectAfflictionEventArgs(type, isAffected));
        }

        public bool IsInflictedWith(StatusEffectType type) => m_stateList[(int)type];
    }
}