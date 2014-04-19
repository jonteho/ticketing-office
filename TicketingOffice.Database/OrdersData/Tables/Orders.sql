CREATE TABLE [OrdersData].[Orders] (
    [ID]             UNIQUEIDENTIFIER NOT NULL,
    [ReservationID]  UNIQUEIDENTIFIER NOT NULL,
    [TotalPrice]     DECIMAL (18)     NOT NULL,
    [DateOfPurchase] DATETIME         NULL,
    [State]          NCHAR (10)       NULL,
    [CustomerID]     UNIQUEIDENTIFIER NOT NULL,
    [EventID]        UNIQUEIDENTIFIER NOT NULL,
    [Remarks]        VARCHAR (MAX)    NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED ([ID] ASC)
);

