using DChild.Gameplay;
using DChild.Gameplay.Characters;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoolsVerdictHammer : MonoBehaviour
{
    [SerializeField]
    private Collider2D m_hurtbox;

    [SerializeField, ValueDropdown("GetEvents")]
    private string m_enableColliderEvent;
    [SerializeField, ValueDropdown("GetEvents")]
    private string m_disableColliderEvent;
    [SerializeField]
    private SpineEventListener m_spineListener;

    // Start is called before the first frame update
    private void Start()
    {
        m_spineListener.Subscribe(m_enableColliderEvent, EnableCollider);
        m_spineListener.Subscribe(m_disableColliderEvent, DisableCollider);
    }

    private void EnableCollider()
    {
        m_hurtbox.enabled = true;
    }

    private void DisableCollider()
    {
        m_hurtbox.enabled = false;
    }

    [SerializeField, PreviewField, OnValueChanged("Initialize")]
    private SkeletonDataAsset m_skeletonDataAsset;

    //#if UNITY_EDITOR
    private IEnumerable GetEvents()
    {
        ValueDropdownList<string> list = new ValueDropdownList<string>();
        var reference = m_skeletonDataAsset.GetAnimationStateData().SkeletonData.Events.ToArray();
        for (int i = 0; i < reference.Length; i++)
        {
            list.Add(reference[i].Name);
        }
        return list;
    }
}
