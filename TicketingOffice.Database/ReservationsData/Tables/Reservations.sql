CREATE TABLE [ReservationsData].[Reservations] (
    [ID]         UNIQUEIDENTIFIER NOT NULL,
    [HallID]     INT              NOT NULL,
    [EventID]    UNIQUEIDENTIFIER NOT NULL,
    [Seats]      VARCHAR (MAX)    NULL,
    [CustomerID] UNIQUEIDENTIFIER NULL,
    [Remark]     VARCHAR (MAX)    NULL,
    CONSTRAINT [PK_Reservations] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Reservations_Theaters] FOREIGN KEY ([HallID]) REFERENCES [TheatersData].[Theaters] ([ID])
);

