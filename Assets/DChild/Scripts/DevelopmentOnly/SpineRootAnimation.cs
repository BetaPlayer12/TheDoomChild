/***********************************************
 * 
 * Base Animation class for All Character Animations
 * Should Contain functions that will be present to all
 * Types of Characters
 * 
 * Child Classess should have functions that does not care about 
 * Transistion from one state to another
 * 
 ***********************************************/

using System;
using Spine.Unity.Modules;

namespace DChild.Gameplay.Characters
{

    public class SpineRootAnimation : SpineAnimation
    {
        private SpineRootMotion m_rootMotion;

        public void EnableRootMotion(bool useX, bool useY)
        {
            m_rootMotion.enabled = true;
            m_rootMotion.useX = useX;
            m_rootMotion.useY = useY;
        }

        public void DisableRootMotion() => m_rootMotion.enabled = false;

        protected virtual void Awake()
        {
            m_rootMotion = GetComponentInChildren<SpineRootMotion>(true);
        }
    }
}