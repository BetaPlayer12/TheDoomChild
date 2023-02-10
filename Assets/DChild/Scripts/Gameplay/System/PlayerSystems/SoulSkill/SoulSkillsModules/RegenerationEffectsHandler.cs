using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenerationEffectsHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject m_health;
    [SerializeField]
    private GameObject m_shadow;


    public void HealthRegenEffect(bool healthactive)
    {
        if (healthactive == true)
        {
            m_health.SetActive(true);
        }
        else
        {
            m_health.SetActive(false);
        }
        
    }
    

    public void ShadowRegenEffect (bool shadowactive)
    {
        if (shadowactive == true)
        {
            m_shadow.SetActive(true);
        }
        else
        {
            m_shadow.SetActive(false);
        }
    }
   
}
