CREATE ROLE [aspnet_Profile_ReportingAccess]
    AUTHORIZATION [dbo];


GO
sp_addrolemember [aspnet_Profile_ReportingAccess], [aspnet_Profile_FullAccess];

