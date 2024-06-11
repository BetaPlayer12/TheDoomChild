using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
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
            m_sameAttackCount = -1;
            m_maxSameAttackCount = 0;
        }

        public bool hasDecidedOnAttack;
        [ShowInInspector, OnValueChanged("ValidateAttack")]
        public AttackInfo<T> chosenAttack { get; private set; }
        private List<AttackInfo<T>> attackList;
        private int m_maxSameAttackCount;
        private int m_previousChosenAttack;
        private int m_sameAttackCount;

        public void SetList(params AttackInfo<T>[] list)
        {
            attackList.Clear();
            attackList.AddRange(list);
        }

        public void DecideOnAttack()
        {
            if (hasDecidedOnAttack == false)
            {
                bool sameAttack = false;
                int chosenAttackIndex = -1;
                do
                {
                    var index = UnityEngine.Random.Range(0, attackList.Count);
                    chosenAttack = attackList[index];
                    chosenAttackIndex = Convert.ToInt32(chosenAttack.attack);
                    sameAttack = m_previousChosenAttack == chosenAttackIndex;

                } while (m_maxSameAttackCount > 0 && sameAttack && m_maxSameAttackCount <= m_sameAttackCount);
                EvaluateSameAttack(sameAttack, chosenAttackIndex);
                hasDecidedOnAttack = true;
            }

            Debug.Log($"chosen attack : {chosenAttack.attack} \n max same attack count : {m_sameAttackCount}");
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxRepeatAttack">If you want the attacks to repeat infinitely value should be <=0 </param>
        public void SetMaxRepeatAttack(int maxRepeatAttack)
        {
            m_maxSameAttackCount = maxRepeatAttack;
            m_sameAttackCount = Math.Min(m_sameAttackCount, m_maxSameAttackCount);
        }

        public void DecideOnAttack(params T[] list)
        {
            if (hasDecidedOnAttack == false)
            {
                bool sameAttack = false;
                int chosenAttackIndex = -1;
                int index = -1;
                do
                {
                    index = UnityEngine.Random.Range(0, list.Length - 1);
                    //chosenAttack = attackList[index];
                    var enumIndex = Convert.ToInt32(list[index]);
                    sameAttack = m_previousChosenAttack == enumIndex;
                    chosenAttackIndex = enumIndex;
                } while (m_maxSameAttackCount > 0 && sameAttack && m_maxSameAttackCount == m_sameAttackCount);


                bool isDecidedAttackPartOfList = false;
                for (int i = 0; i < attackList.Count; i++)
                {
                    if (Convert.ToInt32(attackList[i].attack) == chosenAttackIndex)
                    {
                        chosenAttack = attackList[i];
                        isDecidedAttackPartOfList = true;
                        break;
                    }
                }

                if (isDecidedAttackPartOfList ==false)
                {
                    chosenAttack = new AttackInfo<T>(list[index], 0);
                }

                EvaluateSameAttack(sameAttack, chosenAttackIndex);
                hasDecidedOnAttack = true;
            }
        }

        private void EvaluateSameAttack(bool sameAttack, int chosenAttackIndex)
        {
            if (sameAttack)
            {
                m_sameAttackCount++;
            }
            else
            {
                m_sameAttackCount = 1;
                m_previousChosenAttack = chosenAttackIndex;
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