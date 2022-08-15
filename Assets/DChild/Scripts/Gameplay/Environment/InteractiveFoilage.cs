using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class InteractiveFoilage : MonoBehaviour
    {
        [SerializeField]
        private float m_bendDuration;
        [SerializeField]
        private float m_bendFrequency;
        [SerializeField]
        private float m_bendAmplitude;
        [SerializeField]
        private AnimationCurve m_bendAmplitudeDecay;

        private Renderer[] m_renderers;

        private MaterialPropertyBlock m_propertyBlock;
        private static string property => "Vector1_BendDirection";

        private IEnumerator BendFoilage(float startingDirection)
        {
            var duration = 0f;
            do
            {
                var evaluatedAmplitude = m_bendAmplitudeDecay.Evaluate(duration / m_bendDuration) * m_bendAmplitude;
                var bendValue = Mathf.Sin(duration * m_bendFrequency) * evaluatedAmplitude;
                SetBendDirection(bendValue  * startingDirection);
                duration += GameplaySystem.time.deltaTime;
                yield return null;
            } while (duration < m_bendDuration);
        }

        private void SetBendDirection(float value)
        {
            for (int i = 0; i < m_renderers.Length; i++)
            {
                m_renderers[i].GetPropertyBlock(m_propertyBlock);
                m_propertyBlock.SetFloat(property, value);
                m_renderers[i].SetPropertyBlock(m_propertyBlock);
            }
        }

        [Button]
        private void StartBendingTo(float direction)
        {
            StopAllCoroutines();
            StartCoroutine(BendFoilage(direction));
        }

        private void Awake()
        {
            m_renderers = GetComponentsInChildren<Renderer>();
            m_propertyBlock = new MaterialPropertyBlock();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponentInParent(out Character character))
            {
                int direction = 0;
                if (character.centerMass.position.x < transform.position.x)
                {
                    direction = -1;
                }
                else
                {
                    direction = 1;
                }
                StartBendingTo(direction);
            }
        }
    }
}