using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment.Interractables
{
    public class GrapplePoint : MonoBehaviour, IGrappleObject
    {
        public bool canBePulled => false;

        public float pullOffset => 0f;

        public float dashOffset => 0f;

        public Vector2 position { get => transform.position; }

        public IsolatedPhysics2D physics => null;

        private void OnValidate()
        {
            gameObject.tag = "Droppable";
        }
    }
}