using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Spine.Unity;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Toolkit.EnemyCreation
{
    [System.Serializable]
    public class BehaviourField
    {
        private enum MoveType
        {
            None,
            Crawl,
            Fly,
            Walk,
            SpineRoot
        }

        [OdinSerialize, ValueDropdown("GetAIs"), OnValueChanged("OnAIChange")]
        private Type m_AI;
        [SerializeField, ValueDropdown("GetData"), ShowIf("@m_AI != null")]
        private AIData m_data;
        [SerializeField]
        private MoveType m_moveType;
        [OdinSerialize, ValueDropdown("GetPatrolHandles")]
        private Type m_patrolType;
        [SerializeField, ValueDropdown("GetTurnHandles")]
        private Type m_turnHandleType;
        [SerializeField]
        private bool m_listenToSpineEvents;
        [SerializeField]
        private bool m_canFlinch;

        public AIData aiData => m_data;

        public void Apply(GameObject instance, GameObject behaviour)
        {
            ApplyMovement(instance, behaviour);

            if (m_AI != null)
            {
                var brain = ((ICombatAIBrain)instance.AddComponent(m_AI));
                brain.SetData(m_data);
                brain.InitializeField(instance.GetComponent<Character>(),         
                                      instance.GetComponentInChildren<SpineRootAnimation>(),
                                      instance.GetComponent<Damageable>(),
                                      instance.transform);
            }

            if (m_patrolType != null)
            {
                behaviour.AddComponent(m_patrolType);
            }

            if(m_turnHandleType != null)
            {
                ((TurnHandle)behaviour.AddComponent(m_turnHandleType)).InitializeField(instance.GetComponent<Character>());
            }

            if (m_canFlinch)
            {
                behaviour.AddComponent<FlinchHandler>().InitializeField(instance.GetComponentInChildren<SpineRootAnimation>(),
                                                                        instance.GetComponent<IsolatedPhysics2D>(),
                                                                        instance.GetComponentInChildren<SkeletonAnimation>());
            }

            behaviour.AddComponent<DeathHandle>().InitializeField(instance.GetComponent<Damageable>(),
                                                                  instance.GetComponentInChildren<SpineRootAnimation>());

            if (m_listenToSpineEvents)
            {
                behaviour.AddComponent<SpineEventListener>().InitializeFields(instance.GetComponentInChildren<SkeletonAnimation>());
            }
        }

        private void ApplyMovement(GameObject instance, GameObject behaviour)
        {
            IsolatedPhysics2D physics = null;
            switch (m_moveType)
            {
                case MoveType.None:
                    instance.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                    break;
                case MoveType.Walk:
                    instance.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    physics = instance.AddComponent<IsolatedCharacterPhysics2D>();
                    physics.simulateGravity = true;
                    behaviour.AddComponent<GroundMovementHandler2D>().InitializeField((IsolatedCharacterPhysics2D)physics);
                    break;
                case MoveType.Crawl:
                    instance.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    instance.AddComponent<IsolatedObjectPhysics2D>().simulateGravity = false;
                    break;
                case MoveType.Fly:
                    instance.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    physics = instance.AddComponent<IsolatedObjectPhysics2D>();
                    physics.simulateGravity = false;
                    behaviour.AddComponent<OmniMovementHandle2D>().InitializeField(physics);
                    break;
                case MoveType.SpineRoot:
                    instance.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    break;
            }
        }

        private void OnAIChange()
        {
            m_data = null;
        }

        private IEnumerable GetPatrolHandles() => GetDerivedClassTypes<PatrolHandle>();

        private IEnumerable GetTurnHandles() => GetDerivedClassTypes<TurnHandle>();

        private IEnumerable GetAIs()
        {
            ValueDropdownList<Type> list = new ValueDropdownList<Type>();
            list.Add("None", null);
            foreach (Type type in
           Assembly.GetAssembly(typeof(ICombatAIBrain)).GetTypes()
           .Where(myType => myType.IsClass && !myType.IsAbstract && (typeof(ICombatAIBrain)).IsAssignableFrom(myType)))
            {
                list.Add(type.Name, type);
            }
            return list;
        }

        private IEnumerable GetData()
        {
            var list = new ValueDropdownList<AIData>();
            list.Add("None", null);
            var infoType = m_AI.BaseType.GetGenericArguments()[0];
            var filePaths = AssetDatabase.FindAssets("t:AIData");
            for (int i = 0; i < filePaths.Length; i++)
            {
                var asset = AssetDatabase.LoadAssetAtPath<AIData>(AssetDatabase.GUIDToAssetPath(filePaths[i]));
                if (asset != null && asset.info != null && asset.info.GetType() == infoType)
                {
                    list.Add(asset);
                }
            }
            return list;
        }

        private IEnumerable GetDerivedClassTypes<T>()
        {
            ValueDropdownList<Type> list = new ValueDropdownList<Type>();
            list.Add("None", null);
            foreach (Type type in
           Assembly.GetAssembly(typeof(T)).GetTypes()
           .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
            {
                list.Add(type.Name, type);
            }
            return list;
        }
    }
}