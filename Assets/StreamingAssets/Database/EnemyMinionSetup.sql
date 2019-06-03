insert or replace into EnemyMinion(Enemy_Name) 
select Name from Enemy where EnemyType_ID = 1;

delete from EnemyMinion where not exists
(select Name from Enemy where EnemyType_ID = 1);