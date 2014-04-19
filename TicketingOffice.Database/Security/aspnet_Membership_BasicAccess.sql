CREATE ROLE [aspnet_Membership_BasicAccess]
    AUTHORIZATION [dbo];


GO
sp_addrolemember [aspnet_Membership_BasicAccess], [aspnet_Membership_FullAccess];

