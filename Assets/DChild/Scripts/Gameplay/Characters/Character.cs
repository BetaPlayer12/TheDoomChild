using DChild.Gameplay.Characters;
using DChild.Gameplay.Systems.WorldComponents;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay
{
    public class Character : MonoBehaviour, ICharacter, ITurningCharacter
    {
        public static string objectTag => "Character";

        [SerializeField]
        public IsolatedObject m_isolatedObject;
        [SerializeField]
        private IsolatedPhysics2D m_physics;
        [SerializeField]
        private CharacterColliders m_colliders;
        [SerializeField]
        private HorizontalDirection m_facing = HorizontalDirection.Right;

        public event EventAction<FacingEventArgs> CharacterTurn;

        public IsolatedObject isolatedObject => m_isolatedObject;
        public IsolatedPhysics2D physics => m_physics;
        public CharacterColliders colliders => m_colliders;
        public HorizontalDirection facing => m_facing;

        public void SetFacing(HorizontalDirection facing)
        {
            m_facing = facing;
            CharacterTurn?.Invoke(this, new FacingEventArgs(m_facing));
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