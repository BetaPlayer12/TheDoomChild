using DChild;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Items;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class ShadowBee : MonoBehaviour
{
    [SerializeField]
    private float m_duration;
    [SerializeField]
    private float m_interval;
    [SerializeField]
    private float m_rotationSpeed;
    [SerializeField]
    private float m_radius;
    [SerializeField, BoxGroup("ShadowBee")]
    private List<GameObject> m_shadowBee;
    [SerializeField, BoxGroup("ShadowBee")]
    private GameObject m_shadowBeeParent;
    [SerializeField, BoxGroup("Projectile")]
    private ProjectileInfo m_projectileInfo;
    [SerializeField, BoxGroup("Projectile")]
    private List<Transform> m_launcherPoints;
    [SerializeField, BoxGroup("Animation")]
    private List<SpineRootAnimation> m_spineList;
#if UNITY_EDITOR
    [SerializeField]
    private SkeletonAnimation m_skeletonAnimation;

    public void InitializeField(SpineRootAnimation spineRoot)
    {
        for(int i = 0; i <= m_spineList.Count; i++)
        {
            m_spineList[i] = spineRoot;
        }
        
    }
#endif

    [SerializeField, BoxGroup("Animation"), Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_idleAnimation;
    [SerializeField, BoxGroup("Animation"), Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_idle2Animation;
    [SerializeField, BoxGroup("Animation"), Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_attackAnimation;
    [SerializeField, BoxGroup("Animation"), Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_deathAnimation;
    /*[SerializeField, BoxGroup("FX")]
    private List<ParticleFX> m_fxs;*/
    //[SerializeField]
    //private PoolableObject m_object;
    private ProjectileLauncher m_launcher;
    private SimpleAttackProjectile m_projectile;
    private ShadowPetHandler m_eventHandler;
    private Character m_parentCharacter;

    private Coroutine m_rotationControlRoutine;

    private float m_distanceBehindTargetX = 0f;

    private void Awake()
    {
        m_projectile = GetComponent<SimpleAttackProjectile>();
        m_launcher = new ProjectileLauncher(m_projectileInfo, m_launcherPoints[0]);
        m_eventHandler = GetComponent<ShadowPetHandler>();
    }

    private void Start()
    {
        m_parentCharacter = GetComponentInParent<Character>();
        StartCoroutine(ShadowBeeRoutine());
        m_rotationControlRoutine = StartCoroutine(RotationRoutine());
    }
    private void SetSpineAnimation(string animationName, bool loop)
    {
        foreach (var spine in m_spineList)
        {
            spine.SetAnimation(0, animationName, loop);
        }
    }

    private IEnumerator LaunchShadowBeeProjectiles()
    {
        yield return new WaitForSeconds(0.1f);
        SetSpineAnimation(m_attackAnimation, false);
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < m_launcherPoints.Count; i++)
        {
            m_launcher = new ProjectileLauncher(m_projectileInfo, m_launcherPoints[i]);
            var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(m_projectileInfo.projectile);
            instance.transform.position = m_launcherPoints[i].position;
            if (GetComponentInParent<Character>() != null)
            { 
                instance.GetComponent<Attacker>().SetParentAttacker(GetComponentInParent<Character>().GetComponent<Attacker>());
            }
            //SetSpineAnimation(m_attackAnimation, false);
            //yield return new WaitForSeconds(0.3f);
            if (GetComponentInParent<Character>() != null)
            {
                m_launcher.AimAt(new Vector2(m_launcherPoints[i].position.x + (GetComponentInParent<Character>().facing == HorizontalDirection.Right ? 5 : -5), m_launcherPoints[i].position.y));
            }
            else
            {
                m_launcher.AimAt(new Vector2(m_launcherPoints[i].position.x + 5, m_launcherPoints[i].position.y));
            }
            m_launcher.LaunchProjectile(m_launcherPoints[i].right, instance.gameObject);
            
        }
        
        yield return null;
    }

    private IEnumerator ShadowBeeRoutine()
    {
        /* SetSpineAnimation(m_idle2Animation, false);
         yield return new WaitForAnimationComplete(m_skeletonAnimation.AnimationState, m_idle2Animation);*/
        var timer = m_duration;
        var interval = m_interval;
       
        StartCoroutine(LaunchShadowBeeProjectiles());
        while (timer > 0)
        {
            SetSpineAnimation(m_idleAnimation, true);
            timer -= Time.deltaTime;
            interval -= Time.deltaTime;
            if (interval < 0)
            {
                StartCoroutine(LaunchShadowBeeProjectiles());

                interval = m_interval;
            }
            yield return null;
        }
        /* for (int i = 0; i < m_fxs.Count; i++)
         {
             m_fxs[i].Stop();
         }*/
        StartCoroutine(LaunchShadowBeeProjectiles());
        yield return new WaitForSeconds(0.3f);
        if (m_rotationControlRoutine != null)
        {
            StopCoroutine(m_rotationControlRoutine);
            m_rotationControlRoutine = null;
        }
        SetSpineAnimation(m_idleAnimation, false);
        yield return new WaitForSeconds(0.5f);
        SetSpineAnimation(m_deathAnimation, false);
        yield return new WaitForAnimationComplete(m_spineList[0].animationState, m_deathAnimation);
        /*m_rotationControl.localRotation = Quaternion.identity;*/
        m_eventHandler.PetDesummon();
        m_projectile.CallPoolRequest();
        yield return null;
    }

    
    private IEnumerator RotationRoutine()
    {
        var angle = 0f;
        var offset = 0f;
        var smoothingFactor = 5f;
        var heightOffset = 7f;
        var someThreshold = 5f;
        
        var targetObject = m_parentCharacter;

        if (m_parentCharacter.transform.localScale.x == 1)
        {
            var shadowBeeScale = transform.localScale;
            shadowBeeScale.x *= -1f;
            for (int i = 0; i < m_shadowBee.Count; i++)
            {
                m_shadowBee[i].transform.localScale = shadowBeeScale;
            }
        }

        while (true)
        {
            angle -= m_rotationSpeed * Time.deltaTime;
            offset -= m_rotationSpeed * Time.deltaTime;

            Vector3 targetPosition = targetObject.transform.position;
            Vector3 behindPosition = targetPosition - targetObject.transform.right * m_distanceBehindTargetX + Vector3.up * heightOffset;

            for (int i = 0; i < m_shadowBee.Count; i++)
            {
                float angleOffset = 2 * Mathf.PI * i / m_shadowBee.Count;
                float x = Mathf.Cos(angle + offset + angleOffset) * m_radius;
                float y = Mathf.Sin(angle + offset + angleOffset) * m_radius;

                Vector3 finalPosition = behindPosition + new Vector3(x, y, 0);

                bool overlaps = false;

                for (int j = 0; j < m_shadowBee.Count; j++)
                {
                    if (i != j && Vector3.Distance(finalPosition, m_shadowBee[j].transform.position) < someThreshold)
                    {
                        overlaps = true;
                        break;
                    }
                }

                m_shadowBee[i].transform.position = overlaps ? finalPosition : Vector3.Lerp(m_shadowBee[i].transform.position, finalPosition, smoothingFactor);
            }
            yield return null;
        }
    }

    
}
