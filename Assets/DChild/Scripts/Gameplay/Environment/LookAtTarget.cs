using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    [ExecuteAlways]
    public class LookAtTarget : MonoBehaviour
    {
        [SerializeField]
        private Transform m_target;
        [SerializeField]
        private float m_rotationOffset;

        // Update is called once per frame
        void LateUpdate()
        {
            transform.up = m_target.position - transform.position;
            var rotation = transform.rotation.eulerAngles.z;
            transform.rotation = Quaternion.Euler(0, 0, rotation + m_rotationOffset);
        }
    }
}