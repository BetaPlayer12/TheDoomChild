using DChild.Gameplay.Characters;

namespace Refactor.DChild.Gameplay.Characters
{
    public class LogicTurnHandle : SimpleTurnHandle
    {
        public override void Execute()
        {
            TurnCharacter();
            CallTurnDone(new FacingEventArgs(m_character.facing));
        }
    }
}