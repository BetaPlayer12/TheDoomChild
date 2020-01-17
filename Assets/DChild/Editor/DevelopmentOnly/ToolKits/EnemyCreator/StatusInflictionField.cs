using DChild.Gameplay.Combat.StatusAilment;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChildEditor.Toolkit.EnemyCreation
{
    [System.Serializable]
    public class StatusInflictionField : DataField<StatusEffectChanceData>
    {
        public void Apply(GameObject instance, string path)
        {
            if (m_createNewData)
            {
                CreateData(path, "StatusInflictionData");
            }

            instance.AddComponent<StatusInflictor>().SetData(m_data);
        }
    }
}