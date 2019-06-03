insert or replace into Accessory(Item_Name) 
select Name from Item where ItemType_ID = 4;

delete from Accessory where exists
(select Name from Item where Accessory.Item_Name = Item.Name AND Item.ItemType_ID != 4);

update Accessory
Set HasEffects = case when exists(select null from ItemWithEffects where ItemWithEffects.Item_Name =Accessory.Item_Name) 
then 1 else 0 end ;
