CREATE ROLE [aspnet_Profile_BasicAccess]
    AUTHORIZATION [dbo];


GO
sp_addrolemember  [aspnet_Profile_BasicAccess], [aspnet_Profile_FullAccess];

