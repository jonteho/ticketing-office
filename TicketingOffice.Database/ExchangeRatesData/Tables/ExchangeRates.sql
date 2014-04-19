CREATE TABLE [ExchangeRatesData].[ExchangeRates] (
    [Currency] NCHAR (10) NOT NULL,
    [Rate]     FLOAT (53) NOT NULL,
    CONSTRAINT [PK_ExchangeRates] PRIMARY KEY CLUSTERED ([Currency] ASC)
);

