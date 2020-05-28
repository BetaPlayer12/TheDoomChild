using DChild.Gameplay;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class Waterfall : MonoBehaviour
    {
        [SerializeField, HorizontalGroup("On")]
        private Vector3 m_onPosition;
        [SerializeField, HorizontalGroup("Off")]
        private Vector3 m_offPosition;
        [SerializeField]
        private float m_changeScaleSpeed;
        [SerializeField]
        private Transform m_temporaryParent;
        [SerializeField]
        private Transform m_trigger;
        [SerializeField]
        private bool m_startAsFlowing = true;
        private float m_fullVerticalScale;
        private Vector3 m_triggerScale;

        public void StartFlow()
        {
            ChangePosition(m_onPosition);
            m_trigger.localScale = m_triggerScale;
            m_trigger.localPosition = Vector3.zero;
            StopAllCoroutines();
            StartCoroutine(ChangeScaleTo(m_fullVerticalScale));
        }


        public void StopFlow()
        {
            ChangePosition(m_offPosition);
            StopAllCoroutines();
            StartCoroutine(ChangeScaleTo(0f));
        }

        public void StartAsFlowing(bool isFlowing)
        {
            var newScale = transform.localScale;
            if (isFlowing)
            {
                ChangePosition(m_onPosition);
                m_trigger.localScale = m_triggerScale;
                m_trigger.localPosition = Vector3.zero;
                newScale.y = m_fullVerticalScale;
            }
            else
            {
                ChangePosition(m_offPosition);
                newScale.y = 0;
            }
            transform.localScale = newScale;
        }

        private void ChangePosition(Vector3 position)
        {
            m_trigger.parent = m_temporaryParent;
            transform.position = position;
            m_trigger.parent = transform;
        }

        private IEnumerator ChangeScaleTo(float scale)
        {
            var originalScale = transform.localScale;
            var destinationScale = transform.localScale;
            destinationScale.y = scale;
            float lerpValue = 0;
            do
            {
                lerpValue += GameplaySystem.time.deltaTime * m_changeScaleSpeed;
                transform.localScale = Vector3.Lerp(originalScale, destinationScale, lerpValue);
                yield return null;
            } while (lerpValue < 1);
        }

        private void Awake()
        {
            m_fullVerticalScale = transform.localScale.y;
            m_triggerScale = m_trigger.localScale;
            StartAsFlowing(m_startAsFlowing);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Disable players ability to jump and other movement skills;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            //Disable players ability to jump and other movement skills;
        }

#if UNITY_EDITOR
        [ResponsiveButtonGroup("On/Button"), Button("Use Current")]
        private void UseCurrentForOnPosition()
        {
            m_onPosition = transform.position;
        }

        [ResponsiveButtonGroup("Off/Button"), Button("Use Current")]
        private void UseCurrentForOffPosition()
        {
            m_offPosition = transform.position;
        }
#endif
    }
}
