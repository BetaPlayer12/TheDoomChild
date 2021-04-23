using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay
{
    public class RotateObject : MonoBehaviour
    {
        [SerializeField]
        private Vector3 m_speed;

        private void LateUpdate() => transform.Rotate(m_speed * GameplaySystem.time.deltaTime);
    }
}