using UnityEditor;
using UnityEngine;
using Spine.Unity;
using System.Linq;
using UnityEditor.Animations;
using Sirenix.OdinInspector;

public class AnimationReplacer : MonoBehaviour
{
    [Tooltip("Place animator controller to change the existing animation")]
    public AnimatorController m_animatorController;
    [AssetSelector]
    public AnimationReferenceAsset[] m_newAnimations;

    [Button, Tooltip("Add every animation that will change the existing animations")]
    public void ApplyAnimations()
    {
        ReplaceSpineAnimationReferences();
    }
    [Button]
    public void ClearAnimationData()
    {
        m_newAnimations = null;
    }
    public void ReplaceSpineAnimationReferences()
    {
        if (m_animatorController == null || m_newAnimations == null || m_newAnimations.Length == 0)
        {
            Debug.LogError("AnimatorController or newAnimations not set.");
            return;
        }

        foreach (var layer in m_animatorController.layers)
        {
            ProcessStateMachine(layer.stateMachine);
        }
        AssetDatabase.SaveAssets();
        Debug.Log("Spine animation reference replacement complete!");
    }

    private void ProcessStateMachine(AnimatorStateMachine stateMachine)
    {
        foreach (var state in stateMachine.states)
        {
            ReplaceAnimationInState(state.state);
        }

        foreach (var subStateMachine in stateMachine.stateMachines)
        {
            ProcessStateMachine(subStateMachine.stateMachine);
        }
    }
    private void ReplaceAnimationInState(AnimatorState state)
    {
        var spineState = state.behaviours.OfType<DChild.SpineAnimationState>().FirstOrDefault();
        if (spineState != null)
        {
            foreach (var newAnimation in m_newAnimations)
            {
                if (spineState.m_animation.name == newAnimation.name)
                {
                    spineState.m_animation = newAnimation;
                    Debug.Log($"Replaced animation {spineState.m_animation.name} with {newAnimation.name} in state {state.name}");
                }
                else
                {
                    Debug.LogError($"Can't replace animation {spineState.m_animation.name} with {newAnimation.name} in state {state.name}");
                }
            }
        }
    }
}