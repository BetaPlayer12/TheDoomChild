insert or replace into Weapon(Item_Name) 
select Name from Item where ItemType_ID = 3;

delete from Weapon where exists
(select Name from Item where Weapon.Item_Name = Item.Name AND Item.ItemType_ID != 3);

update Weapon
Set HasEffects = case when exists(select null from ItemWithEffects where ItemWithEffects.Item_Name =Weapon.Item_Name) 
then 1 else 0 end ;


insert or replace into WeaponDamage(Weapon_Name) 
select Name from Item where ItemType_ID = 3;

delete from WeaponDamage where exists
(select Name from Item where WeaponDamage.Weapon_Name = Item.Name AND Item.ItemType_ID != 3);

insert or replace into WeaponProjectile(Weapon_Name) 
select w.Item_Name from Weapon w 
join WeaponType wt on w.WeaponType_ID = wt.ID
where wt.WeaponAttackMethod_ID = 2;

delete from WeaponProjectile where not exists
(select w.Item_Name from Weapon w 
join WeaponType wt on w.WeaponType_ID = wt.ID
where wt.WeaponAttackMethod_ID = 2);