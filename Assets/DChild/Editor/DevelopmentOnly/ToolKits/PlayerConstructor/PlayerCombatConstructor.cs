using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Combat;
using Sirenix.Utilities.Editor;
using Spine.Unity;
using UnityEngine;

namespace DChildEditor.Toolkit
{
    public class PlayerCombatConstructor
    {
        private static bool m_hasHitbox;
        private static bool m_hasBasicAttack;
        private static bool m_hasEquipment;
        private static bool m_hasAttributes;
        private static bool m_hasEdgeAttackMisc;

        public static void Reset()
        {
            m_hasHitbox = false;
            m_hasBasicAttack = false;
            m_hasEquipment = false;
            m_hasAttributes = false;
            m_hasEdgeAttackMisc = false;
        }

        public static void UpdateConstructor(PlayerConstructor window)
        {
            var player = window.player;
            m_hasHitbox = player.GetComponentInChildren<Hitbox>();
            m_hasBasicAttack = player.GetComponentInChildren<CombatController>();

        }

        public static void DrawInspector(PlayerConstructor window)
        {
            var windowWidth = window.position.width;
            var player = window.player;
            SirenixEditorGUI.BeginBox("Combat", false, GUILayout.Width(windowWidth));
            ConstructorCommands.DrawButton(m_hasHitbox, "Attach Hitbox", player, AttachHitbox);
            ConstructorCommands.DrawButton(m_hasBasicAttack, "Attach Basic Attack", player, AttachBasicAttack);
            if (m_hasBasicAttack)
            {
                SirenixEditorGUI.BeginBox("Combat Misc", false, GUILayout.Width(windowWidth - 5));
                ConstructorCommands.DrawButton(m_hasEdgeAttackMisc, "Attach Edge Attack", player, AttachEdgeAttack);
                SirenixEditorGUI.EndBox();
            }
            SirenixEditorGUI.EndBox();
        }

        private static void AttachHitbox(GameObject player)
        {
            var model = player.transform.Find("Model");
            var hitBox = Commands.CreateGameObject("Hitbox", model, Vector3.zero);
            hitBox.tag = "Hitbox";
            var hitBoxTrigger = hitBox.AddComponent<BoxCollider2D>();
            hitBoxTrigger.isTrigger = true;
            hitBoxTrigger.offset = new Vector2(0.07641488f, 6.301981f);
            hitBoxTrigger.size = new Vector2(3.903785f, 12.54331f);
            hitBox.AddComponent<Hitbox>();

            var basicCombatGO = GetOrCreateCombatGO(player);
            basicCombatGO.AddComponent<PlayerFlinch>();

            Commands.SetLayer(hitBox, "Player");
            m_hasHitbox = true;
        }

        private static void AttachBasicAttack(GameObject player)
        {
            player.AddComponent<PlayerCombatAnimation>();
            var attackHitbox = Commands.FindChildOf(player.transform, "ATK_HITBOX");
            var boundingBox = attackHitbox.gameObject.AddComponent<BoundingBoxFollower>();
            boundingBox.slotName = "ATK HITBOX_Sword";
            boundingBox.isTrigger = true;
            boundingBox.Initialize(true);
            attackHitbox.gameObject.AddComponent<ColliderDamage>();

            var behaviour = player.transform.Find("Behaviours");
            var combatController = behaviour.gameObject.AddComponent<CombatController>();
            combatController.Initialize(2f);

            var combatGO = GetOrCreateCombatGO(player);
            var basicAttack = combatGO.AddComponent<BasicAttack>();
            basicAttack.Initialize(2, 0.3f, 0);
            m_hasBasicAttack = true;
        }

        private static void AttachEdgeAttack(GameObject player)
        {
            var combat = GetOrCreateCombatGO(player);
           var attackOnEdge =  combat.AddComponent<AttackOnEdge>();
            attackOnEdge.Initialize(4);
        }

        private static GameObject GetOrCreateCombatGO(GameObject player)
        {
            var combatGO = Commands.FindChildOf(player.transform, "Combat");
            if (combatGO == null)
            {
                var behaviour = Commands.FindChildOf(player.transform, "Behaviours");
                return Commands.CreateGameObject("Combat", behaviour, Vector3.zero);
            }
            return combatGO.gameObject;
        }
    }

}