CREATE TABLE [PricingData].[PricingRules] (
    [RuleID]         INT           IDENTITY (1, 1) NOT NULL,
    [PolicyName]     VARCHAR (50)  NOT NULL,
    [ReductionCode]  INT           NULL,
    [Reduction]      TINYINT       NOT NULL,
    [FromDate]       DATETIME      NULL,
    [ToDate]         DATETIME      NULL,
    [MinNumOfOrders] INT           NULL,
    [Remarks]        VARCHAR (MAX) NULL,
    CONSTRAINT [PK_PricingRules] PRIMARY KEY CLUSTERED ([RuleID] ASC)
);

