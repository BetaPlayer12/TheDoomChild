using UnityEngine;

namespace DChild.Gameplay.Environment.Interractables
{
    public abstract class MultiLockDoorIndicator : MonoBehaviour
    {
        [SerializeField]
        protected MultiLockDoor m_reference;
        protected abstract void SetIndication(int currentUnlockCount, bool isOpen);
        private void OnLockChange(object sender, MultiLockDoor.UnlockEvent eventArgs)
        {
            SetIndication(eventArgs.currentUnlockCount, eventArgs.isOpen);
        }

        private void Awake()
        {
            m_reference.OnLockChange += OnLockChange;
        }
    }

}