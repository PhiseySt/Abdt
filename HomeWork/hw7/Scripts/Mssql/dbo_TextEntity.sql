USE [AbdtHome]
GO

/****** Объект: Table [dbo].[TextEntity] Дата скрипта: 18.05.2021 18:38:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TextEntity] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [TextValue]     NVARCHAR (MAX)   NULL,
    [CreatedDate]   DATETIME         NULL,
    [LastSavedDate] DATETIME         NULL,
    [CreatedBy]     UNIQUEIDENTIFIER NULL,
    [LastSavedBy]   UNIQUEIDENTIFIER NULL,
    [IsDeleted]     BIT              NULL
);


