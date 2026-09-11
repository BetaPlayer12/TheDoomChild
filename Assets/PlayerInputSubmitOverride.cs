using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PlayerInputSubmitOverride : MonoBehaviour
{
    [SerializeField]
    private string[] m_bindingPaths =
    {
        "<Keyboard>/r"
    };

    [SerializeField]
    private string m_bindingGroups = "Keyboard&Mouse;Keyboard";

    private readonly List<Guid> m_addedBindingIds = new List<Guid>();

    private InputAction m_submitAction;

    public void AddBindings()
    {
        InputSystemUIInputModule inputModule =
            EventSystem.current?.currentInputModule as InputSystemUIInputModule;

        m_submitAction = inputModule?.submit?.action;

        if (m_submitAction == null)
        {
            Debug.LogError("Could not find the UI Submit action.", this);
            return;
        }

        string[] pathsToAdd = m_bindingPaths
            .Where(path => !string.IsNullOrWhiteSpace(path))
            .Where(path => !m_submitAction.bindings.Any(binding =>
                string.Equals(
                    binding.effectivePath,
                    path,
                    StringComparison.OrdinalIgnoreCase)))
            .ToArray();

        if (pathsToAdd.Length == 0)
            return;

        ModifyBindings(() =>
        {
            foreach (string path in pathsToAdd)
            {
                var addedBinding = m_submitAction.AddBinding(
                    path,
                    groups: m_bindingGroups);

                m_addedBindingIds.Add(addedBinding.binding.id);
            }
        });
    }

    
    private void ModifyBindings(Action modification)
    {
        InputActionMap actionMap = m_submitAction.actionMap;
        InputActionAsset actionAsset = actionMap?.asset;
        var previouslyEnabledActions = new List<InputAction>();

        if (actionAsset != null)
        {
            foreach (InputActionMap map in actionAsset.actionMaps)
            {
                foreach (InputAction action in map.actions)
                {
                    if (action.enabled)
                        previouslyEnabledActions.Add(action);
                }
            }

            actionAsset.Disable();
        }
        else if (actionMap != null)
        {
            foreach (InputAction action in actionMap.actions)
            {
                if (action.enabled)
                    previouslyEnabledActions.Add(action);
            }

            actionMap.Disable();
        }
        else
        {
            if (m_submitAction.enabled)
                previouslyEnabledActions.Add(m_submitAction);

            m_submitAction.Disable();
        }

        try
        {
            modification();
        }
        finally
        {
            foreach (InputAction action in previouslyEnabledActions)
                action.Enable();
        }
    }

    private void OnDestroy()
    {
        if (m_submitAction == null || m_addedBindingIds.Count == 0)
            return;

        ModifyBindings(() =>
        {
            foreach (Guid bindingId in m_addedBindingIds)
            {
                var binding = m_submitAction.ChangeBindingWithId(bindingId);

                if (binding.valid)
                    binding.Erase();
            }

            m_addedBindingIds.Clear();
        });
    }

}