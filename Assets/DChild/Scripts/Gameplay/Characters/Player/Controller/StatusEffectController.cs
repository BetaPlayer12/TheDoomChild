using DChild.Gameplay.Combat.StatusInfliction;
using DChild.Inputs;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class StatusEffectController : MonoBehaviour, IFrozenController, IPetrifyController
    {
        public event EventAction<ControllerEventArgs> CallFrozenHandler;
        public event EventAction<ControllerEventArgs> CallPetrifyHandler;

        public void CallUpdate(IStatusEffectState state, ControllerEventArgs eventArgs)
        {
            //This does not consider Multiple Controller Use
            var input = eventArgs.input;
            if (state.IsInflictedWith(StatusEffectType.Petrify))
            {
                if (input.direction.isRightPressed || input.direction.isLeftPressed)
                {
                    CallPetrifyHandler?.Invoke(this, eventArgs);
                }
            }
            else if (state.IsInflictedWith(StatusEffectType.Frozen))
            {
                if (input.direction.isRightPressed || input.direction.isLeftPressed)
                {
                    CallFrozenHandler?.Invoke(this, eventArgs);
                }
            }

        }
    }
}
