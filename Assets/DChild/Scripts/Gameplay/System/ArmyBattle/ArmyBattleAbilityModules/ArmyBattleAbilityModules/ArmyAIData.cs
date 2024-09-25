using UnityEngine;
using DChild.Gameplay.ArmyBattle;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "ArmyCharacter", menuName = "DChild/Gameplay/Army/Army AI Data")]
public class ArmyAIData : SerializedScriptableObject
{
  
    [SerializeField]
    private IArmyAIAction[] m_aiAction;
    [SerializeField]
    private RandomizedArmyAttackHandle m_randomAttack;

    
    public ArmyGroupData ChooseAttack(int round)
    {
        /*  var minusRoud = round - 1;
          for (int i = 0; i < m_aiAction.Length; i++)
          {
              if(m_aiAction.Length >= minusRoud)
              {
                  if(m_aiAction[i].isRandomizedAction == false)
                  {
                      Debug.Log("Not random");
                      return m_aiAction[i].GetAction();

                  }
                  else
                  {
                      Debug.Log("random");
                      return m_randomAttack.Randomized();
                  }
              }

          }
          Debug.Log("Random");
          return m_randomAttack.Randomized();*/
        var actionIndex = round - 1;
        if (m_aiAction.Length >= round)
        {
            if (m_aiAction[actionIndex].isRandomizedAction == false)
            {
                Debug.Log("Not random");
                return m_aiAction[actionIndex].GetAction();

            }
            else
            {
                Debug.Log("random");
                return m_randomAttack.Randomized();
            }
        }
        else
        {
            return m_randomAttack.Randomized();
        }
    }

}

