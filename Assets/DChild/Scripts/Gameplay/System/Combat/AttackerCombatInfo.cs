using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public class AttackerCombatInfo
    {
        public bool isPlayer { get; private set; }
        public GameObject instance { get; private set; }
        public Vector2 position { get; private set; }
        public AttackDamageInfo attackInfo { get; private set; }
        public bool hasForceDirection { get; private set; }
        public Vector2 forceDirection { get; private set; }

        public Collider2D hitCollider { get; private set; }
        public GameObject damageFX { get; private set; }

        public void Initialize(GameObject attacker, Vector2 position, AttackDamageInfo attackerInfo, Collider2D hitCollider, GameObject damageFX)
        {
            this.instance = attacker;
            this.isPlayer = GameplaySystem.playerManager.IsPartOfPlayer(attacker);
            this.position = position;
            this.attackInfo = attackerInfo;
            hasForceDirection = false;
            forceDirection = Vector2.zero;
            this.hitCollider = hitCollider;
            this.damageFX = damageFX;
        }

        public void Initialize(GameObject attacker, Vector2 position, AttackDamageInfo attackerInfo,Vector2 forceDirection, Collider2D hitCollider, GameObject damageFX)
        {
            this.instance = attacker;
            this.isPlayer = GameplaySystem.playerManager.IsPartOfPlayer(attacker);
            this.position = position;
            this.attackInfo = attackerInfo;
            hasForceDirection = true;
            this.forceDirection = forceDirection;
            this.hitCollider = hitCollider;
            this.damageFX = damageFX;
        }

    }
}