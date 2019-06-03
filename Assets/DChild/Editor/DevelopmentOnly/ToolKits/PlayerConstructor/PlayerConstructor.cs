using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Physics;
using DChild.Gameplay.Systems.WorldComponents;
using DChild.Inputs;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Spine.Unity;
using Spine.Unity.Modules;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace DChildEditor.Toolkit
{
    public class PlayerConstructor : OdinEditorWindow
    {
        [SerializeField]
        private GameObject m_player;

        private CollisionDetector m_legCollision;
        private Collider2D m_legCollider;
        private RaySensor m_headSensor;
        private RaySensor m_groundSensor;
        private RaySensor m_edgeSensor;
        private RaySensor m_slopeSensor;

        private Transform m_playerModel;
        private bool m_hasPlayerReferenced;
        private static PlayerConstructor m_instance;

        private bool m_hasSlopeAssist;
        private bool m_hasFloatyJump;

        public GameObject player => m_player;

        [MenuItem("Tools/Kit/Player Constructor")]
        private static void OpenWindow()
        {
            m_instance = GetWindow<PlayerConstructor>();
            m_instance.Show();
            m_instance.titleContent = new GUIContent("Player Constructor", EditorIcons.RulerRect.Active);
        }

        private void DrawMain()
        {
            var windowWidth = this.position.width;
            SirenixEditorGUI.BeginBox("Main", false, GUILayout.Width(windowWidth));
            EditorGUILayout.Space();
            EditorGUI.BeginChangeCheck();
            m_player = (GameObject)EditorGUILayout.ObjectField("Player", m_player, typeof(GameObject), true);
            if (EditorGUI.EndChangeCheck())
            {
                var player = m_player?.GetComponent<Player>();
                m_hasPlayerReferenced = player != null;
                if (m_hasPlayerReferenced)
                {
                    m_hasSlopeAssist = m_player.GetComponentInChildren<SlopeAssist>();
                    m_hasFloatyJump = m_player.GetComponentInChildren<FloatyPeak>();
                    PlayerCombatConstructor.UpdateConstructor(this);
                    PlayerMovementSkillConstructor.UpdateConstructor(this);
                }
            }
            else if (m_player == null)
            {
                m_hasPlayerReferenced = false;
                PlayerCombatConstructor.Reset();
                PlayerMovementSkillConstructor.Reset();
            }

            if (m_hasPlayerReferenced)
            {
                SirenixEditorGUI.BeginBox("Basic Misc", false, GUILayout.Width(windowWidth - 5));
                ConstructorCommands.DrawButton(m_hasSlopeAssist, "Attach Slope Assist", player, AttachSlopeAssist);
                ConstructorCommands.DrawButton(m_hasFloatyJump, "Attach Floaty Jump", player, AttachFloatyJump);
                SirenixEditorGUI.EndBox();
            }
            else
            {
                EditorGUILayout.Space();
                SirenixEditorGUI.BeginBox("Template Configuration", false, GUILayout.Width(windowWidth - 10));
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("Currently you do not have a reference Player, You may create one from here", MessageType.Info);
                if (GUILayout.Button("Create Basic Template"))
                {
                    CreatePlayerBasicTemplate();
                    m_hasPlayerReferenced = true;
                }
            }
            SirenixEditorGUI.EndBox();
        }

        private void CreatePlayerBasicTemplate()
        {
            m_player = new GameObject("Player");
            var playerPosition = Camera.main.transform.position;
            playerPosition.z = 0;
            m_player.transform.position = playerPosition;

            ConstructModel();
            ConstructBasicBehaviours();
            ConstructFX();
            ConstructSensors();
            ConstructStats();
            //ConstructStatusEffects();
            AttachComponents();
            Commands.SetLayer(m_player, "Player", true);
        }

        private void AttachComponents()
        {
            var rigidbody = m_player.AddComponent<Rigidbody2D>();
            rigidbody.sharedMaterial = DChildResources.LoadPhysicsMaterial2D("SlipperyPMat");
            m_player.AddComponent<IsolatedObject>();
            m_player.AddComponent<PlayerInput>();

            var player = m_player.AddComponent<Player>();
            m_player.AddComponent<PlayerAnimation>();

            var physics = m_player.AddComponent<ShiftingCharacterPhysics2D>();
            physics.Initialize(30f);
            physics.Initialize(m_legCollision);
            physics.Initialize(m_legCollider);
            physics.Initialize(0, 37f);

            player.Initialize(m_playerModel);

            var velocityRestrictor = m_player.AddComponent<VelocityRestrictor>();
            velocityRestrictor.Initialize(float.NegativeInfinity, -130f, float.PositiveInfinity, float.PositiveInfinity);
        }

        private void ConstructStats()
        {
            var statsGO = Commands.CreateGameObject("Stats", m_player.transform, Vector3.zero);
            statsGO.AddComponent<Magic>();
            statsGO.AddComponent<BasicHealth>();
            statsGO.AddComponent<BasicAttackResistance>();
            statsGO.AddComponent<ExtendedAttackResistance>();
            statsGO.AddComponent<StatusResistance>();
        }

        private void ConstructFX()
        {
            var FxGO = Commands.CreateGameObject("FX", m_player.transform, Vector3.zero);

            var jumpFx = FxGO.AddComponent<JumpFX>();
            var jumpFxPrefab = DChildResources.LoadPrefab($"SpineFx", "JumpFX");
            jumpFx.Initialize(jumpFxPrefab);

            var landFx = FxGO.AddComponent<LandFX>();
            var landFxPrefab = DChildResources.LoadPrefab($"SpineFx", "LandFX");
            landFx.Initialize(landFxPrefab);
        }

        private void ConstructStatusEffects()
        {
            var StatEffectsGO = Commands.CreateGameObject("StatusEffects", m_player.transform, Vector3.zero);
            StatEffectsGO.AddComponent<ImmobilityBreak>();
        }

        private void ConstructBasicBehaviours()
        {
            var behavioursGO = Commands.CreateGameObject("Behaviours", m_player.transform, Vector3.zero);
            var placementTracker = behavioursGO.AddComponent<PlacementTracker>();
            placementTracker.Initialize(-20f);

            behavioursGO.AddComponent<PlayerController>();
            behavioursGO.AddComponent<GroundController>();
            behavioursGO.AddComponent<AirController>();
            behavioursGO.AddComponent<AnimationController>();
            //behavioursGO.AddComponent<StatusEffectController>();
            behavioursGO.AddComponent<EventModuleConnector>();

            var basicMovementGO = Commands.CreateGameObject("BasicMovement", behavioursGO.transform, Vector3.zero);
            var groundMovement = basicMovementGO.AddComponent<GroundMovement>();
            groundMovement.Initialize(30f, 240f, 44.5f);

            basicMovementGO.AddComponent<Crouch>();
            var crouch = basicMovementGO.AddComponent<CrouchMovement>();
            crouch.Initialize(18.7f, 240f, 90.7f);

            var jump = basicMovementGO.AddComponent<GroundJumpHandler>();
            jump.Initialize(67f);

            var HighJump = basicMovementGO.AddComponent<HighJump>();
            HighJump.Initialize(4f, 25f);

            var airMovement = basicMovementGO.AddComponent<AirMovement>();
            airMovement.Initialize(30f, 245f, 145f);

            var landHandler = basicMovementGO.AddComponent<PlayerLandHandler>();
            landHandler.Initialize(-120, 2);

            var fallHandler = basicMovementGO.AddComponent<PlayerFallHandler>();

            var platformDrop = basicMovementGO.AddComponent<PlatformDrop>();
            platformDrop.Initialize(0.5f);

            var fallAssist = basicMovementGO.AddComponent<FallAssist>();
            fallAssist.Initialize(0.2f, 10);
        }

        private void ConstructModel()
        {
            var model = Commands.CreateGameObject("Model", m_player.transform, new Vector3(0.04f, 0f, 0f));
            var skeletonAnimation = model.AddComponent<SkeletonAnimation>();
            skeletonAnimation.skeletonDataAsset = DChildResources.LoadSpine("Characters/PlayerSkeleton", "Player_SkeletonData"); ;
            skeletonAnimation.Initialize(false);
            skeletonAnimation.AnimationName = "Idle";
            skeletonAnimation.loop = true;

            var skeletonUtility = model.AddComponent<SkeletonUtility>();
            skeletonUtility.SpawnHierarchy(SkeletonUtilityBone.Mode.Follow, true, true, true);

            var skeletonGhost = model.AddComponent<SkeletonGhost>();
            //skeletonGhost.Initialize(0.03333334f, 10, 15f);
            skeletonGhost.Initialize(DChildResources.LoadShader("Spine-Special-Skeleton-Ghost"));

            var animator = model.AddComponent<Animator>();
            animator.runtimeAnimatorController = Resources.Load("Assets/DChildNew/Objects/Animations/Player/Player.controller") as RuntimeAnimatorController;
            var instantiate = (GameObject)PrefabUtility.InstantiatePrefab(DChildResources.LoadPrefab("Characters/Player", "Cape"));
            instantiate.transform.parent = Commands.FindChildOf(model.transform, "hood");

            var centerOfMass = Commands.CreateGameObject("CenterOfMass", model.transform, new Vector3(0.56f, 7.03f, 0f));
            m_playerModel = centerOfMass.transform;

            var colliders = Commands.CreateGameObject("Colliders", model.transform, new Vector3(0f, 7.03f, 0f));
            colliders.AddComponent<CharacterColliders>();

            var legColliderGO = Commands.CreateGameObject("LegCollider", colliders.transform, new Vector3(0f, -7.04f, 0));
            var legCollider = legColliderGO.AddComponent<UnityEngine.CapsuleCollider2D>();
            legCollider.offset = new Vector2(0.05919647f, 2.771857f);
            legCollider.size = new Vector2(4.297119f, 5.666287f);
            legCollider.usedByEffector = true;
            m_legCollider = legCollider;
            m_legCollision = legColliderGO.AddComponent<CollisionDetector>();
            m_legCollision.SetCollisionLayerMask("Environment");
            var legPlatformEffector = legColliderGO.AddComponent<PlatformEffector2D>();
            legPlatformEffector.useColliderMask = false;
            legPlatformEffector.rotationalOffset = 180;

            var bodyColliderGO = Commands.CreateGameObject("BodyCollider", colliders.transform, Vector3.zero);
            var bodyCollider = bodyColliderGO.AddComponent<UnityEngine.CapsuleCollider2D>();
            bodyCollider.offset = new Vector2(0.0630002f, -0.5030251f);
            bodyCollider.size = new Vector2(4.329028f, 12.52695f);
            var bodyCollision = bodyColliderGO.AddComponent<CollisionDetector>();
            bodyCollision.SetCollisionLayerMask("Environment");

            var sortingGroup = m_player.AddComponent<SortingGroup>();
            sortingGroup.sortingLayerName = "Default";
            sortingGroup.sortingOrder = 0;
        }

        private void ConstructSensors()
        {
            var sensorGO = Commands.CreateGameObject("Sensors", m_player.transform, Vector3.zero);

            var headSensorGO = Commands.CreateGameObject("HeadSensor", sensorGO.transform, new Vector3(0.18f, 7.8f, 0));
            m_headSensor = headSensorGO.AddComponent<RaySensor>();
            m_headSensor.Initialize(2, LayerMask.GetMask("Environment"), true, 0.8f, 5f);
            m_headSensor.Initialize(90f);
            var headSensorRotator = headSensorGO.AddComponent<RaySensorFaceRotator>();
            headSensorRotator.SetRotations(270, 90);

            var groundSensorGO = Commands.CreateGameObject("GroundSensor", sensorGO.transform, Vector3.zero);
            m_groundSensor = groundSensorGO.AddComponent<RaySensor>();
            m_groundSensor.Initialize(5, LayerMask.GetMask("Environment"), true, 3.3f, 0.75f);
            m_groundSensor.Initialize(-90f);
            groundSensorGO.AddComponent<DropPlatformDetector>();
            var groundSensorRotator = groundSensorGO.AddComponent<RaySensorFaceRotator>();
            groundSensorRotator.SetRotations(90, 270);

            var edgeSensorGO = Commands.CreateGameObject("EdgeSensor", sensorGO.transform, new Vector3(1.98f, 0, 0));
            m_edgeSensor = edgeSensorGO.AddComponent<RaySensor>();
            m_edgeSensor.Initialize(1, LayerMask.GetMask("Environment"), true, 2.09f, 3.87f);
            m_edgeSensor.Initialize(-90f);
            edgeSensorGO.AddComponent<EdgeDetector>();
            var edgeSensorRotator = edgeSensorGO.AddComponent<RaySensorFaceRotator>();
            edgeSensorRotator.SetRotations(90, 270);

            var slopeSensorGO = Commands.CreateGameObject("SlopeSensor", sensorGO.transform, new Vector3(0, 0.17f, 0));
            m_slopeSensor = slopeSensorGO.AddComponent<RaySensor>();
            m_slopeSensor.Initialize(1, LayerMask.GetMask("Environment"), true, 0.1f, 3f);
            m_slopeSensor.Initialize(0f);
            slopeSensorGO.AddComponent<CalculateSlope>();
            var slopeSensorRotator = slopeSensorGO.AddComponent<RaySensorFaceRotator>();
            slopeSensorRotator.SetRotations(180f, 0f);

            var caster = sensorGO.AddComponent<RaySensorCaster>();
            caster.Initialize(m_groundSensor, m_edgeSensor, m_slopeSensor);
            caster.Initialize(true, true);
            caster.InitializeDistance(1f);
            caster.InitializeTime(0.5f);

            var sensors = sensorGO.AddComponent<PlayerSensors>();
            sensors.Initialize(m_groundSensor, m_headSensor, m_edgeSensor, m_slopeSensor);
        }

        private void AttachSlopeAssist(GameObject player)
        {
           var basicMovementGO =  Commands.FindChildOf(player.transform, "BasicMovement").gameObject;
           var slopeAssists = basicMovementGO.AddComponent<SlopeAssist>();
           slopeAssists.Initialize(m_slopeSensor);
            m_hasSlopeAssist = true;
        }

        private void AttachFloatyJump(GameObject player)
        {
            var basicMovementGO = Commands.FindChildOf(player.transform, "BasicMovement").gameObject;
            var slopeAssists = basicMovementGO.AddComponent<FloatyPeak>();
            slopeAssists.Initialize(10, 10);
            m_hasFloatyJump = true;
        }

        protected override void OnGUI()
        {
            DrawMain();
            EditorGUILayout.Space();

            if (m_player != null)
            {
                PlayerCombatConstructor.DrawInspector(this);
                PlayerMovementSkillConstructor.DrawInspector(this);
            }
        }

        protected override object GetTarget()
        {
            return m_instance;
        }
    }
}