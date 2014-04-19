CREATE TABLE [ShowsData].[Shows] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [Title]       VARCHAR (50)   NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    [Url]         NVARCHAR (MAX) NULL,
    [Preview]     VARCHAR (MAX)  NULL,
    [Cast]        VARCHAR (MAX)  NULL,
    [Duration]    INT            NULL,
    [Logo]        IMAGE          NULL,
    [Category]    VARCHAR (50)   NULL,
    CONSTRAINT [PK_Shows] PRIMARY KEY CLUSTERED ([ID] ASC)
);

