using DChild.Gameplay.Systems;
using PixelCrushers.DialogueSystem;
using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.SpecialSkills.Modules
{
    [System.Serializable]
    public class WaitForDialogue : ISpecialSkillIEnumeratorModule
    {
        [SerializeField, ConversationPopup(true)]
        private string m_dialoguetitle = null;
        private bool m_activedialogue = false;
        public IEnumerator ApplyEffect(ArmyController owner, ArmyController target)
        {

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (BaseGameplayUIHandle.Instance == null)
            {
                yield break;
            }
#endif
            /*UI NOTE*/
            /*Cannot display the visuals and apply the effects due to the NullException found here.*/
            DialogueManager.instance.StartConversation(m_dialoguetitle, null, null, 0);
            DialogueManager.instance.conversationEnded += OnConversationEnd;

            m_activedialogue = true;
            while (m_activedialogue)
            {
                yield return null;
            }

            Debug.Log("Dialogue finish!");
            yield return true;
        }

        private void OnConversationEnd(Transform t)
        {
            m_activedialogue = false;
        }


        public IEnumerator RemoveEffect(ArmyController owner, ArmyController target)
        {
            yield return null;
        }

    }
}
