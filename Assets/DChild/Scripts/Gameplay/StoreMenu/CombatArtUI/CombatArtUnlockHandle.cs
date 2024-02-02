using DChild.Gameplay.Characters.Players;
using Doozy.Runtime.UIManager.Components;
using System;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.UI.CombatArts
{


    [System.Serializable]
    public class CombatArtUnlockHandle : MonoBehaviour
    {
        private CombatArtList m_referenceList;
        private Characters.Players.CombatArts m_progress;

        private CombatArt m_artToUnlock;
        private int m_levelToUnlock;

        [SerializeField, Min(0.1f)]
        private float m_holdToUnlockDuration = 0.01f;
        [SerializeField]
        private UIButton m_unlockButton;
        [SerializeField]
        private CombatArtUnlockProgressor m_progressor;

        private float m_unlockProgress;

        public event Action UnlockSuccessful;

        public void InitializeReferences(Characters.Players.CombatArts progress, CombatArtList artList)
        {
            m_progress = progress;
            m_referenceList = artList;
        }

        public void ResetUnlockProgress()
        {
            StopAllCoroutines();
            m_unlockProgress = 0;
            m_progressor.DisplayProgress(0f);
        }

        public void StartUnlockProgress()
        {
            StopAllCoroutines();
            StartCoroutine(UnlockProgressRoutine());
        }

        public void UnlockCombatArt(CombatArt art, int level)
        {
            m_progress.SetAbilityLevel(art, level);
        }

        public void VerifyUnlockFunction(CombatArtSelectButton reference)
        {
            var isUnlockable = reference.currentState == CombatArtUnlockState.Unlockable;
            m_unlockButton.interactable = isUnlockable;
            if (isUnlockable)
            {
                m_artToUnlock = reference.skillUnlock;
                m_levelToUnlock = reference.unlockLevel;
            }
        }

        public void DisableUnlockFunction()
        {
            m_unlockButton.interactable = false;
        }

        private IEnumerator UnlockProgressRoutine()
        {
            do
            {
                m_unlockProgress += Time.unscaledDeltaTime;
                m_progressor.DisplayProgress(m_unlockProgress / m_holdToUnlockDuration);
                yield return null;
            } while (m_unlockProgress < m_holdToUnlockDuration);

            UnlockCombatArt(m_artToUnlock, m_levelToUnlock);
            UnlockSuccessful?.Invoke();
        }
    }

}