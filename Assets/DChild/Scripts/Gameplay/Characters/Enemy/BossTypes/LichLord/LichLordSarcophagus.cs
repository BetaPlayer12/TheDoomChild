using DChild;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichLordSarcophagus : MonoBehaviour
{
    [SerializeField]
    private SpineRootAnimation m_spine;
    [SerializeField]
    private SpineEventListener m_spineListener;
    [SerializeField, TabGroup("FX")]
    private ParticleFX m_explosionFX;
    [SerializeField, TabGroup("Reference")]
    private Collider2D m_bodyCollider;
    [SerializeField, TabGroup("Reference")]
    private Collider2D m_hurtbox;
    [SerializeField, ValueDropdown("GetEvents")]
    private string m_event;

#if UNITY_EDITOR
    [SerializeField]
    private SkeletonAnimation m_skeletonAnimation;
    [SerializeField]
    private GameObject m_placeholder;

    public void InitializeField(SpineRootAnimation spineRoot, SkeletonAnimation animation)
    {
        m_spine = spineRoot;
        //m_skeletonFAnimation = animationF;
        //m_skeletonBAnimation = animationB;
    }
#endif

    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_explodeAnimation;

    // Start is called before the first frame update
    private void Start()
    {
        m_spineListener.Subscribe(m_event, ExplosionStart);
        StartCoroutine(DisableMaskRoutine());
    }

    private void ExplosionStart()
    {
        StartCoroutine(ExplosionRoutine());
    }

    public void ExplosionPrep()
    {
        StartCoroutine(PreExplodeRoutine());
    }

    private IEnumerator DisableMaskRoutine()
    {
        yield return new WaitForSeconds(.1f);
        GetComponentInChildren<SkeletonAnimation>().maskInteraction = SpriteMaskInteraction.None;
        yield return null;
    }

    private IEnumerator ExplosionRoutine()
    {
        m_explosionFX.Play();
        m_bodyCollider.enabled = false;
        m_hurtbox.enabled = true;
        yield return new WaitForSeconds(.25f);
        m_hurtbox.enabled = false;
        m_placeholder.SetActive(false);
        yield return new WaitForAnimationComplete(m_spine.animationState, m_explodeAnimation);
        Destroy(this.gameObject);
        yield return null;
    }

    private IEnumerator PreExplodeRoutine()
    {
        m_placeholder.SetActive(true);
        m_spine.SetAnimation(0, m_explodeAnimation, false);
        yield return null;
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
