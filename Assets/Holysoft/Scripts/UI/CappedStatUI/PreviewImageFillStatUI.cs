using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Holysoft.Gameplay.UI
{
    public class PreviewImageFillStatUI : CappedStatUI
    {
        [SerializeField]
        private Image m_actualStat;
        [SerializeField]
        private Image m_previousStat;
        [SerializeField, MinValue(0)]
        private float m_syncDelay;
        [SerializeField, MinValue(0)]
        private float m_syncDuration;

        private float m_maxValue;
        private float previousCurrent = 0;

        public override float maxValue { set => m_maxValue = value; }
        public override float currentValue
        {
            set
            {
                StopAllCoroutines();
                var uiValue = value / m_maxValue;
                uiValue = float.IsNaN(uiValue) ? 0 : uiValue;
                m_actualStat.fillAmount = uiValue;
                if (uiValue >= m_previousStat.fillAmount)
                {
                    m_previousStat.fillAmount = uiValue;
                }
                else
                {
                    StartCoroutine(SyncUIRoutine(m_previousStat.fillAmount, uiValue));
                }
                previousCurrent = value;
            }
        }

        private IEnumerator SyncUIRoutine(float previousValue, float currentValue)
        {
            yield return new WaitForSeconds(m_syncDelay);
            float lerpValue = 0;
            var lerpSpeed = 1f / m_syncDuration;
            do
            {
                lerpValue += lerpSpeed * Time.unscaledDeltaTime;
                m_previousStat.fillAmount = Mathf.Lerp(previousValue, currentValue, Mathf.Clamp01(lerpValue));
                yield return null;
            } while (lerpValue < 1);
        }
    }
}