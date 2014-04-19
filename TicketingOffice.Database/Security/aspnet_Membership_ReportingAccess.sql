CREATE ROLE [aspnet_Membership_ReportingAccess]
    AUTHORIZATION [dbo];


GO
sp_addrolemember [aspnet_Membership_ReportingAccess], [aspnet_Membership_FullAccess];

