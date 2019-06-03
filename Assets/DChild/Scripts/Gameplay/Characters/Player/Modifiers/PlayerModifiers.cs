using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DChild.Gameplay.Characters.Players
{
    public interface IWallStickModifier
    {
        float stickDuration { get; set; }
    }

    public interface IShadowSkillModifier
    {
        float shadowMagicRequirement { get; set; }
    }

    public interface IDamageModifier
    {
        float critDamageModifier { get; set; }
        int damageModifier { get; set; }
    }

    [System.Serializable]
    public class PlayerModifiers : IMagicModifier, IDashModifier, IWallStickModifier, IShadowSkillModifier, IJumpModifier, IDamageModifier
    {
        [SerializeField, DisableInEditorMode, Min(0)]
        private float m_critDamageModifier = 1;
        [SerializeField, DisableInEditorMode, Min(0)]
        private int m_damageModifier = 0;
        [SerializeField, DisableInEditorMode, Min(0)]
        private float m_magicRequirement = 1;
        [SerializeField, DisableInEditorMode, Min(0)]
        private float m_dashDistance = 1;
        [SerializeField, DisableInEditorMode, Min(0)]
        private float m_dashCooldown = 1;
        [SerializeField, DisableInEditorMode, Min(0)]
        private float m_stickDuration = 1;
        [SerializeField, DisableInEditorMode, Min(0)]
        private float m_shadowMagicRequirement = 1;
        [SerializeField, DisableInEditorMode, Min(0)]
        private float m_jumpPower = 1;


        public float critDamageModifier { get => m_critDamageModifier; set => m_critDamageModifier = value; }
        public int damageModifier { get => m_damageModifier; set => m_damageModifier = value; }
        public float magicRequirement { get => m_magicRequirement; set => m_magicRequirement = value; }
        public float dashDistance { get => m_dashDistance; set => m_dashDistance = value; }
        public float dashCooldown { get => m_dashCooldown; set => m_dashCooldown = value; }
        public float stickDuration { get => m_stickDuration; set => m_stickDuration = value; }
        public float shadowMagicRequirement { get => m_shadowMagicRequirement; set => m_shadowMagicRequirement = value; }
        public float jumpPower { get => m_jumpPower; set => m_jumpPower = value; }
    }
}