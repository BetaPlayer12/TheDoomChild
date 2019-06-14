
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif

namespace DChild
{
    public static class EnumList
    {
        public static List<EnumElement> UpdateListCount<EnumElement, EnumType>(EnumElement[] array) where EnumElement : EnumElement<EnumType>, new()
                                                                                                  where EnumType : Enum, IConvertible
        {
            var maxCount = Enum.GetValues(typeof(EnumType)).Cast<int>().Max();
            List<EnumElement> newList = new List<EnumElement>();

            if (array.Length >= maxCount)
            {
                for (int i = 0; i < maxCount; i++)
                {
                    newList.Add(array[i]);
                }
            }
            else
            {
                for (int i = 0; i < array.Length; i++)
                {
                    newList.Add(array[i]);
                }

                for (int i = array.Length; i < maxCount; i++)
                {
                    newList.Add(new EnumElement());
                }
            }
            return newList;
        }

        public static List<EnumElement> UpdateListCount<EnumElement, EnumType>(List<EnumElement> list) where EnumElement : EnumElement<EnumType>, new()
                                                                                                       where EnumType : Enum, IConvertible
        {
            var maxCount = Enum.GetValues(typeof(EnumType)).Cast<int>().Max();
            List<EnumElement> newList = new List<EnumElement>();

            if (list.Count >= maxCount)
            {
                for (int i = 0; i < maxCount; i++)
                {
                    newList.Add(list[i]);
                }
            }
            else
            {
                for (int i = 0; i < list.Count; i++)
                {
                    newList.Add(list[i]);
                }

                for (int i = list.Count; i < maxCount; i++)
                {
                    newList.Add(new EnumElement());
                }
            }
            AlignListElementTypeToEnums<EnumElement, EnumType>(newList);
            return newList;
        }

        public static void AlignListElementTypeToEnums<EnumElement, EnumType>(List<EnumElement> newList)
        where EnumElement : EnumElement<EnumType>
        where EnumType : Enum, IConvertible
        {
#if UNITY_EDITOR
            for (int i = 0; i < newList.Count; i++)
            {
                newList[i].name = ToEnum<EnumType>(i);
            }
#endif
        }

        public static void AlignListElementTypeToEnums<EnumElement, EnumType>(EnumElement[] newList)
      where EnumElement : EnumElement<EnumType>
      where EnumType : Enum, IConvertible
        {
#if UNITY_EDITOR
            for (int i = 0; i < newList.Length; i++)
            {
                newList[i].name = ToEnum<EnumType>(i);
            }
#endif
        }

        public static EnumType ToEnum<EnumType>(int value) where EnumType : Enum, IConvertible => (EnumType)Enum.ToObject(typeof(EnumType), value);
    }

    [System.Serializable, HideReferenceObjectPicker]
    public class EnumList<T, U> where T : System.Enum
    {
        [NonSerialized, OdinSerialize]
        [ListDrawerSettings(HideAddButton = true, OnTitleBarGUI = "DrawUpdateButton"), HideReferenceObjectPicker]
        private List<EnumElement<T, U>> m_list;

        public EnumList()
        {
#if UNITY_EDITOR
            m_list = new List<EnumElement<T, U>>();
            var maxCount = Enum.GetValues(typeof(T)).Cast<int>().Max();
            for (int i = 0; i < maxCount; i++)
            {
                m_list.Add(new EnumElement<T, U>(EnumList.ToEnum<T>(i)));
            }
#endif
        }

        public List<U> ToList() => m_list.Select(x => x.value).ToList();
        public U[] ToArray() => m_list.Select(x => x.value).ToArray();

#if UNITY_EDITOR
        private void DrawUpdateButton()
        {
            var maxCount = Enum.GetValues(typeof(T)).Cast<int>().Max();

            if (SirenixEditorGUI.ToolbarButton(EditorIcons.Refresh))
            {
                if (m_list.Count != maxCount)
                {
                    List<EnumElement<T, U>> newList = new List<EnumElement<T, U>>();
                    if (m_list == null)
                    {
                        for (int i = 0; i < maxCount; i++)
                        {
                            newList.Add(new EnumElement<T, U>(EnumList.ToEnum<T>(i)));
                        }
                    }
                    else
                    {
                        EnumList.UpdateListCount<EnumElement<T, U>, T>(newList);
                    }
                    m_list = newList;
                }

                EnumList.AlignListElementTypeToEnums<EnumElement<T, U>, T>(m_list);
            }
        }
#endif
    }

    [System.Serializable, HideReferenceObjectPicker]
    public class EnumList<S, T, U> where S : EnumElement<T, U>, new() where T : System.Enum
    {
        [SerializeField]
        [ListDrawerSettings(HideAddButton = true, OnTitleBarGUI = "DrawUpdateButton", HideRemoveButton = true)]
        private List<S> m_list;

        public EnumList()
        {
#if UNITY_EDITOR
            m_list = new List<S>();
            var maxCount = Enum.GetValues(typeof(T)).Cast<int>().Max();
            for (int i = 0; i < maxCount; i++)
            {
                var entry = new S();
                entry.SetName(EnumList.ToEnum<T>(i));
                m_list.Add(entry);
            }
#endif
        }

        public List<U> ToList() => m_list.Select(x => x.value).ToList();
        public U[] ToArray() => m_list.Select(x => x.value).ToArray();

#if UNITY_EDITOR
        private void DrawUpdateButton()
        {
            var maxCount = Enum.GetValues(typeof(T)).Cast<int>().Max();

            if (SirenixEditorGUI.ToolbarButton(EditorIcons.Refresh))
            {
                if (m_list.Count != maxCount)
                {
                    List<S> newList = new List<S>();
                    if (m_list == null)
                    {
                        for (int i = 0; i < maxCount; i++)
                        {
                            var entry = new S();
                            entry.SetName(EnumList.ToEnum<T>(i));
                            newList.Add(entry);
                        }
                    }
                    else
                    {
                        EnumList.UpdateListCount<S, T>(newList);
                    }
                    m_list = newList;
                }

                EnumList.AlignListElementTypeToEnums<S, T>(m_list);
            }
        }
#endif
    }
}