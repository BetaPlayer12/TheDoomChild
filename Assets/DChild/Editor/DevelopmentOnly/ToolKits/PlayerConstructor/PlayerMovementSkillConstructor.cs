using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.Skill;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace DChildEditor.Toolkit
{
    public class PlayerMovementSkillConstructor
    {
        private static bool[] m_hasSkill;

        public static void Reset()
        {
            m_hasSkill = new bool[(int)PrimarySkill._COUNT];
        }

        public static void UpdateConstructor(PlayerConstructor window)
        {
            var player = window.player;
            SetSkillStatus(PrimarySkill.DoubleJump, player.GetComponentInChildren<DoubleJump>());
            SetSkillStatus(PrimarySkill.WallJump, player.GetComponentInChildren<WallJump>());
            var hasDash = player.GetComponentInChildren<GroundDash>() != null &&
                          player.GetComponentInChildren<AirDash>() != null;
            SetSkillStatus(PrimarySkill.Dash, hasDash);
        }

        public static void DrawInspector(PlayerConstructor window)
        {
            if(m_hasSkill == null)
            {
                m_hasSkill = new bool[(int)PrimarySkill._COUNT];
            }

            var windowWidth = window.position.width;
            SirenixEditorGUI.BeginBox("Movement Skills", false, GUILayout.Width(windowWidth));
            if (HasSkill(PrimarySkill.DoubleJump) == false)
            {
                if (GUILayout.Button("Attach Double Jump"))
                {
                    AttachDoubleJump(window.player);
                }
            }

            if (HasSkill(PrimarySkill.WallJump) == false)
            {
                if (GUILayout.Button("Attach Wall Jump"))
                {
                    AttachWallJump(window.player);
                }
            }

            if (HasSkill(PrimarySkill.Dash) == false)
            {
                if (GUILayout.Button("Attach Dash"))
                {
                    AttachDash(window.player);
                }
            }
            SirenixEditorGUI.EndBox();
        }

        private static bool HasSkill(PrimarySkill skill) => m_hasSkill[(int)skill];
        private static void SetSkillStatus(PrimarySkill skill, Component component) => m_hasSkill[(int)skill] = component != null;
        private static void SetSkillStatus(PrimarySkill skill, bool value) => m_hasSkill[(int)skill] = value;

        private static GameObject GetOrCreateMovementSkillGO(GameObject player)
        {
            var movementSkillsTrans = Commands.FindChildOf(player.transform, "MovementSkills");
            if (movementSkillsTrans == null)
            {
                var behaviours = Commands.FindChildOf(player.transform, "Behaviours");
                return Commands.CreateGameObject("MovementSkills", behaviours.transform, Vector3.zero);
            }
            else
            {
                return movementSkillsTrans.gameObject;
            }
        }

        private static void AttachDoubleJump(GameObject player)
        {
            var movementSkillGO = GetOrCreateMovementSkillGO(player);
            var doubleJump = movementSkillGO.AddComponent<DoubleJump>();
            doubleJump.Initialize(70);
            SetSkillStatus(PrimarySkill.DoubleJump, true);
        }
        private static void AttachWallJump(GameObject player)
        {
            ValidateWallStick(player);
            var movementSkillGO = GetOrCreateMovementSkillGO(player);
            var wallJump = movementSkillGO.AddComponent<WallJump>();
            wallJump.Initialize(100);
            wallJump.Initialize(30, 0.1f);
            SetSkillStatus(PrimarySkill.WallJump, true);
        }
        private static void AttachDash(GameObject player)
        {
            var movementSkillGO = GetOrCreateMovementSkillGO(player);
            var groundDash = Commands.GetOrCreateComponent<GroundDash>(movementSkillGO);
            groundDash.Initialize(150f, 0.2f);
            var airDash = Commands.GetOrCreateComponent<AirDash>(movementSkillGO);
            airDash.Initialize(150f, 0.2f);
            airDash.Initialize(0.1f); //(70f);
            SetSkillStatus(PrimarySkill.Dash, true);
        }

        private static void ValidateWallStick(GameObject player)
        {
            if (Commands.FindChildOf(player.transform, "WallStickSensor") == null)
            {
                var sensors = Commands.FindChildOf(player.transform, "Sensors");
                var wallStickSensorGO = Commands.CreateGameObject("WallStickSensor", sensors, new Vector3(0.1030002f, 6.41f, 0));
                var wallStickSensor = wallStickSensorGO.AddComponent<RaySensor>();
                wallStickSensor.Initialize(0f);
                wallStickSensor.Initialize(2, LayerMask.GetMask("Environment"), true, 12.17f, 4.97f);
                var wallStickSensorRotator = wallStickSensorGO.AddComponent<RaySensorFaceRotator>();
                wallStickSensorRotator.SetRotations(180, 0);
                player.GetComponentInChildren<PlayerSensors>().InitializeWallStickSensor(wallStickSensor);
            }

            if (player.GetComponent<WallStick>() == null)
            {
                var movementSkillsGO = GetOrCreateMovementSkillGO(player);
                var wallStick = movementSkillsGO.AddComponent<WallStick>();
                wallStick.Initialize(2.3f, 0.3f, 8f);
            }
        }
    }
}