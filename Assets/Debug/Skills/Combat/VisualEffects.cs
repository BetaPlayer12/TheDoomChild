using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using DChild.Inputs;
using Sirenix.OdinInspector;
using UnityEngine;

public class VisualEffects : MonoBehaviour, IPlayerExternalModule
{
    #region FX
    [SerializeField]
    [TabGroup("Fx")]
    private GameObject m_fxPrefab;
    [SerializeField]
    [TabGroup("Fx")]
    private int m_quantity;
    [SerializeField]
    [TabGroup("Fx")]
    private bool m_generateSlashFx;
    [SerializeField]
    [TabGroup("Fx")]
    private bool m_generateWhipFx;
    #endregion

    #region SpawnPoints
    [SerializeField]
    [TabGroup("SpawnPoints")]
    private Transform m_up;
    [SerializeField]
    [TabGroup("SpawnPoints")]
    private Transform m_forward;
    [SerializeField]
    [TabGroup("SpawnPoints")]
    private Transform m_down;
    #endregion

    private CombatController m_combatController;
    private CharacterPhysics2D m_physics;
    private PlayerInput m_input;

    private ICombatState m_combatState;
    private IPlayerState m_playerState;

    private int m_count;

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
            if (m_combatController.isMainHandPressed && m_generateSlashFx)
            {
                GenerateFx();
            }
            else if (m_combatController.isOffHandPressed && m_generateWhipFx)
            {
                GenerateFx();
            }
        }
        else
        {
            m_count = m_quantity;
        }
    }

    private void GenerateFx()
    {
        var spawnPos = transform.position;
        if (m_playerState.isCrouched)
        {
            spawnPos = m_down.position;
        }
        else if (m_input.direction.isUpHeld)
        {
            spawnPos = m_up.position;
        }
        else
        {
            spawnPos = m_forward.position;
        }

        if (m_count != 0)
        {
            Instantiate(m_fxPrefab, spawnPos, Quaternion.identity);
            m_count--;
        }
    }

    private void Start()
    {
        m_combatController = m_physics.GetComponentInChildren<CombatController>();
        m_input = GetComponentInParent<PlayerInput>();
    }
}
