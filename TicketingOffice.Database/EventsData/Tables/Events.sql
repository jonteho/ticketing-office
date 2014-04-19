CREATE TABLE [EventsData].[Events] (
    [ID]            UNIQUEIDENTIFIER NOT NULL,
    [ShowID]        INT              NOT NULL,
    [TheaterID]     INT              NOT NULL,
    [StartTime]     DATETIME         NOT NULL,
    [Date]          DATETIME         NOT NULL,
    [ListPrice]     INT              NOT NULL,
    [State]         NCHAR (10)       NULL,
    [PricingPolicy] VARCHAR (50)     NULL,
    CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Events_Shows] FOREIGN KEY ([ShowID]) REFERENCES [ShowsData].[Shows] ([ID])
);

