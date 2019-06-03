insert or replace into Consumable(Item_Name) 
select Name from Item where ItemType_ID = 1;

delete from Consumable where exists
(select Name from Item where Consumable.Item_Name = Item.Name AND Item.ItemType_ID != 1);