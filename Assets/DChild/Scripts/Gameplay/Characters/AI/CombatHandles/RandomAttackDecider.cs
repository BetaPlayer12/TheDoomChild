using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.AI
{
    public struct AttackInfo<T> where T : System.Enum
    {
        public AttackInfo(T attack, float range) : this()
        {
            this.attack = attack;
            this.range = range;
        }

        [ShowInInspector]
        public T attack { get; }
        public float range { get; }
    }

    [System.Serializable, HideInEditorMode]
    public class RandomAttackDecider<T> where T : System.Enum
    {

        public RandomAttackDecider()
        {
            attackList = new List<AttackInfo<T>>();
        }

        public bool hasDecidedOnAttack;
        [ShowInInspector, OnValueChanged("ValidateAttack")]
        public AttackInfo<T> chosenAttack { get; private set; }
        private List<AttackInfo<T>> attackList;


        public void SetList(params AttackInfo<T>[] list)
        {
            attackList.Clear();
            attackList.AddRange(list);
        }

        public void DecideOnAttack()
        {
            if (hasDecidedOnAttack == false)
            {
                var index = UnityEngine.Random.Range(0, attackList.Count);
                chosenAttack = attackList[index];
                hasDecidedOnAttack = true;
            }
        }

        public void DecideOnAttack(params T[] list)
        {
            if (hasDecidedOnAttack == false)
            {
                var index = UnityEngine.Random.Range(0, list.Length - 1);
                var enumIndex = Convert.ToInt32(list[index]);
                for (int i = 0; i < attackList.Count; i++)
                {
                    if (Convert.ToInt32(attackList[i].attack) == enumIndex)
                    {
                        chosenAttack = attackList[i];
                        break;
                    }
                }
                hasDecidedOnAttack = true;
            }
        }

#if UNITY_EDITOR
        private void ValidateAttack()
        {
            hasDecidedOnAttack = true;
            var enumIndex = Convert.ToInt32(chosenAttack.attack);
            for (int i = 0; i < attackList.Count; i++)
            {
                if (Convert.ToInt32(attackList[i].attack) == enumIndex)
                {
                    chosenAttack = attackList[i];
                    break;
                }
            }
        }
#endif
    }
}