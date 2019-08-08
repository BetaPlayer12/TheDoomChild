using Sirenix.OdinInspector;

namespace DChild.Gameplay.Characters.Players.Modules
{
    [System.Serializable]
    public struct MovementInfo
    {
        [MinValue(0)]
        public float maxSpeed;
        [MinValue(0)]
        public float acceleration;
        [MinValue(0)]
        public float decceleration;

        public static bool operator !=(MovementInfo a, MovementInfo b)
        {
            return a.maxSpeed != b.maxSpeed || a.acceleration != b.acceleration || a.decceleration != b.decceleration;
        }

        public static bool operator ==(MovementInfo a, MovementInfo b)
        {
            return a.maxSpeed == b.maxSpeed && a.acceleration == b.acceleration && a.decceleration == b.decceleration;
        }
    }

}
