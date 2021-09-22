using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public class AttackerCombatInfo
    {
        public void Initialize(GameObject attacker, Vector2 position, AttackInfo attackerInfo)
        {
            this.instance = attacker;
            this.isPlayer = GameplaySystem.playerManager.IsPartOfPlayer(attacker);
            this.position = position;
            this.attackInfo = attackerInfo;
            hasForceDirection = false;
            forceDirection = Vector2.zero;
        }

        public void Initialize(GameObject attacker, Vector2 position, AttackInfo attackerInfo,Vector2 forceDirection)
        {
            this.instance = attacker;
            this.isPlayer = GameplaySystem.playerManager.IsPartOfPlayer(attacker);
            this.position = position;
            this.attackInfo = attackerInfo;
            hasForceDirection = true;
            this.forceDirection = forceDirection;
        }

        public bool isPlayer { get; private set; }
        public GameObject instance { get; private set; }
        public Vector2 position { get; private set; }
        public AttackInfo attackInfo { get; private set; }
        public bool hasForceDirection { get; private set; }
        public Vector2 forceDirection { get; private set; }
    }
}