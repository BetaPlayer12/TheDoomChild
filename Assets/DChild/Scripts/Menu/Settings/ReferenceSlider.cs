using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.UI
{
    public abstract class ReferenceSlider : MonoBehaviour
    {
        [SerializeField]
        protected Slider m_slider;
        protected abstract float value { get; set; }

        protected virtual void OnValueChange(float arg0) => value = arg0;

        private void Awake() => m_slider = GetComponentInChildren<Slider>();

        private void OnEnable() => m_slider.onValueChanged.AddListener(OnValueChange);

        private void OnDisable() => m_slider.onValueChanged.RemoveListener(OnValueChange);

#if UNITY_EDITOR
        [SerializeField]
        [HideInInspector]
        private bool m_instantiated;

#endif
        private void OnValidate()
        {
#if UNITY_EDITOR
            if (m_instantiated == false)
            {
                if (m_slider == null)
                {
                    m_slider = GetComponentInChildren<Slider>();
                }
                if (m_slider != null)
                {
                    m_instantiated = true;
                }
            }
#endif
        }
    }
}