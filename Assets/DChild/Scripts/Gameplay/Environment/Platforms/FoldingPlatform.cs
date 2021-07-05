using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class FoldingPlatform : MonoBehaviour
    {
        [SerializeField]
        private float m_maxRotationSpeed;
        [SerializeField]
        private AnimationCurve m_rotationSpeed;
        [SerializeField,PropertyOrder(100)]
        private float[] m_rotation;

        [Button]
        public void RotateTo(int index)
        {
            StopAllCoroutines();
            StartCoroutine(RotateToRoutine(m_rotation[index]));
        }

        public void UseRotation(int index)
        {
            StopAllCoroutines();
            transform.rotation = Quaternion.Euler(0, 0, m_rotation[index]);
        }

        private IEnumerator RotateToRoutine(float rotation)
        {
            var startRotatation = transform.eulerAngles;
            var destination = Quaternion.Euler(0, 0, rotation).eulerAngles;
            var time = 0f;
            var lerpValue = 0f;
            do
            {
                var deltaTime = GameplaySystem.time.deltaTime;
                time += deltaTime;
                lerpValue += m_rotationSpeed.Evaluate(time) * m_maxRotationSpeed * deltaTime;
                transform.eulerAngles = Vector3.Lerp(startRotatation, destination, lerpValue);
                yield return null;
            } while (lerpValue < 1);
        }
    }
}
