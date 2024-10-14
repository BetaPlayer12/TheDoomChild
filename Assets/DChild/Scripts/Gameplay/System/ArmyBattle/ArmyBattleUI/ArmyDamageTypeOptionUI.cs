using Doozy.Runtime.UIManager.Components;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.UI
{

    public class ArmyDamageTypeOptionUI : MonoBehaviour
    {
        [SerializeField]
        private DamageType m_damageType;

        private UIButton m_button;

        public DamageType damageType => m_damageType;
        //Put in Details of UI

        public void SetType(DamageType damageType)
        {
            m_damageType = damageType;

            Debug.Log($"chosen dmg type: {m_damageType}");
        }

        public void SetInteractability(bool isInteractable)
        {
            m_button.interactable = isInteractable;
        }

        private void Awake()
        {
            m_button = GetComponent<UIButton>();
        }
    }
}