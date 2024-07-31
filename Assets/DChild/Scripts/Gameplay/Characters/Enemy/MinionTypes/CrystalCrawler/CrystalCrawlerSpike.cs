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

public class CrystalCrawlerSpike : MonoBehaviour
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
    private BoxCollider2D m_hitbox2;
    [SerializeField, BoxGroup("Spike")]
    private float m_spikeDuration;
    [SerializeField]
    private GameObject m_shatterFXGO;

    private bool m_boolFixesEverythingByStephenBibangco;
    // Start is called before the first frame update
    //private void Start()
    //{
    //    m_spineListener.Subscribe(m_event, m_dustFX.Play);
    //    m_spineListener.Subscribe(m_event, m_explodeFX.Play);
    //}

    [SerializeField, PreviewField]
    private SkeletonDataAsset m_skeletonDataAsset;

    private bool isHit;
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

    private void Start()
    {
        StartCoroutine(SpikeRoutine());
    }
    private IEnumerator SpikeRoutine()
    {
        //m_dustFX?.Play();

        //var explodFX = GameSystem.poolManager.GetPool<FXPool>().GetOrCreateItem(m_explodeFXGO);
        //explodFX.Play();
        //explodFX.transform.position = transform.position;
        m_audio.Play();
        //this.gameObject.SetActive(true);
            m_spine.SetAnimation(0, m_startAnimation, false);
            m_hitbox.enabled = true;
            m_hitbox2.enabled = true;
            yield return new WaitForAnimationComplete(m_spine.animationState, m_startAnimation);
            m_spine.SetAnimation(0, m_loopAnimation, true);
            yield return new WaitForSeconds(m_spikeDuration);
            m_hitbox.enabled = false;
            m_hitbox2.enabled = false;
         if(m_boolFixesEverythingByStephenBibangco == false) {
            m_spine.SetAnimation(0, m_endAnimation, false);
        }
        else
        {

            m_spine.SetEmptyAnimation(0, 0);
            m_boolFixesEverythingByStephenBibangco = false;
        }
        
         yield return new WaitForSeconds(5f);
            Destroy(this.gameObject);    
    }
   
    private IEnumerator ShatterShardRoutine()
    {
        m_boolFixesEverythingByStephenBibangco = true;
       // GameObject shatter = m_shatterFXGO;
        var shatterFX = Instantiate(m_shatterFXGO, transform.position, Quaternion.identity);
        m_hitbox.enabled = false;
        m_hitbox2.enabled = false;
        //m_spine.SetAnimation(0, m_shattterAnimation, false);
        //yield return new WaitForAnimationComplete(m_spine.animationState, m_shattterAnimation);
        m_spine.SetEmptyAnimation(0, 0);
        shatterFX.transform.position = transform.position;
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
        Destroy(shatterFX.gameObject);
    }
    
    public void HideObject()
    {
        StopAllCoroutines();
        this.gameObject.SetActive(false);
    }
    public void stopShatterFX()
    {
        var shatterFX = GameSystem.poolManager.GetPool<FXPool>().GetOrCreateItem(m_shatterFX);
        shatterFX.Stop();
        Destroy(shatterFX.gameObject);

    }

    public void PlayShatterShard()
    {
        StartCoroutine(ShatterShardRoutine());
    }
    public void StopShatterShard()
    {
        StopCoroutine(ShatterShardRoutine());
    }

  
}
