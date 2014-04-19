--ALTER ROLE [db_owner] ADD MEMBER [LocalService];


--GO
--ALTER ROLE [db_owner] ADD MEMBER [NetworkService];

--sp_addrolemember  [db_owner], [NetworkService];