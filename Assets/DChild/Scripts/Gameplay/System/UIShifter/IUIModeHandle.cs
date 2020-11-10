﻿using DChild.Gameplay.Characters.NPC;
using DChild.Gameplay.Inventories;
using DChild.Menu.Trading;
using System.Collections;
using System.Collections.Generic;

namespace DChild.Gameplay.Systems
{
    public interface IUIModeHandle
    {
        void OpenTradeWindow(NPCProfile merchantData,ITradableInventory merchantInventory, ITraderAskingPrice merchantAskingPrice);
        void OpenStorePage(StorePage storePage);
        void OpenStorePage();
    }
}
