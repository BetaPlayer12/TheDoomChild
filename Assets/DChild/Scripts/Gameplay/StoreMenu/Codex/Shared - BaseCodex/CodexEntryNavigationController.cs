using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace DChild.Menu.Codex
{
    [DisallowMultipleComponent]
    public class CodexEntryNavigationController : MonoBehaviour
    {
        private readonly List<CodexEntryNavigationItem> m_entries = new List<CodexEntryNavigationItem>();

        private CodexScrollNavigationHandle m_scrollHandle;
        private CodexEntryNavigationItem m_lastSelectedEntry;
        private CodexEntryNavigationItem m_pendingEntry;
        private InputAction m_moveAction;
        private CanvasGroup[] m_parentCanvasGroups;
        private Vector2 m_pendingPosition;
        private bool m_hasPendingSelection;
        private int m_pendingPageDirection;

        internal void Register(CodexEntryNavigationItem entry)
        {
            if (entry == null)
                return;

            entry.DisableBuiltInNavigation();
            if (!m_entries.Contains(entry))
                m_entries.Add(entry);
        }

        internal void RecordSelection(CodexEntryNavigationItem entry)
        {
            if (entry != null && entry.transform.IsChildOf(transform))
                m_lastSelectedEntry = entry;
        }

        internal bool TryMove(CodexEntryNavigationItem source, MoveDirection direction)
        {
            if (source == null || !source.transform.IsChildOf(transform) || direction == MoveDirection.None)
                return false;

            RefreshEntries();

            CodexEntryNavigationItem target = direction == MoveDirection.Left || direction == MoveDirection.Right
                ? FindHorizontalTarget(source, direction)
                : FindVerticalTarget(source, direction);

            if (target != null)
            {
                target.Select();
                return true;
            }

            if (direction == MoveDirection.Up || direction == MoveDirection.Down)
            {
                CacheScrollHandle();
                m_scrollHandle?.TryMovePage(direction == MoveDirection.Down ? 1 : -1);
            }

            // This controller owns all cardinal movement while an entry is selected.
            return true;
        }

        private CodexEntryNavigationItem FindHorizontalTarget(CodexEntryNavigationItem source, MoveDirection direction)
        {
            Vector2 sourcePosition = GetPosition(source);
            float rowTolerance = GetRowTolerance(source);
            float directionSign = direction == MoveDirection.Right ? 1f : -1f;

            CodexEntryNavigationItem closest = null;
            float closestDistance = float.MaxValue;

            foreach (CodexEntryNavigationItem entry in m_entries)
            {
                if (entry == source || !entry.IsAvailable())
                    continue;

                Vector2 offset = GetPosition(entry) - sourcePosition;
                float forwardDistance = offset.x * directionSign;
                if (forwardDistance <= 0f || Mathf.Abs(offset.y) > rowTolerance)
                    continue;

                if (forwardDistance < closestDistance)
                {
                    closest = entry;
                    closestDistance = forwardDistance;
                }
            }

            return closest;
        }

        private CodexEntryNavigationItem FindVerticalTarget(CodexEntryNavigationItem source, MoveDirection direction)
        {
            Vector2 sourcePosition = GetPosition(source);
            float rowTolerance = GetRowTolerance(source);
            float directionSign = direction == MoveDirection.Up ? 1f : -1f;
            float nearestRowDistance = float.MaxValue;

            foreach (CodexEntryNavigationItem entry in m_entries)
            {
                if (entry == source || !entry.IsAvailable())
                    continue;

                float forwardDistance = (GetPosition(entry).y - sourcePosition.y) * directionSign;
                if (forwardDistance > rowTolerance)
                    nearestRowDistance = Mathf.Min(nearestRowDistance, forwardDistance);
            }

            if (nearestRowDistance == float.MaxValue)
                return null;

            CodexEntryNavigationItem closest = null;
            float closestHorizontalDistance = float.MaxValue;
            float rowBand = rowTolerance * 2f;

            foreach (CodexEntryNavigationItem entry in m_entries)
            {
                if (entry == source || !entry.IsAvailable())
                    continue;

                Vector2 position = GetPosition(entry);
                float forwardDistance = (position.y - sourcePosition.y) * directionSign;
                if (forwardDistance < nearestRowDistance || forwardDistance > nearestRowDistance + rowBand)
                    continue;

                float horizontalDistance = Mathf.Abs(position.x - sourcePosition.x);
                if (horizontalDistance < closestHorizontalDistance)
                {
                    closest = entry;
                    closestHorizontalDistance = horizontalDistance;
                }
            }

            return closest;
        }

        private void OnPageChangeStarted(int pageIndex)
        {
            var selectedObject = EventSystem.current?.currentSelectedGameObject;
            var selectedEntry = selectedObject?.GetComponent<CodexEntryNavigationItem>();
            if (selectedEntry != null && selectedEntry.transform.IsChildOf(transform))
                m_lastSelectedEntry = selectedEntry;

            if (m_lastSelectedEntry == null)
                return;

            m_pendingEntry = m_lastSelectedEntry;
            m_pendingPosition = GetPosition(m_lastSelectedEntry);
            m_hasPendingSelection = true;
        }

        private void OnPageChangeCompleted(int pageIndex)
        {
            if (!m_hasPendingSelection)
                return;

            RefreshEntries();
            CodexEntryNavigationItem target = m_pendingEntry != null && m_pendingEntry.IsAvailable()
                ? m_pendingEntry
                : FindClosestAvailable(m_pendingPosition);

            target?.Select();
            m_lastSelectedEntry = target;
            m_pendingEntry = null;
            m_hasPendingSelection = false;
        }

        private CodexEntryNavigationItem FindClosestAvailable(Vector2 position)
        {
            CodexEntryNavigationItem closest = null;
            float closestDistance = float.MaxValue;

            foreach (CodexEntryNavigationItem entry in m_entries)
            {
                if (!entry.IsAvailable())
                    continue;

                float distance = (GetPosition(entry) - position).sqrMagnitude;
                if (distance < closestDistance)
                {
                    closest = entry;
                    closestDistance = distance;
                }
            }

            return closest;
        }

        private Vector2 GetPosition(CodexEntryNavigationItem entry)
        {
            RectTransform rectTransform = entry.rectTransform;
            Vector3 worldPosition = rectTransform == null
                ? entry.transform.position
                : rectTransform.TransformPoint(rectTransform.rect.center);
            return transform.InverseTransformPoint(worldPosition);
        }

        private float GetRowTolerance(CodexEntryNavigationItem entry)
        {
            RectTransform rectTransform = entry.rectTransform;
            return rectTransform == null ? 1f : Mathf.Max(1f, rectTransform.rect.height * 0.35f);
        }

        private void RefreshEntries()
        {
            m_entries.Clear();
            m_entries.AddRange(GetComponentsInChildren<CodexEntryNavigationItem>(true));

            foreach (CodexEntryNavigationItem entry in m_entries)
                entry.DisableBuiltInNavigation();
        }

        private bool CanHandleEmptyPageInput()
        {
            if (!isActiveAndEnabled || !gameObject.activeInHierarchy || !AreParentCanvasGroupsInteractable())
                return false;

            RefreshEntries();
            foreach (CodexEntryNavigationItem entry in m_entries)
            {
                if (entry.IsAvailable())
                    return false;
            }

            return true;
        }

        private bool AreParentCanvasGroupsInteractable()
        {
            foreach (CanvasGroup canvasGroup in m_parentCanvasGroups)
            {
                if (canvasGroup.isActiveAndEnabled && !canvasGroup.interactable)
                    return false;
            }

            return true;
        }

        private void CacheScrollHandle()
        {
            if (m_scrollHandle == null)
                m_scrollHandle = GetComponentInChildren<CodexScrollNavigationHandle>(true);
        }

        private void SubscribeToPageChanges()
        {
            CacheScrollHandle();
            if (m_scrollHandle == null)
                return;

            m_scrollHandle.OnPageChangeStarted -= OnPageChangeStarted;
            m_scrollHandle.OnPageChangeCompleted -= OnPageChangeCompleted;
            m_scrollHandle.OnPageChangeStarted += OnPageChangeStarted;
            m_scrollHandle.OnPageChangeCompleted += OnPageChangeCompleted;
        }

        private void UnsubscribeFromPageChanges()
        {
            if (m_scrollHandle == null)
                return;

            m_scrollHandle.OnPageChangeStarted -= OnPageChangeStarted;
            m_scrollHandle.OnPageChangeCompleted -= OnPageChangeCompleted;
        }

        private void BindMoveInput()
        {
            if (m_moveAction != null)
                return;

            var inputModule = EventSystem.current?.currentInputModule as InputSystemUIInputModule;
            m_moveAction = inputModule?.move?.action;
            if (m_moveAction != null)
                m_moveAction.performed += OnMovePerformed;
        }

        private void UnbindMoveInput()
        {
            if (m_moveAction == null)
                return;

            m_moveAction.performed -= OnMovePerformed;
            m_moveAction = null;
        }

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            if (!CanHandleEmptyPageInput())
                return;

            Vector2 input = context.ReadValue<Vector2>();
            if (Mathf.Abs(input.y) <= Mathf.Abs(input.x) || Mathf.Approximately(input.y, 0f))
                return;

            m_pendingPageDirection = input.y < 0f ? 1 : -1;
        }

        private void LateUpdate()
        {
            if (m_pendingPageDirection == 0)
                return;

            int direction = m_pendingPageDirection;
            m_pendingPageDirection = 0;

            if (!CanHandleEmptyPageInput())
                return;

            CacheScrollHandle();
            m_scrollHandle?.TryMovePage(direction);
        }

        private void Awake()
        {
            m_parentCanvasGroups = GetComponentsInParent<CanvasGroup>(true);
            RefreshEntries();
        }

        private void OnEnable()
        {
            SubscribeToPageChanges();
            BindMoveInput();
        }

        private void Start() => BindMoveInput();

        private void OnDisable()
        {
            m_pendingPageDirection = 0;
            UnsubscribeFromPageChanges();
            UnbindMoveInput();
        }
    }
}
