CREATE TABLE [CustomerData].[Customers] (
    [ID]            UNIQUEIDENTIFIER NOT NULL,
    [FirstName]     VARCHAR (50)     NOT NULL,
    [LastName]      VARCHAR (50)     NOT NULL,
    [Birthdate]     DATETIME         NULL,
    [PhoneNumber]   NVARCHAR (50)    NULL,
    [CellNumber]    NVARCHAR (50)    NULL,
    [Email]         VARCHAR (50)     NULL,
    [Address]       VARCHAR (50)     NULL,
    [Country]       VARCHAR (50)     NULL,
    [ReductionCode] INT              NULL,
    [City]          VARCHAR (50)     NULL,
    CONSTRAINT [PK_Customers_1] PRIMARY KEY CLUSTERED ([ID] ASC)
);

