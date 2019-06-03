insert or replace into Armor(Item_Name) 
select Name from Item where ItemType_ID = 2;

delete from Armor where exists
(select Name from Item where Armor.Item_Name = Item.Name AND Item.ItemType_ID != 2);

update Armor
Set HasEffects = case when exists(select null from ItemWithEffects where ItemWithEffects.Item_Name =Armor.Item_Name) 
then 1 else 0 end ;
