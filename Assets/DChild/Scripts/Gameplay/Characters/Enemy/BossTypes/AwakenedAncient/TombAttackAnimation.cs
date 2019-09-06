using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class TombAttackAnimation : CombatCharacterAnimation
    {
        #region "Animation Names"
        public const string ANIMATION_TOMBA_IDLE = "TombA_Idle";
        public const string ANIMATION_TOMBA_RISE = "TombA_Rise";
        public const string ANIMATION_TOMBB_IDLE = "TombB_Idle";
        public const string ANIMATION_TOMBB_RISE = "TombB_Rise";
        public const string ANIMATION_TOMBC_IDLE = "TombC_Idle";
        public const string ANIMATION_TOMBC_RISE = "TombC_Rise";
        #endregion

        public void DoTombAIdle()
        {
            SetAnimation(0, ANIMATION_TOMBA_IDLE, true);
        }

        public void DoTombARise()
        {
            SetAnimation(0, ANIMATION_TOMBA_RISE, false);
            AddAnimation(0, ANIMATION_TOMBA_IDLE, true, 0);
        }

        public void DoTombFall()
        {
            SetAnimation(0, ANIMATION_TOMBA_RISE, false);
            AddAnimation(0, ANIMATION_TOMBA_IDLE, true, 0);
        }

        public void DoTombBIdle()
        {
            SetAnimation(0, ANIMATION_TOMBB_IDLE, true);
        }

        public void DoTombBRise()
        {
            SetAnimation(0, ANIMATION_TOMBB_RISE, false);
            AddAnimation(0, ANIMATION_TOMBB_IDLE, true, 0);
        }

        public void DoTombCIdle()
        {
            SetAnimation(0, ANIMATION_TOMBC_IDLE, true);
        }

        public void DoTombCRise()
        {
            SetAnimation(0, ANIMATION_TOMBC_RISE, false);
            AddAnimation(0, ANIMATION_TOMBC_IDLE, true, 0);
        }

        public void DoTombRise(int num)
        {
            if(num == 0)
            {
                DoTombARise();
            }
            else if (num == 1)
            {
                DoTombBRise();
            }
            else if (num == 2)
            {
                DoTombCRise();
            }
        }
    }
}
