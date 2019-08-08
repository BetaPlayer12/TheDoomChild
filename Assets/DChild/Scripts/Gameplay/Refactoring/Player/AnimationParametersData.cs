using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Refactor.DChild.Gameplay.Characters.Players
{
    [CreateAssetMenu(fileName = "AnimationParametersData", menuName = "DChild/Animation Parameters")]
    public class AnimationParametersData : SerializedScriptableObject
    {
        public enum Parameter
        {
            SpeedX,
            SpeedY,
            IsFacingLeft,
            IsCrouching,
            IsMidAir,
            Jump,
            Land,
            IsDashing,
            DoubleJump,
            LedgeGrab,
            Attack,
            AttackYDirection,
            WallStickTrigger,
            WallSlide,
            WallStick,
            PlatformDrop,
            Flinch
        }

        [SerializeField]
        private Dictionary<Parameter, string> m_labels;

        public string GetParameterLabel(Parameter parameter) => m_labels[parameter];
    }
}