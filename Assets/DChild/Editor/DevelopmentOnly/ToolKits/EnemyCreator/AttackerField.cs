using DChild.Gameplay;
using DChild.Gameplay.Combat;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace DChildEditor.Toolkit.EnemyCreation
{
    [System.Serializable]
    public class AttackerField : DataField<AttackData>
    {
        public void Apply(GameObject instance, string path)
        {
            if (m_createNewData)
            {
                CreateData(path,"AttackerData");
            }
            var attacker = instance.AddComponent<Attacker>();
            attacker.SetData(m_data);
            attacker.InitializeField(instance.transform);
        }
    }
}