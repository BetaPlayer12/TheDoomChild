using DChild.Gameplay;
using DChild.Gameplay.Characters;
using Sirenix.OdinInspector;
using UnityEngine;

public class MovementController : SerializedMonoBehaviour
{
    public IMoveHandle m_movement;

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            m_movement.Move(HorizontalDirection.Left);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            m_movement.Move(HorizontalDirection.Right);
        }
        else
        {
            m_movement.Stop();
        }
    }
}
