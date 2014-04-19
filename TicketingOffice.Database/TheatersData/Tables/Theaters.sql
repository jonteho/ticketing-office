CREATE TABLE [TheatersData].[Theaters] (
    [ID]       INT          IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (50) NOT NULL,
    [City]     VARCHAR (50) NOT NULL,
    [Country]  VARCHAR (50) NOT NULL,
    [Capacity] INT          NULL,
    [Map]      IMAGE        NULL,
    CONSTRAINT [PK_Theaters] PRIMARY KEY CLUSTERED ([ID] ASC)
);

