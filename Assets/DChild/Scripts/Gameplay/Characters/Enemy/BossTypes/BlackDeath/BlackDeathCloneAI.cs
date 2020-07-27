using DChild;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDeathCloneAI : AIBrain<BlackDeathCloneAI.Info>
{
    [System.Serializable]
    public class Info : BaseInfo
    {
        [Title("Animations")]
        [SerializeField, ValueDropdown("GetAnimations")]
        private string m_deathAnimation;
        public string deathAnimation => m_deathAnimation;

        [Title("Projectiles")]
        [SerializeField]
        private SimpleProjectileAttackInfo m_projectile;
        public SimpleProjectileAttackInfo projectile => m_projectile;
        [Title("Events")]
        [SerializeField, ValueDropdown("GetEvents")]
        private string m_deathFXEvent;
        public string deathFXEvent => m_deathFXEvent;

        public override void Initialize()
        {
#if UNITY_EDITOR
            m_projectile.SetData(m_skeletonDataAsset);
#endif
        }
    }

    [SerializeField]
    private SpineRootAnimation m_spine;
    [SerializeField]
    private Transform m_projectilePoint;
    [SerializeField]
    private SpineEventListener m_spineListener;
    [SerializeField]
    private DeathHandle m_deathHandle;
    [SerializeField]
    private ParticleFX m_deathFX;
    [SerializeField]
    private Damageable m_damageable;
#if UNITY_EDITOR
    [SerializeField]
    private SkeletonAnimation m_skeletonAnimation;

    public void InitializeField(SpineRootAnimation spineRoot, SkeletonAnimation animationF, SkeletonAnimation animationB)
    {
        m_spine = spineRoot;
    }
#endif
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_startAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_idleAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_endAnimation;

    private void OnDeath(object sender, EventActionArgs eventArgs)
    {
        StopAllCoroutines();
    }

    public void SetFacing(int faceValue)
    {
        transform.localScale = new Vector3(faceValue * transform.localScale.x, 1, 1);
        m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
    }

    private IEnumerator SpawnRoutine()
    {
        m_spine.SetAnimation(0, m_startAnimation, false);
        yield return new WaitForAnimationComplete(m_spine.animationState, m_startAnimation);
        StartCoroutine(SpawnBladesRoutine());
        yield return null;
    }

    private IEnumerator SpawnBladesRoutine()
    {
        while (true)
        {
            float angleStep = 360f / 8;
            float angle = 0f;
            m_spine.SetAnimation(0, m_endAnimation, false);
            yield return new WaitForAnimationComplete(m_spine.animationState, m_endAnimation);
            m_spine.SetAnimation(0, m_idleAnimation, true);
            for (int y = 0; y < 2; y++)
            {
                if (y == 1)
                {
                    angle = 69; //( ͡° ͜ʖ ͡°) hihi
                }
                for (int z = 0; z < 8; z++)
                {
                    Vector2 startPoint = new Vector2(m_projectilePoint.position.x, m_projectilePoint.position.y);
                    float projectileDirXposition = startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180) * 5;
                    float projectileDirYposition = startPoint.y + Mathf.Cos((angle * Mathf.PI) / 180) * 5;

                    Vector2 projectileVector = new Vector2(projectileDirXposition, projectileDirYposition);
                    Vector2 projectileMoveDirection = (projectileVector - startPoint).normalized * m_info.projectile.projectileInfo.speed;

                    GameObject projectile = m_info.projectile.projectileInfo.projectile;
                    var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(projectile);
                    instance.transform.position = m_projectilePoint.position;
                    var component = instance.GetComponent<Projectile>();
                    component.ResetState();
                    component.GetComponent<Rigidbody2D>().velocity = projectileMoveDirection;
                    Vector2 v = component.GetComponent<Rigidbody2D>().velocity;
                    var projRotation = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
                    component.transform.rotation = Quaternion.AngleAxis(projRotation, Vector3.forward);

                    angle += angleStep;
                }
                yield return new WaitForSeconds(.5f);
            }
            yield return new WaitForSeconds(.75f);
            m_spine.SetAnimation(0, m_startAnimation, true);
            yield return new WaitForSeconds(3.25f);
            yield return null;
        }
    }

    private void Start()
    {
        m_damageable.Destroyed += OnDeath;
        m_deathHandle.SetAnimation(m_info.deathAnimation);
        //m_spineListener.Subscribe(m_info.deathFXEvent, m_deathFX.Play);
        StartCoroutine(SpawnRoutine());
    }
}
