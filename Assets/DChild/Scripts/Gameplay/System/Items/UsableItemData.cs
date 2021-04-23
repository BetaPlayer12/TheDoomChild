using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Items
{
    [CreateAssetMenu(fileName = "UsableItemData", menuName = "DChild/Database/Usable Item Data")]
    public class UsableItemData : ConsumableItemData
    {
        [SerializeField, BoxGroup("m_enableEdit/Effect")]
        private bool m_durationEffect;
        [SerializeField, ShowIf("m_durationEffect"), MinValue(0), BoxGroup("m_enableEdit/Effect")]
        private float m_duration;
#if UNITY_EDITOR
        [SerializeField, HideLabel, DisplayAsString, TabGroup("m_enableEdit/Effect/Tab", "Summary"), InfoBox("$UpdateSummary")]
        private string m_moduleSummary = "";
#endif
        [SerializeField, HideReferenceObjectPicker, TabGroup("m_enableEdit/Effect/Tab", "Instant")]
        private IUsableItemModule[] m_moduleList = new IUsableItemModule[0];
        [SerializeField, HideReferenceObjectPicker, ShowIf("m_durationEffect"), TabGroup("m_enableEdit/Effect/Tab/Duration", "OnDuration")]
        private IDurationItemEffect[] m_durationEffectList = new IDurationItemEffect[0];
        [SerializeField, HideReferenceObjectPicker, ShowIf("m_durationEffect"), TabGroup("m_enableEdit/Effect/Tab/Duration", "Updatable")]
        private IUpdatableItemEffect[] m_updatableEffectList = new IUpdatableItemEffect[0];


        private static Dictionary<IPlayer, DurationItemHandle> m_activeList = new Dictionary<IPlayer, DurationItemHandle>();

        public override bool CanBeUse(IPlayer player)
        {
            if (m_moduleList != null)
            {
                for (int i = 0; i < m_moduleList.Length; i++)
                {
                    if (m_moduleList[i].CanBeUse(player) == false)
                        return false;
                }
            }

            return true;
        }

        public override void Use(IPlayer player)
        {
#if UNITY_EDITOR
            Debug.Log($"{itemName} Consumed");
#endif
            if (m_moduleList != null)
            {
                for (int i = 0; i < m_moduleList.Length; i++)
                {
                    m_moduleList[i].Use(player);
                }
            }
            if (m_durationEffect)
            {
                if (m_activeList.TryGetValue(player, out DurationItemHandle handle))
                {
                    handle.ResetTimer();
                }
                else
                {
                    handle = new DurationItemHandle(player, m_duration, m_durationEffectList, m_updatableEffectList);
                    handle.EffectEnd += OnEffectEnd;
                    m_activeList.Add(player, handle);
                    handle.Start();
                }
            }
        }

        private void OnEffectEnd(object sender, EventActionArgs eventArgs)
        {
            m_activeList.Remove(((DurationItemHandle)sender).player);
        }

#if UNITY_EDITOR
        private string UpdateSummary()
        {
            m_moduleSummary = "";
            var moduleSummary = "";
            if (m_moduleList.Length > 0)
            {
                moduleSummary += "Immidiately: \n";
                for (int i = 0; i < m_moduleList.Length; i++)
                {
                    moduleSummary += "-" + m_moduleList[i].ToString() + "\n";
                }
                moduleSummary += "\n";
            }

            if (m_durationEffect)
            {
                moduleSummary += $"For {m_duration} seconds: \n";
                for (int i = 0; i < m_durationEffectList.Length; i++)
                {
                    moduleSummary += "-" + m_durationEffectList[i].ToString() + "\n";
                }
                for (int i = 0; i < m_updatableEffectList.Length; i++)
                {
                    moduleSummary += "-" + m_updatableEffectList[i].ToString() + "\n";
                }
            }

            if (m_moduleList.Length == 0 && m_durationEffectList.Length == 0 && m_updatableEffectList.Length == 0)
            {
                moduleSummary += "Does absolutely nothing... Why?";
            }
            return moduleSummary;
        }
#endif
    }
}
