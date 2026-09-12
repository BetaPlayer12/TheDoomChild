using DChild.Gameplay.EquipmentSystem;
using Doozy.Runtime.UIManager.Containers;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Input;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace DChild.Menu.Equipment.UI
{
    public class EquipmentUI : MonoBehaviour
    {
        [BoxGroup("SAMPLE DATA"), SerializeField] private SoulEquipmentList m_equipmentList;

        [SerializeField] private PlayerSoulEquipmentHandle m_equipmentHandle;
        public PlayerSoulEquipmentHandle equipmentHandle => m_equipmentHandle;

        [BoxGroup("GRID SELECTION"), SerializeField] private EquipmentSelectionUI m_selectionUI;
        public EquipmentSelectionUI selectionUI => m_selectionUI;

        [BoxGroup("DETAILS"), SerializeField] private EquipmentDetailsUI m_detailsUI;
        public EquipmentDetailsUI detailsUI => m_detailsUI;

        private List<SoulEquipmentItem> m_acquiredItems;
        private EquipmentCategoryToggleUI[] m_categories;
        private EquipmentCategoryToggleUI m_activeCategory;
        private InputAction m_cancelAction;
        private Coroutine m_releaseBackButtonRoutine;
        private UIView m_view;
        private bool m_isItemSelectionActive;
        private bool m_backButtonBlocked;

        private void GetEquipmentData(SoulEquipmentList equipmentList)
        {
            m_acquiredItems = new();

            int[] IDs = equipmentList.GetIDs();

            for (int i = 0; i < IDs.Length; i++)
            {
                m_acquiredItems.Add(equipmentList.GetInfo(IDs[i]));
            }
        }

        public void Initialize()
        {
            ResetNavigationState();
            BindCancelInput();

            //get acquired items list from player data
            //m_equipmentList = m_equipmentHandle.GetFullSoulEquipmentList();
            GetEquipmentData(m_equipmentList);

            m_selectionUI.SetupUI(m_acquiredItems);

            m_categories ??= GetComponentsInChildren<EquipmentCategoryToggleUI>();
            foreach (EquipmentCategoryToggleUI category in m_categories)
            {
                category.ResetSelection();
                category.RefreshCurrentItem();
            }

            RefreshCategoryNavigation();

            var defaultCategory = m_categories.FirstOrDefault(category => category.category == SoulSlot.Head);
            if (defaultCategory == null)
            {
                m_detailsUI.Clear();
                return;
            }

            defaultCategory.InitializeItemGrid();
            FocusCategory(defaultCategory.category);
        }

        public void EnterItemSelection(SoulSlot category)
        {
            m_activeCategory = GetCategory(category);
            if (m_activeCategory == null)
                return;

            m_isItemSelectionActive = true;
            BlockBackButton();
        }

        public void FocusCategory(SoulSlot category)
        {
            m_activeCategory = GetCategory(category);
            m_isItemSelectionActive = false;
            m_activeCategory?.Select();
            ReleaseBackButton();
        }

        private void RefreshCategoryNavigation()
        {
            foreach (EquipmentCategoryToggleUI category in m_categories)
            {
                var navigation = new Navigation
                {
                    mode = Navigation.Mode.Explicit,
                    selectOnLeft = GetCategoryNavigationTarget(category, Vector2.left),
                    selectOnRight = GetCategoryNavigationTarget(category, Vector2.right),
                    selectOnUp = GetCategoryNavigationTarget(category, Vector2.up),
                    selectOnDown = GetCategoryNavigationTarget(category, Vector2.down)
                };

                category.toggle.navigation = navigation;
            }
        }

        private UIToggle GetCategoryNavigationTarget(EquipmentCategoryToggleUI source, Vector2 direction)
        {
            var sourcePosition = (Vector2)source.transform.position;
            var perpendicularDirection = new Vector2(-direction.y, direction.x);
            EquipmentCategoryToggleUI closestCategory = null;
            float closestScore = float.MaxValue;

            foreach (EquipmentCategoryToggleUI category in m_categories)
            {
                if (category == source || !category.gameObject.activeInHierarchy || !category.toggle.interactable)
                    continue;

                var offset = (Vector2)category.transform.position - sourcePosition;
                float forwardDistance = Vector2.Dot(offset, direction);
                if (forwardDistance <= 0f)
                    continue;

                float perpendicularDistance = Mathf.Abs(Vector2.Dot(offset, perpendicularDirection));
                float score = forwardDistance + perpendicularDistance * 10f;
                if (score < closestScore)
                {
                    closestScore = score;
                    closestCategory = category;
                }
            }

            return closestCategory == null ? source.toggle : closestCategory.toggle;
        }

        private EquipmentCategoryToggleUI GetCategory(SoulSlot category)
        {
            return m_categories?.FirstOrDefault(toggle => toggle.category == category);
        }

        private void BindCancelInput()
        {
            if (m_cancelAction != null)
                return;

            var inputModule = EventSystem.current?.currentInputModule as InputSystemUIInputModule;
            m_cancelAction = inputModule?.cancel?.action;
            if (m_cancelAction != null)
                m_cancelAction.performed += OnCancelPerformed;
        }

        private void UnbindCancelInput()
        {
            if (m_cancelAction == null)
                return;

            m_cancelAction.performed -= OnCancelPerformed;
            m_cancelAction = null;
        }

        private void OnCancelPerformed(InputAction.CallbackContext context)
        {
            if (!m_isItemSelectionActive || m_activeCategory == null)
                return;

            m_isItemSelectionActive = false;
            m_activeCategory.Select();
            ReleaseBackButtonDeferred();
        }

        private void BlockBackButton()
        {
            if (m_backButtonBlocked)
                return;

            BackButton.Disable();
            m_backButtonBlocked = true;
        }

        private void ReleaseBackButtonDeferred()
        {
            if (m_releaseBackButtonRoutine != null)
                StopCoroutine(m_releaseBackButtonRoutine);

            m_releaseBackButtonRoutine = StartCoroutine(ReleaseBackButtonNextFrame());
        }

        private IEnumerator ReleaseBackButtonNextFrame()
        {
            yield return null;
            m_releaseBackButtonRoutine = null;
            ReleaseBackButton();
        }

        private void ReleaseBackButton()
        {
            if (!m_backButtonBlocked)
                return;

            BackButton.Enable();
            m_backButtonBlocked = false;
        }

        private void ResetNavigationState()
        {
            m_isItemSelectionActive = false;
            m_activeCategory = null;

            if (m_releaseBackButtonRoutine != null)
            {
                StopCoroutine(m_releaseBackButtonRoutine);
                m_releaseBackButtonRoutine = null;
            }

            ReleaseBackButton();
        }

        private void Awake()
        {
            m_view = GetComponent<UIView>();
            m_view?.OnHideCallback.Event.AddListener(ResetNavigationState);
        }

        private void OnEnable()
        {
            BindCancelInput();
        }

        private void OnDisable()
        {
            UnbindCancelInput();
            ResetNavigationState();
        }

        private void OnDestroy()
        {
            m_view?.OnHideCallback.Event.RemoveListener(ResetNavigationState);
        }
    }
}
