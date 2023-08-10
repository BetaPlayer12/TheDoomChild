using DChild;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Projectiles;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EelecktrickFX : MonoBehaviour
{
    [SerializeField, BoxGroup("Reference")]
    private float m_deathTimer;
    [SerializeField, BoxGroup("Reference")]
    private Projectile m_projectile;
    [SerializeField, BoxGroup("Reference")]
    private Rigidbody2D m_righidybody2D;
    [SerializeField, BoxGroup("Reference")]
    private Collider2D m_hurtbox;
    [SerializeField, BoxGroup("FX")]
    private ParticleSystem m_impactFX;
    [SerializeField, BoxGroup("Animation")]
    private SpineRootAnimation m_spine;
#if UNITY_EDITOR
    [SerializeField]
    private SkeletonAnimation m_skeletonAnimation;
#endif
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation"), BoxGroup("Animation")]
    private string m_eelecktrickStartAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation"), BoxGroup("Animation")]
    private string m_eelecktrickLoopAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation"), BoxGroup("Animation")]
    private string m_eelecktrickEndAnimation;

    private Coroutine m_startAnimationRoutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (DChildUtility.IsAnEnvironmentLayerObject(collision.gameObject))
        {
            if (m_startAnimationRoutine != null)
            {
                StopCoroutine(m_startAnimationRoutine);
                m_startAnimationRoutine = null;
            }
            StartCoroutine(PoolRoutine());
        }
    }

    private IEnumerator StartAnimationRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        this.transform.localScale = Vector3.one;
        m_spine.SetEmptyAnimation(0, 0);
        m_spine.SetAnimation(0, m_eelecktrickStartAnimation, false);
        yield return new WaitForAnimationComplete(m_spine.animationState, m_eelecktrickStartAnimation);
        m_spine.SetAnimation(0, m_eelecktrickLoopAnimation, true);
        yield return null;
    }

    private IEnumerator PoolRoutine()
    {
        m_hurtbox.enabled = false;
        m_righidybody2D.velocity = Vector2.zero;
        m_spine.SetEmptyAnimation(0, 0);
        m_spine.SetAnimation(0, m_eelecktrickEndAnimation, false);
        yield return new WaitForAnimationComplete(m_spine.animationState, m_eelecktrickEndAnimation);
        m_impactFX.Play();
        //yield return new WaitForSeconds(m_deathTimer);
        //m_projectile.CallPoolRequest();
        yield return null;
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        m_hurtbox.enabled = true;
        m_startAnimationRoutine = StartCoroutine(StartAnimationRoutine());
    }
}
