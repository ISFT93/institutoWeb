USE instituto_db8;
GO

IF COL_LENGTH(N'dbo.MesaAlumno', N'Ausente') IS NULL
BEGIN
    ALTER TABLE dbo.MesaAlumno
        ADD Ausente BIT NOT NULL
            CONSTRAINT DF_MesaAlumno_Ausente DEFAULT (0);
END;
GO
