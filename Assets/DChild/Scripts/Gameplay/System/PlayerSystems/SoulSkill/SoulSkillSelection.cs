using DChild.Gameplay.SoulSkills.UI;
using Holysoft.Event;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace DChild.Gameplay.SoulSkills
{
    public class SoulSkillSelection : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_acquiredListUI;
        [SerializeField]
        private GameObject m_activatedListUI;
        [SerializeField]
        private Image m_highlight;

        private SoulSkillUI m_currentSelectedSoulSkill;

        public event EventAction<SoulSkillSelected> OnSelected;
        public event EventAction<SoulSkillSelected> OnActionRequired;

        private bool m_doNotAcceptClickOnMouseRelease;

        public void Reset()
        {
            m_currentSelectedSoulSkill = null;
        }

        private void OnSkillSelected(object sender, SoulSkillSelected eventArgs)
        {
            var skillUI = eventArgs.soulskillUI;
            if (m_currentSelectedSoulSkill != skillUI)
            {
                m_currentSelectedSoulSkill = skillUI;
                OnSelected?.Invoke(this, eventArgs);

                //MouseUp on Same Object triggers OnClick again, force it to not recognize that OnClick
                if (Mouse.current?.leftButton.wasPressedThisFrame ?? false)
                {
                    m_doNotAcceptClickOnMouseRelease = true;
                    enabled = true;
                }
            }

        }

        private void OnSkillHighlight(object sender, SoulSkillSelected eventArgs)
        {
            var rectTransform = m_highlight.rectTransform;
            rectTransform.SetParent(eventArgs.soulskillUI.transform);
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }
        private void OnSkillClicked(object sender, SoulSkillSelected eventArgs)
        {
            if (m_doNotAcceptClickOnMouseRelease)
                return;

            var skillUI = eventArgs.soulskillUI;
            if (m_currentSelectedSoulSkill == skillUI)
            {
                OnActionRequired?.Invoke(this, eventArgs);
            }
        }

        private void Awake()
        {
            var m_acquiredSoulSkillUIList = m_acquiredListUI.GetComponentsInChildren<SoulSkillUI>();
            for (int i = 0; i < m_acquiredSoulSkillUIList.Length; i++)
            {
                var soulSkillUI = m_acquiredSoulSkillUIList[i];
                soulSkillUI.OnHighlighted += OnSkillHighlight;
                soulSkillUI.OnSelected += OnSkillSelected;
                soulSkillUI.OnClick += OnSkillClicked;
            }

            var m_activatedSoulSkillUIList = m_activatedListUI.GetComponentsInChildren<SoulSkillUI>();
            for (int i = 0; i < m_activatedSoulSkillUIList.Length; i++)
            {
                var soulSkillUI = m_activatedSoulSkillUIList[i];
                soulSkillUI.OnHighlighted += OnSkillHighlight;
                soulSkillUI.OnSelected += OnSkillSelected;
                soulSkillUI.OnClick += OnSkillClicked;
            }

            enabled = false;
        }

        private void Update()
        {
            if (m_doNotAcceptClickOnMouseRelease)
            {
                if (Mouse.current.leftButton.wasReleasedThisFrame)
                {
                    m_doNotAcceptClickOnMouseRelease = false;
                    enabled = false;
                    EventSystem.current.SetSelectedGameObject(m_currentSelectedSoulSkill.gameObject); //Force Event System to recognize last GameObject
                }
            }
        }
    }
}
