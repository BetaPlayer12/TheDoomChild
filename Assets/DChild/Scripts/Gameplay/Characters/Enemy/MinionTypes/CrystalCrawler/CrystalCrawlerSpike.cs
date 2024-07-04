using DChild;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalCrawlerSpike : FX
{
    [SerializeField, BoxGroup("FX")]
    private ParticleFX m_dustFX;
    [SerializeField, BoxGroup("FX")]
    private GameObject m_explodeFXGO;
    [SerializeField, BoxGroup("FX")]
    private GameObject m_shatterFX;
    [SerializeField, BoxGroup("Spine")]
    private SpineEventListener m_spineListener;
    [SerializeField, BoxGroup("Spine")]
    private SpineRootAnimation m_spine;

    [SerializeField, BoxGroup("Audio")]
    private AudioSource m_audio;

#if UNITY_EDITOR
    [SerializeField, BoxGroup("Spine")]
    private SkeletonAnimation m_skeletonAnimation;

    public void InitializeField(SpineRootAnimation spineRoot, IsolatedPhysics2D physics, SkeletonAnimation animation)
    {
        m_spine = spineRoot;
        m_skeletonAnimation = animation;
    }
#endif
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation"), BoxGroup("Spine")]
    private string m_startAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation"), BoxGroup("Spine")]
    private string m_loopAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation"), BoxGroup("Spine")]
    private string m_endAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation"), BoxGroup("Spine")]
    private string m_shattterAnimation;

    [SerializeField, ValueDropdown("GetEvents")]
    private string m_event;

    [SerializeField, BoxGroup("Spike")]
    private BoxCollider2D m_hitbox;
    [SerializeField, BoxGroup("Spike")]
    private float m_spikeDuration;

    // Start is called before the first frame update
    //private void Start()
    //{
    //    m_spineListener.Subscribe(m_event, m_dustFX.Play);
    //    m_spineListener.Subscribe(m_event, m_explodeFX.Play);
    //}

    [SerializeField, PreviewField, OnValueChanged("Initialize")]
    private SkeletonDataAsset m_skeletonDataAsset;

    //[SerializeField]
    //private string[] m_viableTags;

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    for (int j = 0; j < m_viableTags.Length; j++)
    //    {
    //        if (collision.CompareTag(m_viableTags[j]))
    //        {
    //            Stop();
    //        }
    //    }
    //}

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

    private IEnumerator SpikeRoutine()
    {
        //m_dustFX?.Play();
        var explodFX = GameSystem.poolManager.GetPool<FXPool>().GetOrCreateItem(m_explodeFXGO);
        explodFX.transform.position = transform.position;
        m_audio.Play();
        this.gameObject.SetActive(true);
        m_spine.SetAnimation(0, m_startAnimation, false);
        yield return new WaitForAnimationComplete(m_spine.animationState, m_startAnimation);
        m_spine.SetAnimation(0, m_loopAnimation, false);
        yield return new WaitForSeconds(m_spikeDuration);
        m_hitbox.enabled = false;
        m_spine.SetAnimation(0, m_endAnimation, false);
        yield return new WaitForAnimationComplete(m_spine.animationState, m_endAnimation);
        Stop();
        yield return null;
    }

    private IEnumerator ShatterShardRoutine()
    {
        
        m_spine.SetAnimation(0, m_shattterAnimation, false);
        StopCoroutine(SpikeRoutine());
        var shatterFX = GameSystem.poolManager.GetPool<FXPool>().GetOrCreateItem(m_shatterFX);
        shatterFX.transform.position = transform.position;
        yield return new WaitForAnimationComplete(m_spine.animationState, m_shattterAnimation);
        Stop();

    }
    
    public void HideObject()
    {
        StopAllCoroutines();
        this.gameObject.SetActive(false);
    }
    public void PlayShatterShard()
    {
        StartCoroutine(ShatterShardRoutine());
    }
    public void StopShatterShard()
    {
        StopCoroutine(ShatterShardRoutine());
    }

    public override void Play()
    {
        this.gameObject.SetActive(true);
        m_hitbox.enabled = true;
        StartCoroutine(SpikeRoutine());
    }

    public override void Stop()
    {
        CallFXDone();
        CallPoolRequest();
    }

    public override void Pause()
    {

    }

    public override void SetFacing(HorizontalDirection horizontalDirection)
    {

    }
}
