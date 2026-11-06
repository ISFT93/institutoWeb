USE [instituto_db]
GO

ALTER TABLE [dbo].[MesasFinales]
ADD [LibroActaId] INT NULL,
[NroFolio] CHAR(3) NULL
GO

ALTER TABLE [dbo].[MesasFinales]
ADD CONSTRAINT [FK_MesasFinales_LibroActas]
FOREIGN KEY ([LibroActaId])
REFERENCES [dbo].[LibroActas] ([LibroActaId]);
GO