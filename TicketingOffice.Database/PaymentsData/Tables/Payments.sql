CREATE TABLE [PaymentsData].[Payments] (
    [ID]              BIGINT           IDENTITY (1, 1) NOT NULL,
    [Amount]          INT              NOT NULL,
    [Date]            DATETIME         NOT NULL,
    [MethodOfPayment] INT              NOT NULL,
    [OrderID]         UNIQUEIDENTIFIER NOT NULL,
    [CustomerID]      UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Payments] PRIMARY KEY CLUSTERED ([ID] ASC)
);

