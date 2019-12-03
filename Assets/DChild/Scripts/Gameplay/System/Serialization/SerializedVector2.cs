using UnityEngine;

namespace DChild.Serialization
{

    [System.Serializable]
    public struct SerializedVector2
    {
        [SerializeField]
        public float x;
        [SerializeField]
        public float y;

        public static implicit operator SerializedVector2(Vector2 vector2)
        {
            return new SerializedVector2 { x = vector2.x, y = vector2.y };
        }

        public static implicit operator Vector2(SerializedVector2 vector2)
        {
            return new Vector2 { x = vector2.x, y = vector2.y };
        }
    }
}