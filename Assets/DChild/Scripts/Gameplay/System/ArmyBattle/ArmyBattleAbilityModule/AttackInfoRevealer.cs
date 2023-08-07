using UnityEngine;
using DChild.Gameplay.ArmyBattle;




public class AttackInfoRevealer : IArmyAbilityEffect
{
    [SerializeField]
    private float m_percentageChance;
    [SerializeField]
    private bool m_fakeInfoRevealer;
    [SerializeField]
    private bool m_hostageReveal;

    private UnitType m_unityTypeInfo;



    public void ApplyEffect(Army owner, Army opponent)
    {
        m_unityTypeInfo = ArmyBattleSystem.GetArmyCombatHandleOf(opponent).attackInfo.type;
        float number = Random.Range(0f, 1f);
        var convertedPercentage = PercentageConverter.ConvertPercentage(m_percentageChance);

        Debug.Log(number+ " random number");
        Debug.Log(convertedPercentage + " Converted percentage");

        if (m_fakeInfoRevealer)
        {
            if (number >= 0f && number <= convertedPercentage)
            {
                Debug.Log(m_unityTypeInfo);
            }
            else
            {
                RevealingFakeInfo();
            }
        }
        else if (m_hostageReveal)
        {

            if (number >= 0f && number <= convertedPercentage)
            {
                Debug.Log(m_unityTypeInfo);
            }
            else
            {
                Debug.Log("No Info");
            }
        }
        else
        {
            if (number >= 0f && number <= convertedPercentage)
            {
                Debug.Log(m_unityTypeInfo);
            }
        }
        
        


    }

    private void RevealingFakeInfo()
    {
        switch (m_unityTypeInfo)
        {
            case UnitType.Rock:
                int rockMessage = Random.Range(1, 3);
                if (rockMessage == 1)
                {
                    Debug.Log("Paper" + " fake");
                }
                else
                {
                    Debug.Log("Scissors" + " fake");
                }
                break;
            case UnitType.Paper:
                int paperMessage = Random.Range(1, 3);
                if (paperMessage == 1)
                {
                    Debug.Log("Rock" + " fake");
                }
                else
                {
                    Debug.Log("Scissors" + " fake");
                }
                break;
            case UnitType.Scissors:
                int scissorsMessage = Random.Range(1, 3);
                if (scissorsMessage == 1)
                {
                    Debug.Log("Rock" + " fake");
                }
                else
                {
                    Debug.Log("Paper" + " fake");
                }
                break;
        }
    }

}



