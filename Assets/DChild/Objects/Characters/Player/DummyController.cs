using Sirenix.OdinInspector;
using UnityEngine;
using DChild.Gameplay.Characters;
using DChild.Gameplay;
using DChild.Gameplay.Combat;

public class DummyController : MonoBehaviour
{
    [SerializeField, TabGroup("PlayerStats")]
    private float m_moveSpeed;
    [SerializeField, TabGroup("PlayerStats")]
    private float m_jumpPower;
    [SerializeField, TabGroup("Reference")]
    protected Character m_character;
    [SerializeField, TabGroup("Reference")]
    protected SpineRootAnimation m_animation;
    [SerializeField, TabGroup("Reference")]
    private Transform m_capeTransformReference;
    [SerializeField, TabGroup("Reference")]
    private GameObject m_attackHitBox;
    [SerializeField, TabGroup("Reference")]
    private Damageable m_damageable;
    [SerializeField, TabGroup("Reference")]
    private CollisionRegistrator m_collisionRegistrator;
    [SerializeField, TabGroup("Behaviours")]
    private MovementHandle2D m_movement;
    [SerializeField, TabGroup("Behaviours")]
    private AnimatedTurnHandle m_turnHandle;

    private IsolatedPhysics2D m_physics;

    private void Awake()
    {
        m_physics = GetComponent<IsolatedPhysics2D>();
    }

    private void Start()
    {
        m_attackHitBox.SetActive(false);
    }

    private void Update()
    {

        //m_character.SetFacing(transform.localScale == Vector3.one ? HorizontalDirection.Right : HorizontalDirection.Left);
        if (GetComponent<IsolatedCharacterPhysics2D>().inContactWithGround)
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                m_movement.MoveTowards(new Vector2(Input.GetAxisRaw("Horizontal"), m_physics.velocity.y), m_moveSpeed);
            }
            else
            {
                m_movement.Stop();
            }

            if (Input.GetButtonDown("Jump"))
            {
                m_physics.AddForce(Vector2.up * m_jumpPower, ForceMode2D.Impulse);
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            m_collisionRegistrator.ResetHitCache();
        }
        if (Input.GetButton("Fire1"))
        {
            m_attackHitBox.SetActive(true);
        }
        else
        {
            m_attackHitBox.SetActive(false);
        }

        if (m_character.facing == HorizontalDirection.Right && Input.GetAxisRaw("Horizontal") < 0)
        {
            m_character.SetFacing(HorizontalDirection.Left);
            //transform.localScale = Vector3.one;
        }
        else if (m_character.facing == HorizontalDirection.Left && Input.GetAxisRaw("Horizontal") > 0)
        {
            m_character.SetFacing(HorizontalDirection.Right);
            //transform.localScale = new Vector3(-1, 1, 1);
        }
        transform.localScale = m_character.facing == HorizontalDirection.Right ? transform.localScale = new Vector3(-1, 1, 1) : Vector3.one;
        m_capeTransformReference.localScale = new Vector3(-transform.localScale.x, 1, 1);
    }
}
