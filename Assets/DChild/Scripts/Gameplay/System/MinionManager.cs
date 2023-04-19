using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Systems;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay
{
    public class MinionManager : MonoBehaviour, IGameplaySystemModule, IMinionManager
    {
        [SerializeField]
        private  List<ICombatAIBrain> m_registeredMinions = new List<ICombatAIBrain>();
        public  void Register(ICombatAIBrain minion)
        {
            m_registeredMinions.Add(minion);
        }
         public  void Unregister(ICombatAIBrain minion)
        {

            m_registeredMinions.Remove(minion);
        }
        [Button]
        public void showlist()
        {
            Debug.Log(m_registeredMinions.Count);
        }

    }
}
