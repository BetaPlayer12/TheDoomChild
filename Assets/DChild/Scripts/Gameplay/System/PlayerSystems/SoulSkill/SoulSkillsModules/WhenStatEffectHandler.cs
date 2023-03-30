using DChild.Gameplay;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WhenStatEffectHandler : MonoBehaviour
{
    private enum Stats
    {
        HP,
        MP
    }

    private enum Comparison
    {
        Greater,
        Lesser
    }

    [SerializeField, LabelText("Stat")]
    private Stats m_toChange;
    [SerializeField]
    private bool m_isPercentage;
    [SerializeField, Wrap(1f, 100f), SuffixLabel("%", overlay: true)]
    private int m_value;
    [SerializeField, LabelText("Stat")]
    private Comparison m_comparison;
    [SerializeField]
    private ParticleSystem m_particleeffects;
    [SerializeField]
    private VisualEffect m_visualeffects;
    [SerializeField]
    private bool m_isparticle = false;
    private float m_maxhealth;
    private float m_maxmagic;
    private void Start()
    {
        if (m_isparticle == true)
        {
            m_particleeffects.Stop();
        }
        else
        {
            m_visualeffects.Stop();
        }
        if (m_toChange == Stats.HP)
        {
            m_maxhealth = GameplaySystem.playerManager.player.health.maxValue;
            GameplaySystem.playerManager.player.health.ValueChanged += OnStatChange;
        }
        else
        {
            m_maxmagic = GameplaySystem.playerManager.player.magic.maxValue;
            GameplaySystem.playerManager.player.magic.ValueChanged += OnStatChange;
        }
           
        this.transform.localPosition = new Vector3(0.0f, 8.0f, 0.0f);
    }
    private bool IsValid(float currentPercent)
    {
        float maxvalue;
        if (m_toChange == Stats.HP)
        {
            maxvalue = m_maxhealth;
        }
        else
        {
            maxvalue = m_maxmagic;
        }
        if (m_comparison == Comparison.Greater)
        {
            if (m_isPercentage)
            {
                maxvalue = maxvalue * (m_value / 100f);
                return currentPercent > maxvalue;

            }
            return currentPercent > m_value;
        }
        else
        {
            if (m_isPercentage)
            {
                maxvalue = maxvalue * (m_value / 100f);
                return currentPercent < maxvalue;

            }

            return currentPercent < m_value;


        }
    }
    private void OnStatChange(object sender, StatInfoEventArgs eventArgs)
    {
        
        if (IsValid(GameplaySystem.playerManager.player.health.currentValue))
        {
            if (m_isparticle == true)
            {
                m_particleeffects.Stop();
                m_particleeffects.Play();
            }
            else
            {
                m_visualeffects.Stop();
                m_visualeffects.Play();
            }
        }
        else
        {
            if (m_isparticle == true)
            {
                m_particleeffects.Stop();
               
            }
            else
            {
                m_visualeffects.Stop();
                
            }
        }
    }
}
