using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public class DamageColorIndicator : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer m_renderer;
        [SerializeField]
        private Color m_color;
        [SerializeField, Min(0)]
        private float m_fadeInDuration = 0.05f;
        [SerializeField, Min(0)]
        private float m_fadeOutDuration = 0.1f;

        private MaterialPropertyBlock m_propertyBlock;
        private int m_colorValueID;
        private int m_colorTriggerValueID;

        [Button]
        public void Execute()
        {
            StopAllCoroutines();
            StartCoroutine(ColorChangeRoutine());
        }

        private IEnumerator ColorChangeRoutine()
        {
            m_renderer.GetPropertyBlock(m_propertyBlock);
            m_propertyBlock.SetColor(m_colorValueID, m_color);
            yield return ChangeColorToRoutine(0, 1, 1f / m_fadeInDuration);
            yield return ChangeColorToRoutine(1, 0, 1f / m_fadeOutDuration);
        }

        private IEnumerator ChangeColorToRoutine(float from, float to, float speed)
        {
            var time = 0f;
            do
            {
                time += GameplaySystem.time.deltaTime * speed;
                var lerpValue = Mathf.Lerp(from, to, time);
                m_propertyBlock.SetFloat(m_colorTriggerValueID, lerpValue);
                m_renderer.SetPropertyBlock(m_propertyBlock);
                yield return null;
            } while (time < 1);
        }

        private void OnDamageTaken(object sender, Damageable.DamageEventArgs eventArgs)
        {
            Execute();
        }

        private void Awake()
        {
            GetComponentInParent<Damageable>().DamageTaken += OnDamageTaken;

            m_propertyBlock = new MaterialPropertyBlock();
            m_colorValueID = Shader.PropertyToID("_Damage_Color");
            m_colorTriggerValueID = Shader.PropertyToID("_DamageTriggerValue");

        }
    }
}
