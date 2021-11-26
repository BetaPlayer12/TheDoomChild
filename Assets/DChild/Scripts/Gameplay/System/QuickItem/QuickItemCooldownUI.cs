using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.Inventories.QuickItem
{
    public class QuickItemCooldownUI : MonoBehaviour
    {
        [SerializeField]
        private Image m_cooldownIndicatorShade;
        [SerializeField]
        private Image m_cooldownIndicatorFX;
        [SerializeField, MinValue(0.0001f)]
        private float m_transitionDuration = 0.75f;
        private bool m_showIndicator;

        public void DisplayCooldown(float timerPercentage)
        {
            if (timerPercentage == 0 && m_showIndicator)
            {
                StopAllCoroutines();
                StartCoroutine(ChangeVisibilityRoutine(1, 0));
                m_showIndicator = false;
            }
            else if (timerPercentage > 0 && m_showIndicator == false)
            {
                StopAllCoroutines();
                StartCoroutine(ChangeVisibilityRoutine(0, 1));
                m_showIndicator = true;
            }
        }

        private IEnumerator ChangeVisibilityRoutine(float from, float to)
        {
            var lerpSpeed = 1f / m_transitionDuration;
            var lerpValue = 0f;
            var visiblityValue = 0f;
            var shadeMaterial = m_cooldownIndicatorShade.materialForRendering;
            var fxMaterial = m_cooldownIndicatorFX.materialForRendering;
            do
            {
                visiblityValue = Mathf.Lerp(from, to, lerpValue);
                SetMaterialValue(visiblityValue);
                lerpValue += lerpSpeed * Time.fixedUnscaledDeltaTime;
                yield return null;
            } while (lerpValue < 1);

            visiblityValue = Mathf.Lerp(from, to, 1f);
            SetMaterialValue(visiblityValue);


            yield return null;

            void SetMaterialValue(float visibilityValue)
            {
                SetVisibility(shadeMaterial, visiblityValue);
                SetVisibility(fxMaterial, visiblityValue);
            }
        }

        private void SetVisibility(Material material, float visiblityValue)
        {
            material.SetFloat("_VisibilityValue", visiblityValue);
        }

        private void Awake()
        {
            SetVisibility(m_cooldownIndicatorShade.materialForRendering, 0);
            SetVisibility(m_cooldownIndicatorFX.materialForRendering, 0);
        }
    }
}
