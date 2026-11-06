go
alter table cursos
drop column  admitecurso
go

ALTER PROC [dbo].[sp_insertCursosMaterias]
	@NombreCurso varchar(10),
	@AnioCarreraId int
AS
	declare @aniocarreracodigobloque varchar(10)
	declare @aniolectivo varchar(2)
	declare @codigobloquecurso varchar(10)

	select @aniocarreracodigobloque = AniosCarrerasCodigoBloque 
	from AniosCarreras 
	where AnioCarreraId = @AnioCarreraId

	select @aniolectivo = RIGHT(CAST(AnioLectivo AS varchar(4)), 2) 
	from CicloLectivo 
	where Activo = 1

	set @codigobloquecurso = CONCAT(@aniocarreracodigobloque, @NombreCurso)

	INSERT INTO Cursos(NombreCurso, CodigoBloque, AnioCarreraId, Activo)
    VALUES(@NombreCurso, @codigobloquecurso, @AnioCarreraId, 1 )

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
		m.MateriasCodigoBloque + @NombreCurso
	from Materias m
	where AnioCarreraId = @AnioCarreraId