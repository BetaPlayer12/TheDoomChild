using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Holysoft
{
    public static class ComponentUtility
    {
        public enum ComponentSearchMethod
        {
            SelfOnly,
            Parent,
            Child
        }

        public static void AssignNullComponent<T>(Component gameObject, ref T component, ComponentSearchMethod searchMethod = ComponentSearchMethod.SelfOnly) where T : Component
        {
            if (component == null)
            {
                switch (searchMethod)
                {
                    case ComponentSearchMethod.SelfOnly:
                        component = gameObject.GetComponent<T>();
                        break;
                    case ComponentSearchMethod.Parent:
                        component = gameObject.GetComponentInParent<T>();
                        break;
                    case ComponentSearchMethod.Child:
                        component = gameObject.GetComponentInChildren<T>();
                        break;
                }
            }
        }

        public static void AssignNullComponents<T>(Component gameObject, T[] component, ComponentSearchMethod searchMethod = ComponentSearchMethod.SelfOnly) where T : Component
        {
            if (component == null)
            {
                switch (searchMethod)
                {
                    case ComponentSearchMethod.SelfOnly:
                        component = gameObject.GetComponents<T>();
                        break;
                    case ComponentSearchMethod.Parent:
                        component = gameObject.GetComponentsInParent<T>();
                        break;
                    case ComponentSearchMethod.Child:
                        component = gameObject.GetComponentsInChildren<T>();
                        break;
                }
            }
        }
    }
}