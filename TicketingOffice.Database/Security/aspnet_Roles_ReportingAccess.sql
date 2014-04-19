CREATE ROLE [aspnet_Roles_ReportingAccess]
    AUTHORIZATION [dbo];


GO
sp_addrolemember [aspnet_Roles_ReportingAccess], [aspnet_Roles_FullAccess];

