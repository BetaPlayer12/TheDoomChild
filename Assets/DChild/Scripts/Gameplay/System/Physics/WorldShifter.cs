using DChild.Gameplay.Physics;
using UnityEngine;

namespace DChild.Gameplay
{

    [System.Serializable]
    public class WorldShifter
    {
        [SerializeField]
        private WorldOrientation m_orientation;
        [SerializeField]
        private bool m_enable;

        private Rigidbody2D m_rigidbody;
        private IGravity2D m_gravity;

        public Vector2 velocity
        {
            get
            {
                if (convertionNeeded)
                {
                    return Convert(m_rigidbody.velocity, m_orientation, WorldOrientation.Up);
                }
                else
                {
                    return m_rigidbody.velocity;
                }
            }
        }

        public Vector2 up
        {
            get
            {
                if (convertionNeeded)
                {
                    switch (m_orientation)
                    {
                        case WorldOrientation.Up:
                            return Vector2.up;
                        case WorldOrientation.Down:
                            return Vector2.down;
                        case WorldOrientation.Left:
                            return Vector2.left;
                        case WorldOrientation.Right:
                            return Vector2.right;
                        default:
                            return Vector2.zero;
                    }
                }
                else
                {
                    return Vector2.up;
                }
            }
        }

        public WorldOrientation orientation => m_orientation;

        private bool convertionNeeded => (m_enable == true && m_orientation != WorldOrientation.Up);


        public void Initialize(Rigidbody2D rigidbody2D, IGravity2D gravity)
        {
            m_rigidbody = rigidbody2D;
            m_gravity = gravity;
        }

        public void SetOrientation(WorldOrientation worldOrientation)
        {
            m_rigidbody.velocity = Convert(m_rigidbody.velocity, m_orientation, worldOrientation);
            m_orientation = worldOrientation;
            m_gravity.SetAngle(-up);
        }

        public Vector2 ConvertToOrientation(Vector2 value)
        {
            if (convertionNeeded)
            {
                return Convert(value, WorldOrientation.Up, m_orientation);
            }
            else
            {
                return value;
            }
        }

        public Vector2 Convert(Vector2 value, WorldOrientation from, WorldOrientation to)
        {
            if (m_enable)
            {
                switch (from)
                {
                    case WorldOrientation.Up:
                        switch (to)
                        {
                            case WorldOrientation.Down:
                                return -value;
                            case WorldOrientation.Right:
                                return new Vector2(value.y, -value.x);
                            case WorldOrientation.Left:
                                return new Vector2(-value.y, value.x);
                        }
                        break;

                    case WorldOrientation.Down:
                        switch (to)
                        {
                            case WorldOrientation.Up:
                                return -value;
                            case WorldOrientation.Left:
                                return new Vector2(value.y, -value.x);
                            case WorldOrientation.Right:
                                return new Vector2(-value.y, value.x);
                        }
                        break;

                    case WorldOrientation.Left:
                        switch (to)
                        {
                            case WorldOrientation.Right:
                                return -value;
                            case WorldOrientation.Up:
                                return new Vector2(value.y, -value.x);
                            case WorldOrientation.Down:
                                return new Vector2(-value.y, value.x);
                        }
                        break;

                    case WorldOrientation.Right:
                        switch (to)
                        {
                            case WorldOrientation.Left:
                                return -value;
                            case WorldOrientation.Down:
                                return new Vector2(value.y, -value.x);
                            case WorldOrientation.Up:
                                return new Vector2(-value.y, value.x);
                        }
                        break;
                }
                return value;
            }
            else
            {
                return value;
            }
        }
    }
}
