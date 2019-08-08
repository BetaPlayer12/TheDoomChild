using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters;
using DChild.Gameplay;
using Holysoft.Event;
using UnityEngine;

public class ShadowCollider : MonoBehaviour
{
    [SerializeField]
    private LayerMask m_mask;
    [SerializeField]
    private Transform m_bodyTransform;
    [SerializeField]
    private float m_castXExtent;
    [SerializeField]
    private float m_castYExtent;

    private Skills m_skills;
    private CharacterColliders m_colliders;
    private Collider2D m_wall;
    private Vector2 m_overlapSize;

    private IShadow m_shadowState;

    private void Update()
    {
        if (m_shadowState.hasMorphed)
        {
            m_overlapSize = m_bodyTransform.localScale;
            m_overlapSize.x = m_bodyTransform.localScale.x + m_castXExtent;
            m_overlapSize.y = m_bodyTransform.localScale.y + m_castYExtent;

            Collider2D[] hit = Physics2D.OverlapBoxAll(m_bodyTransform.position, m_overlapSize, 0f, m_mask.value);

            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i] != null)
                {
                    if (hit[i].tag == "ShadowPassable")
                    {
                        PassWall(hit[i]);
                        m_skills.Enable(MovementSkill.WallJump, false); // to avoid wallsticking when passing walls using shadowmorph
                    }

                    else
                    {
                        m_skills.Enable(MovementSkill.WallJump, true);
                    }
                }
            }
        }
        else
        {
            OnIgnoreColliderEnd(this, EventActionArgs.Empty);
        }
    }

    private void PassWall(Collider2D collider)
    {
        if (m_shadowState.hasMorphed)
        {
            m_wall = collider;
            m_colliders.IgnoreCollider(collider);

        }
    }

    private void OnIgnoreColliderEnd(object sender, EventActionArgs eventArgs)
    {
        m_skills.Enable(MovementSkill.WallJump, true);
        if (m_wall != null)
        {
            m_colliders.ClearIgnoredCollider(m_wall);
            m_wall = null;
        }
    }

    private void Start()
    {
        m_shadowState = GetComponentInParent<CharacterPhysics2D>().GetComponentInChildren<IShadow>();
        m_colliders = GetComponentInChildren<CharacterColliders>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(m_bodyTransform.position, m_overlapSize);
    }
}
