CREATE ROLE [aspnet_Personalization_ReportingAccess]
    AUTHORIZATION [dbo];


GO
sp_addrolemember [aspnet_Personalization_ReportingAccess], [aspnet_Personalization_FullAccess];

