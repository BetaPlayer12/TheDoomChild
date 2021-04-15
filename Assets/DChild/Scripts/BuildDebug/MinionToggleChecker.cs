using DChild.Gameplay;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug.Window
{
    public class MinionToggleChecker : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_minions;

        private void Toggle(object sender, EventActionArgs eventArgs)
        {
            if (MinionToggle.toggleValue == true)
            {
                m_minions.SetActive(true);
            }
            else
            {
                Debug.Log("it worked");
                m_minions.SetActive(false);
            }


        }

        private void Start()
        {
            MinionToggle.minionToggleInstance.OnToggle += Toggle;
        }

    }
}
