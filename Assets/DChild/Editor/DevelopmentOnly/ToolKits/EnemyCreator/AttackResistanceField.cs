using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChildEditor.Toolkit.EnemyCreation
{
    [System.Serializable]
    public class AttackResistanceField : DataField<AttackResistanceData>
    {
        public void Apply(GameObject instance, GameObject stats, string path)
        {
            if (m_createNewData)
            {
                CreateData(path, "AttackResistanceData");
            }
            var resistance = stats.AddComponent<BasicAttackResistance>();
            resistance.SetData(m_data);
            instance.GetComponent<Damageable>().InitializeField(resistance);
        }
    }
}