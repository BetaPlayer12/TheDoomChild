using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Characters
{
    public interface IFacing
    {
        HorizontalDirection currentFacingDirection { get; }
    }

    public interface IFacingConfigurator : IFacing
    {
        void SetFacing(HorizontalDirection facing);
        void TurnCharacter();
    }
}