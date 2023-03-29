using DChild;
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
    private SpineFX m_animationFX;
    [SerializeField]
    private Collider2D m_hurtbox;
    [SerializeField, BoxGroup("FX")]
    private GameObject m_impactFX;
    [SerializeField, BoxGroup("FX")]
    private GameObject m_groundFX;
    [SerializeField]
    private Transform m_impactStartPoint;
    [SerializeField]
    private Transform m_groundStartPoint;

    [SerializeField, ValueDropdown("GetEvents")]
    private string m_enableColliderEvent;
    [SerializeField, ValueDropdown("GetEvents")]
    private string m_disableColliderEvent;
    [SerializeField]
    private SpineEventListener m_spineListener;

    // Start is called before the first frame update
    private void Start()
    {
        transform.localScale = Vector3.one;
        m_spineListener.Subscribe(m_enableColliderEvent, EnableCollider);
        m_spineListener.Subscribe(m_enableColliderEvent, ImpactFX);
        m_spineListener.Subscribe(m_enableColliderEvent, GroundFX);
        m_spineListener.Subscribe(m_disableColliderEvent, DisableCollider);

        m_animationFX.Stop();
        m_animationFX.Play();
    }

    private void EnableCollider()
    {
        m_hurtbox.enabled = true;
    }

    private void DisableCollider()
    {
        m_hurtbox.enabled = false;
    }

    private void ImpactFX()
    {
        var instance = GameSystem.poolManager.GetPool<FXPool>().GetOrCreateItem(m_impactFX);
        instance.transform.position = m_impactStartPoint.position;
    }

    private void GroundFX()
    {
        var instance = GameSystem.poolManager.GetPool<FXPool>().GetOrCreateItem(m_groundFX);
        instance.transform.position = m_groundStartPoint.position;
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
