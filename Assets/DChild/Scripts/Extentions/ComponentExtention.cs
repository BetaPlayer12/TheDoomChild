using System;
using System.Reflection;
using UnityEngine;

namespace DChild
{

    public static class ComponentExtention
    {
        public static T CopyComponentAsNew<T>(this GameObject gameObject, T component) where T : Component
        {
            var newComponent = gameObject.AddComponent<T>();
            newComponent.CopyComponentValueOf(component);
            return newComponent;
        }

        public static T CopyComponentValueOf<T>(this Component comp, T other) where T : Component
        {
            Type type = comp.GetType();
            if (type != other.GetType()) return null; // type mis-match
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
            PropertyInfo[] pinfos = type.GetProperties(flags);
            foreach (var pinfo in pinfos)
            {
                if (pinfo.CanWrite)
                {
                    try
                    {
                        pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                    }
                    catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
                }
            }
            FieldInfo[] finfos = type.GetFields(flags);
            foreach (var finfo in finfos)
            {
                finfo.SetValue(comp, finfo.GetValue(other));
            }
            return comp as T;
        }

        public static void ValidateChildComponent<T>(this MonoBehaviour mono, string parentName = "", string name = "") where T : Component
        {
            if (mono.GetComponentInChildren<T>() == null)
            {
                if (parentName == "")
                {
                    name = name == "" ? typeof(T).Name : name;
                    var instance = new GameObject(name);
                    instance.transform.parent = mono.transform;
                    instance.transform.localPosition = Vector3.zero;
                    instance.AddComponent<T>();
                }
                else
                {
                    var parentObject = mono.transform.Find(parentName).gameObject;
                    parentObject.AddComponent<T>();
                }
            }
        }

        public static void ValidateComponent<T>(this MonoBehaviour mono) where T : Component
        {
            if (mono.GetComponentInChildren<T>() == null)
            {
                mono.gameObject.AddComponent<T>();
            }
        }

        public static T GetOrCreateComponent<T>(this MonoBehaviour mono) where T : Component
        {
            var existingComponent = mono.GetComponent<T>();
            return existingComponent != null ? existingComponent : mono.gameObject.AddComponent<T>();
        }

        public static bool TryGetComponentInParent<T>(this Component mono, out T component)
        {
            component = mono.GetComponentInParent<T>();
            return component != null;
        }

        public static bool TryGetComponentInChildren<T>(this Component mono, out T component, bool includeInactive = false)
        {
            component = mono.GetComponentInChildren<T>();
            return component != null;
        }

        public static bool TryGetComponentInParent<T>(this GameObject mono, out T component)
        {
            component = mono.GetComponentInParent<T>();
            return component != null;
        }

        public static bool TryGetComponentInChildren<T>(this GameObject mono, out T component, bool includeInactive = false)
        {
            component = mono.GetComponentInChildren<T>(includeInactive);
            return component != null;
        }
    }
}
