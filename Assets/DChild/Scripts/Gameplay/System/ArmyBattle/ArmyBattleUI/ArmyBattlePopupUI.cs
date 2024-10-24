using Doozy.Runtime.Signals;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.UI
{
    public class ArmyBattlePopupUI : MonoBehaviour
    {
        [SerializeField]
        private string m_signalCategory;
        [SerializeField]
        private string m_signalName;
        [SerializeField]
        private TextMeshProUGUI m_popupLabel;


        private SignalReceiver m_signalReceiver;
        private SignalStream m_signalStream;

        private void Awake()
        {
            m_signalStream = SignalStream.Get(m_signalCategory, m_signalName);
            m_signalReceiver = new SignalReceiver().SetOnSignalCallback(OnSignal);
        }

        private void OnEnable()
        {
            m_signalStream.ConnectReceiver(m_signalReceiver);
        }

        private void OnDisable()
        {
            m_signalStream.DisconnectReceiver(m_signalReceiver);
        }

        private void OnSignal(Signal signal)
        {

            if (signal.valueType != typeof(bool))
            {
                m_popupLabel.text = "BATTLE";
                return;
            }

            bool battleResult = (bool)signal.valueAsObject;

            if (battleResult != true)
            {
                m_popupLabel.text = "DEFEAT";
                return;
            }
            m_popupLabel.text = "VICTORY";
        }
    }
}