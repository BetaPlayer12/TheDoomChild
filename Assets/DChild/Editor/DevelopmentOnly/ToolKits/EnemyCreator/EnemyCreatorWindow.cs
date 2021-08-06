using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Combat.StatusAilment;
using DChild.Gameplay.Systems.WorldComponents;
using DChildEditor.Toolkit.EnemyCreation;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Toolkit
{
    public class EnemyCreatorWindow : OdinEditorWindow
    {
        [MenuItem("Tools/Kit/Create/Enemy Creator")]
        private static void OpenWindow()
        {
            GetWindow<EnemyCreatorWindow>().Show();
        }

        [HorizontalGroup("MainSplit", Width = 0.3f)]

        #region Main Info
        [BoxGroup("MainSplit/MainInfo")]
        [SerializeField, PreviewField(100), HorizontalGroup("MainSplit/MainInfo/Split"), LabelWidth(100), AssetsOnly]
        private GameObject m_model;
        [SerializeField, HideLabel, VerticalGroup("MainSplit/MainInfo/Split/Info")]
        private EnemyNameField m_name;
        [SerializeField, VerticalGroup("MainSplit/MainInfo/Split/Info")]
        private bool m_isBoss;


        [SerializeField, BoxGroup("MainSplit/MainInfo/Details")]
        private bool m_hasAttackResistance;
        [SerializeField, BoxGroup("MainSplit/MainInfo/Details")]
        private bool m_canInflictStatusEffect;
        [SerializeField, BoxGroup("MainSplit/MainInfo/Details")]
        private bool m_canReceiveStatusEffects;
        [SerializeField, BoxGroup("MainSplit/MainInfo/Details"), ShowIf("m_canReceiveStatusEffects")]
        private bool m_hasStatusResistance;
        #endregion

        [BoxGroup("MainSplit/Editable")]
        #region Basic
        [TabGroup("MainSplit/Editable/Tabs", "Basic")]

        [SerializeField, BoxGroup("MainSplit/Editable/Tabs/Basic/BasicInfo"), HideLabel]
        private DamageableField m_damageableField;
        [SerializeField, BoxGroup("MainSplit/Editable/Tabs/Basic/BasicInfo/Attack Info"), HideLabel]
        private AttackerField m_attacker;

        [ShowIfGroup("MainSplit/Editable/Tabs/Basic/Addon", MemberName = "@m_hasAttackResistance || m_canInflictStatusEffect || (m_canReceiveStatusEffects && m_hasStatusResistance)")]
        [TitleGroup("Add Ons", GroupID = "MainSplit/Editable/Tabs/Basic/Addon/Title")]

        [ShowIfGroup("MainSplit/Editable/Tabs/Basic/Addon/Title/AttackResistanceField", MemberName = "@m_hasAttackResistance")]
        [SerializeField, FoldoutGroup("MainSplit/Editable/Tabs/Basic/Addon/Title/AttackResistanceField/Group", true, GroupName = "Attack Resistance"), HideLabel, PropertySpace(SpaceAfter = 10)]
        private AttackResistanceField m_attackResistanceField;

        [ShowIfGroup("MainSplit/Editable/Tabs/Basic/Addon/Title/StatusInfliction", MemberName = "@m_canInflictStatusEffect")]
        [SerializeField, FoldoutGroup("MainSplit/Editable/Tabs/Basic/Addon/Title/StatusInfliction/Group", true, GroupName = "Status Infliction"), HideLabel, PropertySpace(SpaceAfter = 10)]
        private StatusInflictionField m_statusInflictionField;

        [ShowIfGroup("MainSplit/Editable/Tabs/Basic/Addon/Title/StatusResistanceField", MemberName = "@ m_canReceiveStatusEffects && m_hasStatusResistance")]
        [SerializeField, FoldoutGroup("MainSplit/Editable/Tabs/Basic/Addon/Title/StatusResistanceField/Group", true, GroupName = "Status Resistance"), HideLabel, PropertySpace(SpaceAfter = 10)]
        private StatusResistanceField m_statusResistanceField;
        #endregion

        [OdinSerialize, TabGroup("MainSplit/Editable/Tabs", "Behaviour"), HideLabel, HideReferenceObjectPicker]
        private BehaviourField m_behaviourField = new BehaviourField();

        [SerializeField, FolderPath, BoxGroup("MainSplit/MainInfo")]
        private string m_saveDataReference;
        [Button, BoxGroup("MainSplit/MainInfo")]
        private void InstantiateEnemy()
        {
            var name = m_name.value;
            var filePath = $"{m_saveDataReference}/{name}";

            GameObject instance = new GameObject(m_name.value);
            instance.transform.position = Vector3.zero;
            var modelInstance = (GameObject)PrefabUtility.InstantiatePrefab(m_model, instance.transform);

            var character = instance.AddComponent<Character>();
            instance.AddComponent<Rigidbody2D>();
           var isolatedObject = instance.AddComponent<IsolatedObject>();

            GameObject stats = new GameObject("Stats");
            stats.transform.SetParent(instance.transform);
            m_damageableField.Apply(instance, stats);
            m_attacker.Apply(instance, filePath);
            if (m_isBoss)
            {
                instance.AddComponent<Boss>().InitializeFields(m_behaviourField.aiData.bestiaryData, instance.GetComponentInChildren<Health>());
            }

            if (m_canInflictStatusEffect)
            {
                m_statusInflictionField.Apply(instance, filePath);

            }

            if (m_hasAttackResistance)
            {
                m_attackResistanceField.Apply(instance, stats, filePath);
            }

            if (m_canReceiveStatusEffects)
            {
                instance.AddComponent<StatusEffectReciever>().InitializeField(instance.GetComponent<Character>());
                if (m_hasStatusResistance)
                {
                    m_statusResistanceField.Apply(instance, stats, filePath);
                }
            }

            GameObject behaviour = new GameObject("Behaviour");
            behaviour.transform.SetParent(instance.transform);
            m_behaviourField.Apply(instance, behaviour);
            character.InitializeField(instance.transform, isolatedObject, instance.GetComponent<IsolatedCharacterPhysics2D>(), modelInstance.GetComponent<CharacterColliders>());
        }
    }
}