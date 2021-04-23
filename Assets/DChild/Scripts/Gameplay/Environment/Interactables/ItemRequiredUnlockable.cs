using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Environment.Interractables;
using DChild.Serialization;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{

    public class ItemRequiredUnlockable : MonoBehaviour, IButtonToInteract, ISerializableComponent
    {
        [System.Serializable]
        public struct SaveData : ISaveData
        {
            [SerializeField]
            private bool m_isUnlocked;

            public SaveData(bool isUnlocked)
            {
                m_isUnlocked = isUnlocked;
            }

            public bool isUnlocked => m_isUnlocked;

            ISaveData ISaveData.ProduceCopy() => new SaveData(m_isUnlocked);
        }

        [SerializeField]
        private Vector3 m_promptOffset;
        [SerializeField]
        private ItemRequirement m_itemRequirement;
        [SerializeField, OnValueChanged("OnUnlockedChanged")]
        private bool m_isUnlocked;

        [TabGroup("Main", "Serialized")]

        [SerializeField, TabGroup("Main/Serialized", "Unlocked")]
        private UnityEvent m_alreadyUnlocked;
        [SerializeField, TabGroup("Main/Serialized", "Locked")]
        private UnityEvent m_alreadyLocked;

        [TabGroup("Main", "Transistion")]

        [SerializeField, TabGroup("Main/Transistion", "Invalid Attempt")]
        private UnityEvent m_invalidAttempt;
        [SerializeField, TabGroup("Main/Transistion", "Unlock")]
        private UnityEvent m_onUnlock;

        public bool showPrompt => true;

        public Vector3 promptPosition => transform.position + m_promptOffset;

        public string promptMessage => throw new System.NotImplementedException();

        public void Interact(Character character)
        {
            var inventory = character.GetComponent<PlayerControlledObject>().owner.inventory;
            if (m_itemRequirement.HasAllItems(inventory))
            {
                m_itemRequirement.ConsumeItems(inventory);
                m_onUnlock?.Invoke();
                m_isUnlocked = true;
            }
            else
            {
                m_invalidAttempt?.Invoke();
            }
        }

        public void Load(ISaveData data)
        {
            m_isUnlocked = ((SaveData)data).isUnlocked;
            if (m_isUnlocked)
            {
                m_alreadyUnlocked?.Invoke();
            }
            else
            {
                m_alreadyLocked?.Invoke();
            }
        }

        public ISaveData Save() => new SaveData(m_isUnlocked);

        private void OnDrawGizmosSelected()
        {
            var position = promptPosition;
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(position, 1f);
        }

#if UNITY_EDITOR
        private void OnUnlockedChanged()
        {
            if (m_isUnlocked)
            {
                m_alreadyUnlocked?.Invoke();
            }
            else
            {
                m_alreadyLocked?.Invoke();
            }
        }

        [Button, HideInEditorMode, HideIf("m_isUnlocked")]
        public void CallInvalidAttempt()
        {
            m_invalidAttempt?.Invoke();
        }

        [Button, HideInEditorMode, HideIf("m_isUnlocked")]
        public void Unlock()
        {
            m_onUnlock?.Invoke();
            m_isUnlocked = true;
        }
#endif
    }
}