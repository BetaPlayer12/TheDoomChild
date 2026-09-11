using System;
using System.Runtime.InteropServices;
using DChild.Gameplay.Items;
using DChild.Localization;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.Inventories.UI
{
    public class FullItemDetailsUI : ItemDetailsUI, IItemViewLocalizer
    {
        [SerializeField]
        private TextMeshProUGUI m_name;
        [SerializeField]
        private Image m_icon;
        [SerializeField]
        private TextMeshProUGUI m_description;
        [SerializeField, BoxGroup("Optional")]
        private TextMeshProUGUI m_quantityLimit;

        [SerializeField, BoxGroup("Opacity Targets")] private CanvasGroup m_itemNameCG;
        [SerializeField, BoxGroup("Opacity Targets")] private CanvasGroup m_itemIconCG;
        [SerializeField, BoxGroup("Opacity Targets")] private CanvasGroup m_itemDescriptionCG;
        [SerializeField, BoxGroup("Opacity Targets")] private CanvasGroup m_quantityValueCG;


        private Canvas m_canvas;

        public event Action<ItemData> LocalizeItemView;

        public override void AdjustIconColor(bool isModified)
        {
            throw new NotImplementedException();
        }

        public override void Hide()
        {
            m_canvas.enabled = false;
        }

        public override void Show()
        {
            m_canvas.enabled = true;
        }

        public override void ShowDetails(IStoredItem reference)
        {
            var data = reference?.data ?? null;

            AdjustUIAlphas(data != null);

            if (data == null)
                return;
            //{
            //    m_name.text = "Nothing";
            //    m_icon.sprite = null;
            //    m_description.text = "You have nothing, this is not a lack of something but the absence of everything.\n " +
            //                        "Do not worry having nothing is fine but if you still see this when you should have something is troubling" +
            //                        "Please make sure you have nothing first before saying nothing is fine";
            //    if (m_quantityLimit != null)
            //    {
            //        m_quantityLimit.text = "0";
            //    }
            //}

            m_name.text = data.itemName;
            m_icon.sprite = data.icon;
            m_description.text = data.description;
            SetQuantityValue(reference.hasInfiniteCount
                ? "∞"
                : $"{reference.count} / {data.quantityLimit}");

            LocalizeItemView?.Invoke(data);
        }

        protected void SetQuantityValue(string value)
        {
            if (m_quantityLimit != null)
                m_quantityLimit.text = value;
        }

        private void AdjustUIAlphas(bool hasItemData)
        {
            var opacity = hasItemData ? 1f : 0f;
            if (m_itemNameCG.alpha == opacity)
                return;

            m_itemNameCG.alpha = opacity;
            m_itemDescriptionCG.alpha = opacity;
            m_itemIconCG.alpha = opacity;

            if (m_quantityValueCG != null)
                m_quantityValueCG.alpha = opacity;
        }

        private void Awake()
        {
            m_canvas = GetComponent<Canvas>();
        }

        private void OnDisable()
        {
            AdjustUIAlphas(false);
        }
    }
}
