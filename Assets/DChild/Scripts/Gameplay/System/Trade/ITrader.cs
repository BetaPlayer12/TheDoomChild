using DChild.Gameplay.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Trade
{
    public interface ITrader
    {
        ITradeInventory inventory { get; }
    }

}