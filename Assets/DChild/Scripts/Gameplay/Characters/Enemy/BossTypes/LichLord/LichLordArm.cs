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
    private ParticleFX m_phase1ImpactFX;
    [SerializeField, TabGroup("FX")]
    private ParticleFX m_phase3ImpactFX;
    [SerializeField, TabGroup("Reference")]
    private GameObject m_phase1FxRef;
    [SerializeField, TabGroup("Reference")]
    private GameObject m_phase3FxRef;
    [SerializeField, TabGroup("Reference")]
    private ParticleFX m_phase3GroundFXref;
    [SerializeField, ValueDropdown("GetEvents")]
    private string m_event;
    [SerializeField]
    private SpineEventListener m_spineListener;

    public void ChangePhaseFX() // added
    {
        m_spineListener.Unsubcribe(m_event, m_phase1ImpactFX.Play);
        m_spineListener.Subscribe(m_event, m_phase3ImpactFX.Play);
        m_phase1FxRef.SetActive(false);
        m_phase3FxRef.SetActive(true);
    }
    public ParticleFX GetPhase3GroundFx()
    {
        return m_phase3GroundFXref;
    }
    // Start is called before the first frame update
    private void Start()
    {
        m_spineListener.Subscribe(m_event, m_phase1ImpactFX.Play);
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
