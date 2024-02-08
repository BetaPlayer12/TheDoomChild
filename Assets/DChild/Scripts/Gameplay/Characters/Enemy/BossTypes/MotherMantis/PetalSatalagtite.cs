using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetalSatalagtite : MonoBehaviour
{
    [SerializeField]
    private struct Info
    {
        //Animations
        [SerializeField, ValueDropdown("GetAnimations")]
        private string m_growAnimation;
        public string growAnimation => m_growAnimation;
        [SerializeField, ValueDropdown("GetAnimations")]
        private string m_idleAnimation;
        public string idleAnimation => m_idleAnimation;
        [SerializeField, ValueDropdown("GetAnimations")]
        private string m_deathAnimation;
        public string deathAnimation => m_deathAnimation;
        [SerializeField, ValueDropdown("GetAnimations")]
        private string m_death2Animation;
        public string death2Animation => m_death2Animation;
        [SerializeField, ValueDropdown("GetAnimations")]
        private string m_flinchAnimation;
        public string flinchAnimation => m_flinchAnimation;
        [SerializeField, ValueDropdown("GetAnimations")]
        private string m_flinch2Animation;
        public string flinch2Animation => m_flinch2Animation;
    }

    private Info m_info;

    private IEnumerator GrowthRoutine()
    {
        yield return null;
    }

    void Start()
    {
        
    }

   
    void Update()
    {
        
    }
}
