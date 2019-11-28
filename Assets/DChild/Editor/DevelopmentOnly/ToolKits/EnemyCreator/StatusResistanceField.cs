using DChild.Gameplay.Combat.StatusAilment;
using UnityEngine;

namespace DChildEditor.Toolkit.EnemyCreation
{
    [System.Serializable]
    public class StatusResistanceField : DataField<StatusEffectChanceData>
    {
        public void Apply(GameObject instance, GameObject stats, string path)
        {
            if (m_createNewData)
            {
                CreateData(path, "StatusResistanceData");
            }
            var resistance = stats.AddComponent<StatusEffectResistance>();
            resistance.SetData(m_data);
            instance.GetComponent<StatusEffectReciever>().InitializeField(resistance);
        }
    }
}