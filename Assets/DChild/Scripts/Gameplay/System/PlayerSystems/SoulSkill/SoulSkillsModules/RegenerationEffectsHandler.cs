using DChild.Gameplay.Characters.Players.SoulSkills;
using DChild.Gameplay.UI;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenerationEffectsHandler : MonoBehaviour
{
    [SerializeField]
    private HealthRegenUIFX m_health;
    [SerializeField]
    private ShadowRegenBoostUIFX m_shadow;

    public void SetHealthRegenReference(PassiveRegeneration.Handle handle)
    {
        m_health.SetReference(handle);
    }

    public void HealthRegenEffect(bool healthactive)
    {
        if (healthactive == true)
        {
            m_health.Enable();
        }
        else
        {
            m_health.Disable();
        }

    }


    public void ShadowRegenEffect(bool shadowactive)
    {
        if (shadowactive == true)
        {
            m_shadow.Enable();
        }
        else
        {
            m_shadow.Disable();
        }
    }

    private void Awake()
    {
        HealthRegenEffect(false);
        ShadowRegenEffect(false);
    }

}
