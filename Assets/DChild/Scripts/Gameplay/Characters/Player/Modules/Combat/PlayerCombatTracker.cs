using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using Holysoft.Collections;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public class PlayerCombatTracker : MonoBehaviour, IPlayerExternalModule
    {
        [SerializeField]
        private float m_combatRadius;
        [SerializeField]
        private CountdownTimer m_checkInterval;
        private ICombatState m_state;
        private IPlayer m_player;
        private ContactFilter2D m_filter;
        private RaycastHit2D[] m_hitbuffer;

        public void Initialize(IPlayerModules player)
        {
            m_state = player.characterState;
            m_player = (IPlayer)player;
            m_player.Attacks += OnEnterCombat;
            m_player.Damaged += OnEnterCombat;
        }

        private void OnEnterCombat(object sender, EventActionArgs eventArgs)
        {
            if (m_state.inCombat == false)
            {
                m_checkInterval.Reset();
                m_state.inCombat = true;
            }
            enabled = true;
        }

        private void OnCheckIntervalEnd(object sender, EventActionArgs eventArgs)
        {
            var hitCount = Physics2D.CircleCast(m_player.position, m_combatRadius, Vector2.one, m_filter, m_hitbuffer);
            if(hitCount == 0)
            {
                m_state.inCombat = false;
                enabled = false;
            }
            else
            {

            }
        }

        private void Awake()
        {
            m_filter.useTriggers = false;
            m_filter.useLayerMask = true;
            m_filter.SetLayerMask(LayerMask.NameToLayer("Enemy"));
            m_hitbuffer = new RaycastHit2D[16];
            m_checkInterval.CountdownEnd += OnCheckIntervalEnd;
            enabled = false;
        }

        private void LateUpdate()
        {
            m_checkInterval.Tick(GameplaySystem.time.deltaTime);
        }
    }
}