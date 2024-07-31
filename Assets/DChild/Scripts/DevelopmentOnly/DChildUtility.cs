using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using DChild.Gameplay.Characters.Players;
using static DChild.DChildDatabase.SoulSkillConnection;

namespace DChild
{
    public static class DChildUtility
    {
        public static LayerMask GetEnvironmentMask() => LayerMask.GetMask("Environment", "PassableEnvironment");
        public static bool IsAnEnvironmentLayerObject(GameObject gameObject) => gameObject.layer == LayerMask.NameToLayer("Environment") || gameObject.layer == LayerMask.NameToLayer("PassableEnvironment");
        public static string GetSensorTag() => "Sensor";
        public static void ValidateSensor(GameObject gameObject)
        {
            var sensorTag = DChildUtility.GetSensorTag();
            if (gameObject.CompareTag(sensorTag) == false)
            {
                gameObject.tag = sensorTag;
            }
            if (gameObject.TryGetComponent(out Rigidbody2D rigidbody2D))
            {
                var colliders = rigidbody2D.GetComponentsInChildren<Collider2D>(true);
                foreach (var collider in colliders)
                {
                    if (collider.isTrigger && collider.gameObject.CompareTag(sensorTag) == false)
                    {
                        collider.gameObject.tag = sensorTag;
                    }
                }
            }
            else if (gameObject.TryGetComponent<Collider2D>(out Collider2D collider))
            {
                collider.isTrigger = true;
            }
        }

        public static bool IsADroppable(Component component) => component.CompareTag("Droppable");

        public static bool HasInterface<T>(object instance) => (typeof(T)).IsAssignableFrom(instance.GetType());
        public static bool IsSubclassOf<T>(object instance) => instance.GetType().IsSubclassOf(typeof(T));

        public static ValueDropdownList<int> GetSoulSkills()
        {
            var connection = DChildDatabase.GetSoulSkillConnection();
            connection.Initialize();
            var skills = connection.GetAllSkills();
            var list = ConvertToDropdownList(skills);
            connection.Close();
            return list;
        }

        private static ValueDropdownList<int> ConvertToDropdownList(Element[] skills)
        {
            var list = new ValueDropdownList<int>();
            for (int i = 0; i < skills.Length; i++)
            {
                list.Add(new ValueDropdownItem<int>(skills[i].name, skills[i].id));
            }
            return list;
        }

        #region Raycast
        private static ContactFilter2D m_contactFilter;
        private static RaycastHit2D[] m_hitResults;
        private static bool m_isInitialized;


        public static void Initialize()
        {
            if (m_isInitialized == false)
            {
                m_contactFilter.useLayerMask = true;
                m_contactFilter.SetLayerMask(DChildUtility.GetEnvironmentMask());
                //m_contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(DChildUtility.GetEnvironmentMask()));
                m_hitResults = new RaycastHit2D[16];
                m_isInitialized = true;
            }
        }

        public static RaycastHit2D[] RayCastEnvironment(Vector2 origin, Vector2 direction, float distance, bool ignoreTriggers, out int hitCount, bool debugMode = false)
        {
            Initialize();
            m_contactFilter.useTriggers = !ignoreTriggers;
            hitCount = Physics2D.Raycast(origin, direction, m_contactFilter, m_hitResults, distance);
#if UNITY_EDITOR
            if (debugMode)
            {
                if (hitCount > 0)
                {
                    Debug.DrawRay(origin, direction * m_hitResults[0].distance, Color.cyan, 1f);
                }
                else
                {
                    Debug.DrawRay(origin, direction * distance, Color.cyan, 1f);
                }
            }
#endif
            return m_hitResults;
        } 
        #endregion

#if UNITY_EDITOR
        private static IEnumerable<Type> GetDerivedClasses(Type baseClass, bool includeAbstract = false, bool includeGenerics = false)
        {
            var assembly = Assembly.GetAssembly(baseClass);
            var types = assembly.GetTypes();
            var derivedClasses = (from System.Type type in types where type.IsSubclassOf(baseClass) select type);
            if (includeAbstract && includeAbstract)
            {
                return derivedClasses;
            }
            else
            {
                return derivedClasses.Where(x => x.IsClass && x.IsAbstract == includeAbstract && x.IsGenericType == includeGenerics);
            }
        }

        private static IEnumerable<Type> GetDerivedInterfaces(Type baseClass, bool includeAbstract = false, bool includeGenerics = false)
        {
            var assembly = Assembly.GetAssembly(baseClass);
            var types = assembly.GetTypes();
            var derivedClasses = (from System.Type type in types where baseClass.IsAssignableFrom(type) select type);
            if (includeAbstract && includeAbstract)
            {
                return derivedClasses;
            }
            else
            {
                return derivedClasses.Where(x => x.IsClass && x.IsAbstract == includeAbstract && x.IsGenericType == includeGenerics);
            }
        }

        public static Type[] GetDerivedClasses<T>(bool includeAbstract = false, bool includeGenerics = false) where T : class
        {
            return GetDerivedClasses(typeof(T), includeAbstract, includeGenerics).ToArray();
        }

        public static string[] GetDerivedClassNames<T>(bool includeAbstract = false, bool includeGenerics = false) where T : class
        {
            return GetDerivedClasses(typeof(T), includeAbstract, includeGenerics).Select(x => x.Name).ToArray();
        }

        public static Type[] GetDerivedInterfaces<T>(bool includeAbstract = false, bool includeGenerics = false)
        {
            return GetDerivedInterfaces(typeof(T), includeAbstract, includeGenerics).ToArray();
        }

        public static string[] GetDerivedInterfacesNames<T>(bool includeAbstract = false, bool includeGenerics = false)
        {
            return GetDerivedInterfaces(typeof(T), includeAbstract, includeGenerics).Select(x => x.Name).ToArray();
        }
#endif
    }
}