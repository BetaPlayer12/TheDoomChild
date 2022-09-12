using System;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class ChargeAttackHandle
    {
        private IChargeAttackBehaviour m_instance;
        private Func<bool> GetInput;

        public void Set(IChargeAttackBehaviour instance, Func<bool> inputFunc)
        {
            m_instance = instance;
            GetInput = inputFunc;
        }

        public void Execute()
        {
            if (m_instance != null)
            {
                if (m_instance?.IsChargeComplete() ?? false)
                {
                    if (GetInput() == false)
                    {
                        m_instance?.Execute();
                    }
                    else
                    {
                        m_instance.HandleCharge();
                    }
                }
                else
                {
                    if (GetInput() == true)
                    {
                        m_instance.HandleCharge();
                    }
                    else
                    {
                        m_instance.Cancel();
                    }
                }
            }
        }
    }
}
