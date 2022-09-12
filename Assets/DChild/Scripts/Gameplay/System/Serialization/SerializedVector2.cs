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

    [System.Serializable]
    public struct SerializedVector3
    {
        [SerializeField]
        public float x;
        [SerializeField]
        public float y;
        [SerializeField]
        public float z;

        public static implicit operator SerializedVector3(Vector3 vector3)
        {
            return new SerializedVector3 { x = vector3.x, y = vector3.y, z = vector3.z };
        }

        public static implicit operator Vector3(SerializedVector3 vector3)
        {
            return new Vector3 { x = vector3.x, y = vector3.y, z = vector3.z };
        }

        public static implicit operator SerializedVector3(Vector2 vector2)
        {
            return new SerializedVector3 { x = vector2.x, y = vector2.y, z = 0 };
        }

        public static implicit operator Vector2(SerializedVector3 vector2)
        {
            return new Vector2 { x = vector2.x, y = vector2.y };
        }
    }
}