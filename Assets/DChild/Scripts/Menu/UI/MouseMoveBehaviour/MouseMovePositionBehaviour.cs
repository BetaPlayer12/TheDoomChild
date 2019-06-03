using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Menu
{

    public abstract class MouseMovePositionBehaviour : MouseMoveBehaviour
    {
        protected override float TranslateMovementX() => m_mouseMovement.x;
        protected override float TranslateMovementY() => m_mouseMovement.y;
    }
}