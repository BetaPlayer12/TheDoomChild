using Holysoft.Event;
using Holysoft.UI;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Holysoft.Menu
{
    public interface ICanvasBackTracker
    {
        void Stack(UICanvas canvas);
        void BackTrack();
    }

    public class CanvasBackTracker : MonoBehaviour, ICanvasBackTracker
    {
        [SerializeField]
        [ReadOnly]
        protected List<UICanvas> m_stack;

        public bool canBacktrack => m_stack.Count > 1;

        public bool HasStacked(UICanvas canvas) => m_stack.Contains(canvas);

        public void Stack(UICanvas canvas)
        {
            if (m_stack.Count == 0)
            {
                m_stack.Insert(0, canvas);
            }
            else if (m_stack[0] != canvas)
            {
                m_stack.Insert(0, canvas);
            }
        }

        public void RemoveLastStack() => m_stack.RemoveAt(0);

        public void ClearStack() => m_stack.Clear();

        public virtual void BackTrack()
        {
            if (canBacktrack)
            {
                m_stack[0].Hide();
                m_stack[1].Show();
                m_stack.RemoveAt(0);
            }
        }
    }
}