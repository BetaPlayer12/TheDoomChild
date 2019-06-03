using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug
{
    [SelectionBase]
    [DisallowMultipleComponent]
    public class MeasureObject : MonoBehaviour
    {
        [SerializeField]
        [PropertyOrder(0)]
        [ValidateInput("ValidateReference")]
        private GameObject m_reference;

        [Button("Recreate")]
        [PropertyOrder(0)]
        private void Recreate()
        {
            Recreate(m_reference);
        }

        private void Recreate(GameObject reference)
        {
            CleanChildren();
            InstantiateOrigin(reference);
            ReadjustMeasureCount();
            ReadjustMeasureVisual();
        }

        [BoxGroup("Dimensions")]
        [SerializeField]
        [MinValue(0)]
        [PropertyOrder(1)]
        private float m_width = 1;
        [BoxGroup("Dimensions")]
        [SerializeField]
        [MinValue(0)]
        [PropertyOrder(1)]
        private float m_height = 1;

        [SerializeField]
        [HideInInspector]
        private GameObject m_origin;
        [SerializeField]
        [HideInInspector]
        private List<Transform> m_widthObjects;
        [SerializeField]
        [HideInInspector]
        private List<Transform> m_heightObjects;

        [SerializeField]
        [HideInInspector]
        private Transform m_heightGroup;
        [SerializeField]
        [HideInInspector]
        private Transform m_widthGroup;
        [SerializeField]
        [HideInInspector]
        private bool m_instantiated;

        private bool ValidateReference(GameObject reference)
        {
            if (m_reference != reference)
            {
                Recreate(reference);
            }
            return true;
        }

        private void ReadjustMeasureCount()
        {
            var widthInstances = Mathf.CeilToInt(m_width);
            AlignMeasureObjectCount(m_widthObjects, m_widthGroup, widthInstances);
            var heightInstances = Mathf.CeilToInt(m_height);
            AlignMeasureObjectCount(m_heightObjects, m_heightGroup, heightInstances);
        }

        private void ReadjustMeasureVisual()
        {
            for (int i = 0; i < m_widthObjects.Count; i++)
            {
                m_widthObjects[i].localPosition = new Vector3(1 + (i * 2), 1, 0);
                m_widthObjects[i].localScale = Vector3.one;
            }
            var widthRemainder = m_width - Mathf.FloorToInt(m_width);
            if (widthRemainder > 0)
            {
                var lastWidthObjectIndex = m_widthObjects.Count - 1;
                var lastWidthObject = m_widthObjects[lastWidthObjectIndex];
                lastWidthObject.localPosition = new Vector3(1 + (lastWidthObjectIndex * 2) - (1 - widthRemainder), 1, 0);
                lastWidthObject.localScale = new Vector3(widthRemainder, 1, 1);
            }

            for (int i = 0; i < m_heightObjects.Count; i++)
            {
                m_heightObjects[i].localPosition = new Vector3(1, 1 + (i * 2), 0);
                m_heightObjects[i].localScale = Vector3.one;
            }
            var heightRemainder = m_height - Mathf.FloorToInt(m_width);
            if (heightRemainder > 0)
            {
                var lastHeightObjectIndex = m_heightObjects.Count - 1;
                var lastHeightObject = m_heightObjects[lastHeightObjectIndex];
                lastHeightObject.localPosition = new Vector3(1, 1 + (lastHeightObjectIndex * 2) - (1 - heightRemainder), 0);
                lastHeightObject.localScale = new Vector3(1, heightRemainder, 1);
            }
        }

        private void InstantiateOrigin(GameObject origin)
        {
            var instance = Instantiate(origin) as GameObject;
            instance.name = "Origin";
            instance.transform.parent = transform;
            instance.transform.localPosition = Vector2.one;
            instance.transform.localScale = Vector3.one;
            m_widthObjects.Insert(0, instance.transform);
            m_heightObjects.Insert(0, instance.transform);
            m_origin = instance;
        }

        private void CleanChildren()
        {
            m_widthObjects.Clear();
            m_heightObjects.Clear();
            DestroyImmediate(m_origin);
            for (int i = m_widthGroup.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(m_widthGroup.GetChild(i).gameObject);
            }

            for (int i = m_heightGroup.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(m_heightGroup.GetChild(i).gameObject);
            }
        }

        private void CreateMeasureObjects(List<Transform> objectList, Transform parent, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var instance = Instantiate(m_reference) as GameObject;
                instance.transform.parent = parent;
                objectList.Add(instance.transform);
            }
        }

        private void DeleteMeasureObjects(List<Transform> objectList, int amount)
        {
            for (int i = objectList.Count - 1; i >= objectList.Count - 1 - amount; i--)
            {
                var toDelete = objectList[i];
                objectList.RemoveAt(i);
                DestroyImmediate(toDelete);
            }
        }

        private void AlignMeasureObjectCount(List<Transform> objectList, Transform parent, int amount)
        {
            if (objectList.Count > amount)
            {
                DeleteMeasureObjects(objectList, objectList.Count - amount);
            }
            else if (objectList.Count < amount)
            {
                CreateMeasureObjects(objectList, parent, amount - objectList.Count);
            }
        }

        private void OnValidate()
        {
            if (m_instantiated == false)
            {
                var widthGroup = new GameObject("WidthGroup");
                widthGroup.transform.parent = transform;
                widthGroup.transform.localPosition = Vector3.zero;
                widthGroup.transform.localScale = Vector3.one;
                m_widthGroup = widthGroup.transform;

                var heightGroup = new GameObject("HeightGroup");
                heightGroup.transform.parent = transform;
                heightGroup.transform.localPosition = Vector3.zero;
                heightGroup.transform.localScale = Vector3.one;
                m_heightGroup = heightGroup.transform;

                m_instantiated = true;
            }
        }
    }

}