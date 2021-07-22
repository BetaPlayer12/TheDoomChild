using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChildDebug.Window
{
    public class ModifyDamage : MonoBehaviour, IToggleDebugBehaviour, ITrackableValue
    {
        [SerializeField]
        public float m_damage = 999999;

        public bool value => GameplaySystem.playerManager.player.modifiers.Get(PlayerModifier.AttackDamage) >= m_damage;

        float ITrackableValue.value => GameplaySystem.playerManager.player.modifiers.Get(PlayerModifier.AttackDamage);

        public event EventAction<EventActionArgs> ValueChange;

        [Button]
        public void AddDamage()
        {
            float atk= GameplaySystem.playerManager.player.modifiers.Get(PlayerModifier.AttackDamage);
            if(atk >= 85)
            {

            }
            else
            {
                GameplaySystem.playerManager.player.modifiers.Add(PlayerModifier.AttackDamage, m_damage);
            }
           

        }
        [Button]
        public void ReduceDamage()
        {
            float atk = GameplaySystem.playerManager.player.modifiers.Get(PlayerModifier.AttackDamage);
            if (atk >= 25)
            {
                GameplaySystem.playerManager.player.modifiers.Add(PlayerModifier.AttackDamage, -m_damage);
            }
            else
            {

            }
               

        }

        public void SetToNormal()
        {
            GameplaySystem.playerManager.player.modifiers.Set(PlayerModifier.AttackDamage, 1);
        }

        private void OnChange(object sender, ModifierChangeEventArgs eventArgs)
        {
            ValueChange?.Invoke(this,EventActionArgs.Empty);
        }

        private void Awake()
        {
            GameplaySystem.playerManager.player.modifiers.ModifierChange += OnChange;
        }

       
    }
}
