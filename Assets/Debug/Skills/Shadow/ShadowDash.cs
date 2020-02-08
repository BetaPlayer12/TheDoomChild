using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Systems.WorldComponents;
using Holysoft.Collections;
using Holysoft.Event;
using Refactor.DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Skill
{
    public class ShadowDash : ShadowSkill
    {
        [SerializeField]
        private CountdownTimer m_cooldownDuration;
        [ShowInInspector]
        private bool m_canShadowDash;
        [ShowInInspector]
        private bool m_isCoolingOff;

        private IShadow m_shadow;
        private IDashState m_state;
        private IIsolatedTime m_time;
        private bool m_hasShadowDashed;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);
            m_state = info.state;
            m_time = info.character.isolatedObject;
        }

        protected override void UseSkill()
        {
            base.UseSkill();
            m_shadow.BecomeAShadow(false);
            m_hasShadowDashed = true;
        }

        private void OnDashEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_hasShadowDashed)
            {
                m_shadow.BecomeNormal();
                m_canShadowDash = false;
                m_hasShadowDashed = false;
                m_cooldownDuration.Reset();
                enabled = true;
            }
        }

        private void OnDashStart(object sender, EventActionArgs eventArgs)
        {
            if (m_canShadowDash)
            {
                DoSkill();
            }
        }

        private void OnCooldownEnd(object sender, EventActionArgs eventArgs)
        {
            enabled = false;
            m_canShadowDash = true;
        }

        private void Start()
        {
            m_shadow = GetComponent<IShadow>();
            m_canShadowDash = true;
            var dashes = GetComponentsInParent<Dash>();
            for (int i = 0; i < dashes.Length; i++)
            {
                //dashes[i].DashStart += OnDashStart;
                //dashes[i].DashEnd += OnDashEnd;
            }
            m_cooldownDuration.CountdownEnd += OnCooldownEnd;
            enabled = false;
        }

        private void Update()
        {
            m_cooldownDuration.Tick(m_time.deltaTime);
        }
    }

}