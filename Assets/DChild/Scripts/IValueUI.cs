using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.UI
{
    public interface IValueUI
    {
        void UpdateUI();
    }

    public interface IValueUI<T>
    {
        void UpdateUI(T reference);
    }
}