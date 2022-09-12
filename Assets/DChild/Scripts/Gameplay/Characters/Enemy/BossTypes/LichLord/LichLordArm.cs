using DChild.Gameplay;
using DChild.Gameplay.Characters;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichLordArm : MonoBehaviour
{
    [SerializeField, TabGroup("FX")]
    private ParticleFX m_impactFX;
    [SerializeField, ValueDropdown("GetEvents")]
    private string m_event;
    [SerializeField]
    private SpineEventListener m_spineListener;

    // Start is called before the first frame update
    private void Start()
    {
        m_spineListener.Subscribe(m_event, m_impactFX.Play);
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
