CREATE ROLE [aspnet_Personalization_BasicAccess]
    AUTHORIZATION [dbo];


GO
sp_addrolemember [aspnet_Personalization_BasicAccess], [aspnet_Personalization_FullAccess];

