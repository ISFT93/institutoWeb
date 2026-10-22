USE instituto_db;
GO

ALTER PROC [dbo].[sp_insertCursosMaterias]
	@NombreCurso varchar(10),
	@AnioCarreraId int AS
	declare @aniocarreracodigobloque varchar(10)
	declare @codigobloquecurso varchar(10)

	select @aniocarreracodigobloque = AniosCarrerasCodigoBloque 
	from AniosCarreras 
	where AnioCarreraId = @AnioCarreraId

	
	set @codigobloquecurso = CONCAT(@aniocarreracodigobloque, @NombreCurso)

	INSERT INTO Cursos(NombreCurso, CodigoBloque, AnioCarreraId, Activo)
    VALUES(@NombreCurso, @codigobloquecurso, @AnioCarreraId, 1)

	DECLARE @CursoId int
    select @CursoId = @@IDENTITY

	DECLARE @fecha date
	set @fecha = GETDATE()

	
	INSERT INTO CursoMaterias(MateriaId, CursoId, FechaAlta, Activo, CodigoBloque)
	SELECT 
		MateriaId, 
		@CursoId,
		@fecha,
		1,
		CONCAT(MateriasCodigoBloque, @NombreCurso)
	from Materias 
	where AnioCarreraId = @AnioCarreraId
GO

----borrar tabla pedida de cursos

IF COL_LENGTH(N'dbo.Cursos', N'AdmiteCurso') IS NOT NULL
ALTER TABLE [dbo].[Cursos]
DROP COLUMN [AdmiteCurso]
GO