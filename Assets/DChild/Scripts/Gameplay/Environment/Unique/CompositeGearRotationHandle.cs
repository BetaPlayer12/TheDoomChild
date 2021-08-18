using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class CompositeGearRotationHandle : MonoBehaviour
    {
        private enum RotateCondition
        {
            Always,
            BasedOnReferenceMovement
        }

        [System.Serializable]
        private class GearInfo
        {
            [SerializeField]
            private Transform m_gear;
            [SerializeField]
            private float m_rotationSpeed = 1;

            public Transform gear => m_gear;
            public float rotationSpeed => m_rotationSpeed;
        }

        [SerializeField]
        private RotateCondition m_rotateCondition;
        [SerializeField, ShowIf("@m_rotateCondition == RotateCondition.BasedOnReferenceMovement")]
        private Transform m_reference;
        [SerializeField, MinValue(0.001)]
        private float m_rotationModifier = 1;
#if UNITY_EDITOR
        [ShowInInspector]
        private bool m_simulateRotation;
#endif
        [SerializeField]
        private GearInfo[] m_gearInfos;

        private Vector3 m_referenceLastPosition;

        public void SetRotationModifier(float rotationModifier)
        {
            m_rotationModifier = rotationModifier;
        }

        private void RotateAllGears(float referenceSpeed, float deltaTime)
        {
            var rotationSpeed = m_rotationModifier * referenceSpeed * deltaTime;
            for (int i = 0; i < m_gearInfos.Length; i++)
            {
                var gearInfo = m_gearInfos[i];
                var gearRotationSpeed = rotationSpeed * gearInfo.rotationSpeed;
                gearInfo.gear?.Rotate(Vector3.forward, gearRotationSpeed);
            }
        }

        private void OnEnable()
        {
            m_referenceLastPosition = m_reference.position;
        }

        private void LateUpdate()
        {
            switch (m_rotateCondition)
            {
                case RotateCondition.Always:
                    RotateAllGears(1f, 1f);
                    break;
                case RotateCondition.BasedOnReferenceMovement:
                    var currentReferencePosition = m_reference.position;
                    if (m_referenceLastPosition != currentReferencePosition)
                    {
                        var movement = currentReferencePosition - m_referenceLastPosition;
                        var referenceSpeed = movement.magnitude;
                        if (Mathf.Sign(movement.x) == -1 || Mathf.Sign(movement.y) == -1)
                        {
                            referenceSpeed *= -1;
                        }
                        RotateAllGears(referenceSpeed, 1f);
                        m_referenceLastPosition = currentReferencePosition;
                    }
                    break;
            }
        }


        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            if (Application.isPlaying ==false && m_simulateRotation)
            {
                RotateAllGears(1f, Time.unscaledDeltaTime);
            }
#endif
        }
    }

}