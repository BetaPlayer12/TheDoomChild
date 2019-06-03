delete from ItemWithEffects where not exists
(Select iwe.Item_Name
from ItemWithEffects iwe
join Item i on iwe.Item_Name = i.Name
join ItemEffect ie on iwe.Effect_Name = ie.Name
where 
(i.ItemType_ID = 1 and ie.IsOnConsume = true ) or
(i.ItemType_ID = 2 and ie.IsOnEquip = true ) or
(i.ItemType_ID = 3 and (ie.IsOnHit = true or ie.IsOnKill = true)) or
(i.ItemType_ID = 4 and (ie.IsOnHit = true or ie.IsOnEquip = true)))
;