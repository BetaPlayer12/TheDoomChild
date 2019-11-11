using DChild.Gameplay.Characters;
using DChild.Gameplay.Systems.WorldComponents;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay
{
    [AddComponentMenu("DChild/Gameplay/Objects/Character")]
    public class Character : MonoBehaviour, ICharacter, ITurningCharacter
    {
        public static string objectTag => "Character";

        [SerializeField]
        private Transform m_centerMass;
        [SerializeField]
        private IsolatedObject m_isolatedObject;
        [SerializeField]
        private IsolatedPhysics2D m_physics;
        [SerializeField]
        private CharacterColliders m_colliders;
        [SerializeField]
        private HorizontalDirection m_facing = HorizontalDirection.Right;
        private int m_ID;
        private bool m_hasID;

        public event EventAction<FacingEventArgs> CharacterTurn;
        public event EventAction<ObjectIDEventArgs> InstanceDestroyed;

        public IsolatedObject isolatedObject => m_isolatedObject;
        public IsolatedPhysics2D physics => m_physics;
        public CharacterColliders colliders => m_colliders;
        public HorizontalDirection facing => m_facing;

        public Transform centerMass => m_centerMass;

        public int ID => m_ID;
        public bool hasID => m_hasID;

        public void SetID(int ID)
        {
            m_ID = ID;
            m_hasID = true;
        }

        public void SetFacing(HorizontalDirection facing)
        {
            m_facing = facing;
            CharacterTurn?.Invoke(this, new FacingEventArgs(m_facing));
        }

        private void OnDestroy()
        {
            InstanceDestroyed?.Invoke(this, new ObjectIDEventArgs(this));
        }

        private void OnValidate()
        {
            if (gameObject.CompareTag(objectTag) == false)
            {
                gameObject.tag = objectTag;
                Debug.Log(gameObject.tag);
            }
        }
    }
}