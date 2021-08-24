using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Inventories.QuickItem;
using Holysoft.Event;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.Inventories
{
    public class QuickItemRestriction : MonoBehaviour
    {
        [SerializeField]
        private QuickItemController m_controller;
        [SerializeField]
        private ShadowMorph m_shadowMorph;
        [SerializeField]
        private Image m_disabledQuickItemImage;

        private void OnShadowMorphEnd(object sender, EventActionArgs eventArgs)
        {
            SetQuickItemEnability(true);
        }

        private void OnShadowMorphExecute(object sender, EventActionArgs eventArgs)
        {
            SetQuickItemEnability(false);
        }

        private void SetQuickItemEnability(bool isEnable)
        {
            m_controller.SetEnable(isEnable);
            m_disabledQuickItemImage.enabled = !isEnable;
        }

        private void OnEnable()
        {
            m_shadowMorph.ExecuteModule += OnShadowMorphExecute;
            m_shadowMorph.End += OnShadowMorphEnd;

            SetQuickItemEnability(m_controller.isEnabled);
        }

        private void OnDisable()
        {
            m_shadowMorph.ExecuteModule -= OnShadowMorphExecute;
            m_shadowMorph.End -= OnShadowMorphEnd;
        }
    }
}
