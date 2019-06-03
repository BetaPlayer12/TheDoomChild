using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Menu
{
    public abstract class MouseMoveRotationBehaviour : MouseMoveBehaviour
    {
        protected override float TranslateMovementX() => m_mouseMovement.y;
        protected override float TranslateMovementY() => m_mouseMovement.x;
    }
}