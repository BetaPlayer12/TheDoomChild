using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using DChild.Inputs;
using UnityEngine;

public class WhipForward : MonoBehaviour, IPlayerExternalModule
{
    [SerializeField]
    private Color m_color;
    private Color m_defaultColor = Color.white;

    private CombatController m_combatController;
    private CharacterPhysics2D m_physics;
    private PlayerInput m_input;

    private SpriteRenderer m_visual;
    private ICombatState m_combatState;
    private IPlayerState m_playerState;

    public void Initialize(IPlayerModules player)
    {
        m_playerState = player.characterState;
        m_combatState = player.characterState;
        m_physics = player.physics;
    }

    private void Update()
    {
        if (m_combatState.isAttacking)
        {
            if (m_combatController.isOffHandPressed)
            {
                if (m_playerState.isCrouched)
                {

                }
                else if (m_input.direction.isUpHeld)
                {

                }
                else
                {
                    m_visual.color = m_color;
                }
            }
        }
        else
        {
            m_visual.color = m_defaultColor;
        }
    }

    private void Start()
    {
        m_combatController = m_physics.GetComponentInChildren<CombatController>();
        m_input = GetComponentInParent<PlayerInput>();
        m_visual = GetComponentInChildren<SpriteRenderer>();
    }
}