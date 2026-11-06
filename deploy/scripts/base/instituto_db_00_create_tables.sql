-- CREATE DATABASE IF NOT EXISTS instituto_db;
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'instituto_db') BEGIN
	CREATE DATABASE instituto_db;
END

GO

USE [instituto_db]
GO
/****** Object:  UserDefinedFunction [dbo].[funCicloLectivoActivo]    Script Date: 19/6/2026 17:21:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[funCicloLectivoActivo](
@FechaInicio date,
@FechaCierre date
)
RETURNS bit
AS
BEGIN
	IF (@FechaInicio <= GETDATE() AND @FechaCierre >= GETDATE()) RETURN 'True'
	RETURN 'False'
END
GO
/****** Object:  UserDefinedFunction [dbo].[funLicenciasActivo]    Script Date: 19/6/2026 17:21:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[funLicenciasActivo](
@FechaAlta date,
@FechaBaja date
)
RETURNS bit
AS
BEGIN
	DECLARE @Hoy DATE
	SET @Hoy = CAST(GETDATE() AS DATE)
	IF (@FechaAlta <= @Hoy AND (@FechaBaja IS NULL OR @FechaBaja >= @Hoy)) RETURN 'True'
	RETURN 'False'
END
GO
/****** Object:  Table [dbo].[Alumnos]    Script Date: 19/6/2026 17:21:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Alumnos](
	[AlumnoId] [int] IDENTITY(1,1) NOT NULL,
	[Apellido] [varchar](50) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[TipoDocumento] [varchar](10) NOT NULL,
	[NumeroDocumento] [varchar](10) NOT NULL,
	[EstadoCivil] [varchar](15) NULL,
	[Sexo] [char](1) NOT NULL,
	[FechaNacimiento] [date] NULL,
	[LocalidadNacimiento] [varchar](15) NULL,
	[PaisNacimiento] [varchar](50) NULL,
	[Calle] [varchar](255) NOT NULL,
	[Numero] [varchar](10) NULL,
	[Piso] [varchar](10) NULL,
	[Departamento] [varchar](10) NULL,
	[Provincia] [varchar](50) NULL,
	[Distrito] [varchar](50) NULL,
	[Localidad] [varchar](50) NOT NULL,
	[CodigoPostal] [varchar](10) NULL,
	[Telefono] [varchar](30) NULL,
	[Celular] [varchar](30) NULL,
	[Email] [varchar](255) NULL,
	[TituloSecundario] [bit] NULL,
	[MateriasAdeuda] [int] NULL,
	[DescripcionMaterias] [varchar](150) NULL,
	[Titulo] [varchar](50) NULL,
	[Orientacion] [varchar](50) NULL,
	[OtorgadoPor] [varchar](50) NULL,
	[AnioEgreso] [int] NULL,
	[Promedio] [decimal](18, 0) NULL,
	[TituloTramite] [bit] NULL,
	[MayorTitulo] [varchar](50) NULL,
	[OtroTitulo] [varchar](50) NULL,
	[MayorOtorgadoPor] [varchar](50) NULL,
	[MayorPromedio] [decimal](18, 0) NULL,
	[FotocopiaTitulo] [bit] NULL,
	[ConstanciaTituloTramite] [bit] NULL,
	[ConstanciaAdeudaMaterias] [bit] NULL,
	[CantidadAdeudaMaterias] [int] NULL,
	[CertificadoAptitud] [bit] NULL,
	[FotocopiaDocumento] [bit] NULL,
	[FotoCarnet] [bit] NULL,
	[FotocopiaPartidaNacimiento] [bit] NULL,
	[VacunaAntihepatitis] [bit] NULL,
	[VacunaAntitetanica] [bit] NULL,
	[Recibo] [int] NULL,
	[Monto] [int] NULL,
	[ObraSocialPrepaga] [bit] NULL,
	[DescripcionObraSocial] [varchar](50) NULL,
	[TratamientoMedico] [bit] NULL,
	[DescripcionTratamiento] [varchar](150) NULL,
	[Medicacion] [bit] NULL,
	[DescripcionMedicacion] [varchar](150) NULL,
	[Discapacidad] [bit] NULL,
	[DescripcionDiscapacidad] [varchar](150) NULL,
	[EstadoDiscapacidad] [varchar](15) NULL,
	[CertificadoDiscapacidad] [bit] NULL,
	[ContactoEmergencia] [varchar](100) NULL,
	[TelefonoContacto] [varchar](20) NULL,
	[Activo] [bit] NULL,
	[FotoUrl] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[AlumnoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlumnosCarreras]    Script Date: 19/6/2026 17:21:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlumnosCarreras](
	[AlumnoCarreraId] [int] IDENTITY(1,1) NOT NULL,
	[CarreraId] [int] NULL,
	[AlumnoId] [int] NULL,
	[FechaAlta] [date] NULL,
	[FechaBaja] [date] NULL,
	[Activo] [bit] NULL,
	[CicloLectivoId] [int] NULL,
	[Inicializado] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[AlumnoCarreraId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AniosCarreras]    Script Date: 19/6/2026 17:21:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AniosCarreras](
	[AnioCarreraId] [int] IDENTITY(1,1) NOT NULL,
	[AniosCarrerasCodigoBloque] [varchar](20) NULL,
	[AnioCarrera] [int] NOT NULL,
	[CantidadMaterias] [int] NULL,
	[CargaHorariaCompleta] [int] NULL,
	[CarreraId] [int] NULL,
	
PRIMARY KEY CLUSTERED 
(
	[AnioCarreraId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Carreras]    Script Date: 19/6/2026 17:21:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Carreras](
	[CarreraId] [int] IDENTITY(1,1) NOT NULL,
	[CarrerasCodigoBloque] [varchar](20) NULL,
	[Titulo] [varchar](255) NOT NULL,
	[Nombre] [varchar](255) NOT NULL,
	[DescripcionCorta] [varchar](50) NULL,
	[JefeCatedra] [varchar](50) NOT NULL,
	[AnioInicio] [int] NOT NULL,
	[AnioFin] [int] NULL,
	[Activo] [bit] NOT NULL,
	[PlanEstudio] [varchar](255) NULL,
	[Resolucion] [varchar](255) NULL,
	[Correlatividades] [varchar](255) NULL,
	[ImagenDescriptiva] [varchar](255) NULL,
	[NumeroExpediente] [varchar](20) NULL,
	[CantidadHoras] [int] NOT NULL,
	[Duracion] [int] NOT NULL,
	[CarreraEstadoId] [int] NOT NULL,
	
PRIMARY KEY CLUSTERED 
(
	[CarreraId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CursadaAlumnoCarreras]    Script Date: 19/6/2026 17:21:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CursadaAlumnoCarreras](
	[CursadaAlumnoCarreraId] [int] IDENTITY(1,1) NOT NULL,
	[AlumnoCarreraId] [int] NULL,
	[CursadaId] [int] NULL,
	[AnioCicloLectivo] [int] NULL,
	[Estado] [char](2) NULL,
	[HorasCursadas] [int] NULL,
	[UltimoPresentismo] [date] NULL,
	[PorcentajeAsistencia] [decimal](18, 0) NULL,
	[Cursada] [varchar](30) NULL,
	[Activo] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[CursadaAlumnoCarreraId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cursadas]    Script Date: 19/6/2026 17:21:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cursadas](
	[CursadaId] [int] IDENTITY(1,1) NOT NULL,
	[CursoMateriaId] [int] NULL,
	[AnioLectivo] [int] NULL,
	[Anio] [int] NULL,
	[CantidadAlumnos] [int] NULL,
	[CantidadAlumnosRecursantes] [int] NULL,
	[CantidadAlumnosDesertores] [int] NULL,
	[HoraCatedra] [int] NULL,
	[FechaAsistencia] [date] NULL,
	[PorcentajeAsistencia] [decimal](18, 0) NULL,
PRIMARY KEY CLUSTERED 
(
	[CursadaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CursoMaterias]    Script Date: 19/6/2026 17:21:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CursoMaterias](
	[CursoMateriaId] [int] IDENTITY(1,1) NOT NULL,
	[MateriaId] [int] NULL,
	[CursoId] [int] NULL,
	[FechaAlta] [date] NULL,
	[FechaBaja] [date] NULL,
	[Activo] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[CursoMateriaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cursos]    Script Date: 19/6/2026 17:21:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cursos](
	[CursoId] [int] IDENTITY(1,1) NOT NULL,
	[CodigoBloque] [varchar](10) NULL,
	[NombreCurso] [varchar](10) NULL,
	[AnioCarreraId] [int] NULL,
	[Activo] [bit] NULL,
	[AdmiteCurso] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CursoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Materias]    Script Date: 19/6/2026 17:21:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Materias](
	[MateriaId] [int] IDENTITY(1,1) NOT NULL,
	[MateriasCodigoBloque] [varchar](20) NULL,
	[Nombre] [varchar](255) NOT NULL,
	[AnioCarreraId] [int] NULL,
	[Activo] [bit] NOT NULL,
	[EspacioId] [int] NULL,
	[CargaHoraria] [int] NULL,
	[Modulos] [int] NOT NULL,
	[Multiple] [bit] NULL,
	[CarreraId] [int] NULL,
	[FinalPromocion] [char](1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MateriaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoExamen]    Script Date: 19/6/2026 17:21:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoExamen](
	[TipoExamenId] [int] IDENTITY(1,1) NOT NULL,
	[Secuencia] [tinyint] NULL,
	[Nombre] [varchar](25) NULL,
	[Descripción] [varchar](250) NULL,
 CONSTRAINT [PK_TipoExamen] PRIMARY KEY CLUSTERED 
(
	[TipoExamenId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CursadaAlumnoExamen]    Script Date: 19/6/2026 17:21:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CursadaAlumnoExamen](
	[CursadaAlumnoExamenId] [int] IDENTITY(1,1) NOT NULL,
	[ExamenId] [int] NULL,
	[CursadaAlumnoCarreraId] [int] NULL,
	[nota] [tinyint] NULL,
 CONSTRAINT [PK_CursadaAlumnoExamen] PRIMARY KEY CLUSTERED 
(
	[CursadaAlumnoExamenId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InstanciaExamenCursada]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InstanciaExamenCursada](
	[InstanciaExamenId] [int] IDENTITY(1,1) NOT NULL,
	[ExamenId] [int] NULL,
	[CursadaId] [int] NULL,
	[Fecha] [datetime] NULL,
	[Secuencia] [int] NULL,
	[CantidadAprobados] [int] NULL,
	[CantidadDesaprobados] [int] NULL,
	[CantidadAusente] [int] NULL,
	[Promedio] [int] NULL,
 CONSTRAINT [PK_InstanciaExamenCursada] PRIMARY KEY CLUSTERED 
(
	[InstanciaExamenId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Examen]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Examen](
	[ExamenId] [int] IDENTITY(1,1) NOT NULL,
	[Tipoid] [int] NULL,
	[Nombre] [varchar](50) NULL,
 CONSTRAINT [PK_Examen] PRIMARY KEY CLUSTERED 
(
	[ExamenId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[AlumnoMateriaCursoAnioCarrera]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[AlumnoMateriaCursoAnioCarrera]
AS
SELECT dbo.Alumnos.AlumnoId, dbo.Alumnos.Nombre, dbo.Alumnos.Apellido, dbo.Materias.MateriaId, dbo.Materias.Nombre AS Materia, dbo.CursadaAlumnoCarreras.Estado, dbo.Cursos.NombreCurso AS Curso, 
                  dbo.AniosCarreras.AnioCarrera AS Anio, dbo.AlumnosCarreras.AlumnoCarreraId, dbo.Carreras.CarreraId, dbo.Carreras.DescripcionCorta AS Carrera, dbo.Alumnos.Activo, dbo.AlumnosCarreras.Inicializado
FROM     dbo.TipoExamen INNER JOIN
                  dbo.Examen ON dbo.TipoExamen.TipoExamenId = dbo.Examen.Tipoid INNER JOIN
                  dbo.Alumnos INNER JOIN
                  dbo.AlumnosCarreras ON dbo.AlumnosCarreras.AlumnoId = dbo.Alumnos.AlumnoId INNER JOIN
                  dbo.Carreras ON dbo.Carreras.CarreraId = dbo.AlumnosCarreras.CarreraId INNER JOIN
                  dbo.CursadaAlumnoCarreras ON dbo.AlumnosCarreras.AlumnoCarreraId = dbo.CursadaAlumnoCarreras.AlumnoCarreraId INNER JOIN
                  dbo.Cursadas ON dbo.Cursadas.CursadaId = dbo.CursadaAlumnoCarreras.CursadaId INNER JOIN
                  dbo.CursoMaterias ON dbo.Cursadas.CursoMateriaId = dbo.CursoMaterias.CursoMateriaId INNER JOIN
                  dbo.Materias ON dbo.CursoMaterias.MateriaId = dbo.Materias.MateriaId INNER JOIN
                  dbo.Cursos ON dbo.CursoMaterias.CursoId = dbo.Cursos.CursoId INNER JOIN
                  dbo.AniosCarreras ON dbo.Cursos.AnioCarreraId = dbo.AniosCarreras.AnioCarreraId INNER JOIN
                  dbo.CursadaAlumnoExamen ON dbo.CursadaAlumnoCarreras.CursadaAlumnoCarreraId = dbo.CursadaAlumnoExamen.CursadaAlumnoCarreraId INNER JOIN
                  dbo.InstanciaExamenCursada ON dbo.Cursadas.CursadaId = dbo.InstanciaExamenCursada.CursadaId AND dbo.CursadaAlumnoExamen.ExamenId = dbo.InstanciaExamenCursada.InstanciaExamenId ON 
                  dbo.Examen.ExamenId = dbo.InstanciaExamenCursada.ExamenId
GO
/****** Object:  Table [dbo].[CicloLectivo]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CicloLectivo](
	[AnioLectivo] [int] NOT NULL,
	[CantidadSemana] [int] NOT NULL,
	[FechaInicio] [date] NULL,
	[FechaInscripcionInicio] [date] NULL,
	[FechaInscripcionFinal] [date] NULL,
	[FechaMarzoInicio] [date] NULL,
	[FechaMarzoFinal] [date] NULL,
	[FechaJunioInicio] [date] NULL,
	[FechaJunioFinal] [date] NULL,
	[FechaDiciembreInicio] [date] NULL,
	[FechaDiciembreFinal] [date] NULL,
	[FechaEspecialInicio] [date] NULL,
	[FechaEspecialFinal] [date] NULL,
	[FechaCierre] [date] NULL,
	[Activo]  AS ([dbo].[funCicloLectivoActivo]([FechaInicio],[FechaCierre])),
	[FechaPreInscripcionInicio] [date] NULL,
	[FechaPreInscripcionFinal] [date] NULL,
	[FechaInscripcionSuperioresInicio] [date] NULL,
	[FechaInscripcionSuperioresFinal] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[AnioLectivo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[MateriasCarrerasVigentes]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[MateriasCarrerasVigentes]
AS
SELECT Cursadas.AnioLectivo, Materias.MateriaId, Materias.Nombre AS Materia, Materias.AnioCarreraId, CAST(AniosCarreras.AnioCarrera AS varchar(10)) + Cursos.NombreCurso AS Anio,
Carreras.CarreraId, Carreras.DescripcionCorta AS Carrera, Cursos.CursoId
FROM Cursadas
INNER JOIN CicloLectivo ON CicloLectivo.AnioLectivo = Cursadas.AnioLectivo
INNER JOIN CursoMaterias ON CursoMaterias.CursoMateriaId = Cursadas.CursoMateriaId
INNER JOIN Cursos ON Cursos.CursoId = CursoMaterias.CursoId
INNER JOIN AniosCarreras ON AniosCarreras.AnioCarreraId = Cursos.AnioCarreraId
INNER JOIN Carreras ON Carreras.CarreraId = AniosCarreras.CarreraId
INNER JOIN Materias ON Materias.MateriaId = CursoMaterias.CursoMateriaId
WHERE Materias.Activo = 'True' AND Carreras.CarreraEstadoId = 1 AND CicloLectivo.Activo = 'True'
GO
/****** Object:  Table [dbo].[Cargos]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cargos](
	[CargoId] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](50) NOT NULL,
	[Activo] [bit] NULL,
	[TipoAsignacionId] [int] NULL,
	[TipoAplicacionId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CargoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Personal]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Personal](
	[PersonalId] [int] IDENTITY(1,1) NOT NULL,
	[NumeroDocumento] [varchar](10) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Apellido] [varchar](50) NOT NULL,
	[FechaNacimiento] [date] NULL,
	[Sexo] [char](1) NOT NULL,
	[Direccion] [varchar](250) NOT NULL,
	[Piso] [varchar](10) NULL,
	[Departamento] [varchar](10) NULL,
	[Localidad] [varchar](250) NOT NULL,
	[Celular] [varchar](50) NULL,
	[Telefono] [varchar](50) NULL,
	[Nacionalidad] [varchar](150) NOT NULL,
	[Email] [varchar](250) NOT NULL,
	[EstadoCivil] [varchar](50) NULL,
	[Foto] [varchar](250) NULL,
	[Titulo] [varchar](50) NULL,
	[TramoPedagogico] [bit] NULL,
	[FechaAlta] [date] NULL,
	[FechaBaja] [date] NULL,
	[PersonalEstadoId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[PersonalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Servicios]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Servicios](
	[ServicioId] [int] IDENTITY(1,1) NOT NULL,
	[PersonalId] [int] NULL,
	[SituacionRevistaId] [int] NULL,
	[CargoId] [int] NULL,
	[CursoMateriaId] [int] NULL,
	[FechaAlta] [date] NULL,
	[CantidadModulos] [int] NULL,
	[Causa] [varchar](max) NULL,
	[FechaBaja] [date] NULL,
	[Personal] [varchar](50) NULL,
	[Activo] [bit] NOT NULL,
	[LibroActaId] [int] NULL,
	[CarreraId] [int] NULL,
	[Modulo] [int] NULL,
	[Folio] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ServicioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[ViewCargosServiciosCursoMateria]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewCargosServiciosCursoMateria]
AS
SELECT CursoMaterias.CursoMateriaId, Nombre + ' ' + Apellido AS NombreCompleto,
Cargos.Descripcion AS Cargo, Servicios.Activo
FROM Personal
INNER JOIN Servicios ON Servicios.PersonalId = Personal.PersonalId
INNER JOIN CursoMaterias ON CursoMaterias.CursoMateriaId = Servicios.CursoMateriaId
INNER JOIN Cargos ON Cargos.CargoId = Servicios.CargoId
GO
/****** Object:  Table [dbo].[Licencias]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Licencias](
	[LicenciaId] [int] IDENTITY(1,1) NOT NULL,
	[PersonalId] [int] NULL,
	[TipoLicenciaId] [varchar](10) NULL,
	[FechaAlta] [date] NULL,
	[Certificado] [varchar](50) NULL,
	[FechaBaja] [date] NULL,
	[Activo]  AS ([dbo].[funLicenciasActivo]([FechaAlta],[FechaBaja])),
PRIMARY KEY CLUSTERED 
(
	[LicenciaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoLicencias]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoLicencias](
	[TipoLicenciaId] [varchar](10) NOT NULL,
	[Descripcion] [varchar](20) NULL,
	[Dias] [int] NULL,
	[FechaFinObligatoria] [bit] NOT NULL,
	[Activo] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TipoLicenciaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[viewLicencias]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[viewLicencias]
AS
SELECT LicenciaId, PersonalId, TipoLicencias.Descripcion AS Tipo, FechaAlta, FechaBaja, Certificado, Licencias.Activo 
FROM Licencias
INNER JOIN TipoLicencias ON TipoLicencias.TipoLicenciaId = Licencias.TipoLicenciaId
GO
/****** Object:  Table [dbo].[Asistencias]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Asistencias](
	[AsistenciaId] [int] IDENTITY(1,1) NOT NULL,
	[Fecha] [date] NOT NULL,
	[Asistencia] [char](1) NOT NULL,
	[CursadaAlumnoCarreraId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AsistenciaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Correlativas]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Correlativas](
	[CorrelativaId] [int] IDENTITY(1,1) NOT NULL,
	[MateriaId] [int] NOT NULL,
	[MateriaCorrelativaId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CorrelativaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dias]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dias](
	[DiaId] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[DiaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Equivalencias]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Equivalencias](
	[EquivalenciaId] [int] IDENTITY(1,1) NOT NULL,
	[CarreraId] [int] NULL,
	[MateriaId] [int] NULL,
	[CarreraEquivalenciaId] [int] NULL,
	[MateriaEquivalenciaId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[EquivalenciaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Espacios]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Espacios](
	[EspacioId] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[EspacioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Estados]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Estados](
	[EstadoId] [int] NOT NULL,
	[Descripcion] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[EstadoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExamenCursoMateria]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExamenCursoMateria](
	[ExamenId] [int] IDENTITY(1,1) NOT NULL,
	[CursoMateriaId] [int] NULL,
	[TipoExamenId] [int] NULL,
	[Fecha] [datetime] NULL,
 CONSTRAINT [PK_ExamnCursoMateria] PRIMARY KEY CLUSTERED 
(
	[ExamenId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Horarios]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Horarios](
	[HorarioId] [int] IDENTITY(1,1) NOT NULL,
	[DiaId] [int] NULL,
	[ModuloId] [int] NULL,
	[CursoMateriaId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[HorarioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LibroActas]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LibroActas](
	[LibroActaId] [int] IDENTITY(1,1) NOT NULL,
	[TipoLibroId] [int] NULL,
	[LibroNumero] [int] NULL,
	[FolioNumero] [int] NULL,
	[FolioMaximo] [int] NULL,
	[FechaAlta] [date] NULL,
	[FechaBaja] [date] NULL,
	[Activo] [bit] NULL,
	[CarreraID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[LibroActaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LicenciaServicios]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LicenciaServicios](
	[LicenciaServicioId] [int] IDENTITY(1,1) NOT NULL,
	[LicenciaId] [int] NULL,
	[ServicioId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[LicenciaServicioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Llamados]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Llamados](
	[LlamadoId] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[LlamadoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Localidades]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Localidades](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[descripcion] [nvarchar](150) NULL,
 CONSTRAINT [PK_Localidades] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Mensajes]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mensajes](
	[MensajeId] [int] IDENTITY(1,1) NOT NULL,
	[Mensaje] [text] NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[FechaHora] [datetime] NOT NULL,
	[Asunto] [varchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MensajeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MesasFinales]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MesasFinales](
	[MesaFinalId] [int] IDENTITY(1,1) NOT NULL,
	[CarreraId] [int] NULL,
	[Fecha] [date] NULL,
	[TurnoId] [int] NULL,
	[LlamadoId] [int] NULL,
	[MateriaId] [int] NULL,
	[PresidenteId] [int] NULL,
	[VocalId] [int] NULL,
	[CicloLectivoId] [int] NULL,
	[FinalEstadoId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MesaFinalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Modulos]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Modulos](
	[ModuloId] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[ModuloId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Parametros]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Parametros](
	[ParametroId] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Descripcion] [varchar](150) NULL,
	[Valor] [varchar](500) NOT NULL,
	[TipoId] [tinyint] NOT NULL,
	[Activo] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ParametroId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Parametros_Nombre] UNIQUE NONCLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Personas]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Personas](
	[PersonaId] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Apellido] [varchar](50) NOT NULL,
	[Documento] [varchar](50) NOT NULL,
	[FechaNacimiento] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PersonaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SituacionRevistas]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SituacionRevistas](
	[SituacionRevistaId] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SituacionRevistaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoAplicacion]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoAplicacion](
	[TipoAplicacionId] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](50) NOT NULL,
	[Detalle] [varchar](150) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TipoAplicacionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoAsignacion]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoAsignacion](
	[TipoAsignacionId] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[TipoAsignacionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoLibros]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoLibros](
	[TipoLibroId] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[TipoLibroId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoParametro]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoParametro](
	[TipoParametroId] [tinyint] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TipoParametroId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Turnos]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Turnos](
	[TurnoId] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TurnoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [I_AniosCarreras_AC_CodigoBloque]    Script Date: 19/6/2026 17:21:15 ******/
CREATE NONCLUSTERED INDEX [I_AniosCarreras_AC_CodigoBloque] ON [dbo].[AniosCarreras]
(
	[AniosCarrerasCodigoBloque] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [I_Carreras_C_CodigoBloque]    Script Date: 19/6/2026 17:21:15 ******/
CREATE NONCLUSTERED INDEX [I_Carreras_C_CodigoBloque] ON [dbo].[Carreras]
(
	[CarrerasCodigoBloque] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [I_Materias_MateriasCodigo]    Script Date: 19/6/2026 17:21:15 ******/
CREATE NONCLUSTERED INDEX [I_Materias_MateriasCodigo] ON [dbo].[Materias]
(
	[MateriasCodigoBloque] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AlumnosCarreras] ADD  CONSTRAINT [DF_Inicializado]  DEFAULT ('False') FOR [Inicializado]
GO
ALTER TABLE [dbo].[AniosCarreras] ADD  CONSTRAINT [df_CargaHoraria]  DEFAULT ((0)) FOR [CargaHorariaCompleta]
GO
ALTER TABLE [dbo].[Cargos] ADD  DEFAULT ((1)) FOR [TipoAplicacionId]
GO
ALTER TABLE [dbo].[Materias] ADD  DEFAULT ((0)) FOR [Modulos]
GO
ALTER TABLE [dbo].[Materias] ADD  DEFAULT ('F') FOR [FinalPromocion]
GO
ALTER TABLE [dbo].[TipoLicencias] ADD  DEFAULT ('True') FOR [Activo]
GO
ALTER TABLE [dbo].[AlumnosCarreras]  WITH CHECK ADD FOREIGN KEY([AlumnoId])
REFERENCES [dbo].[Alumnos] ([AlumnoId])
GO
ALTER TABLE [dbo].[AlumnosCarreras]  WITH CHECK ADD FOREIGN KEY([AlumnoId])
REFERENCES [dbo].[Alumnos] ([AlumnoId])
GO
ALTER TABLE [dbo].[AlumnosCarreras]  WITH CHECK ADD FOREIGN KEY([CarreraId])
REFERENCES [dbo].[Carreras] ([CarreraId])
GO
ALTER TABLE [dbo].[AlumnosCarreras]  WITH CHECK ADD FOREIGN KEY([CarreraId])
REFERENCES [dbo].[Carreras] ([CarreraId])
GO
ALTER TABLE [dbo].[AniosCarreras]  WITH CHECK ADD FOREIGN KEY([CarreraId])
REFERENCES [dbo].[Carreras] ([CarreraId])
GO
ALTER TABLE [dbo].[AniosCarreras]  WITH CHECK ADD FOREIGN KEY([CarreraId])
REFERENCES [dbo].[Carreras] ([CarreraId])
GO
ALTER TABLE [dbo].[Asistencias]  WITH CHECK ADD  CONSTRAINT [FKAsistenciaCursadaAlumnoCarrera] FOREIGN KEY([CursadaAlumnoCarreraId])
REFERENCES [dbo].[CursadaAlumnoCarreras] ([CursadaAlumnoCarreraId])
GO
ALTER TABLE [dbo].[Asistencias] CHECK CONSTRAINT [FKAsistenciaCursadaAlumnoCarrera]
GO
ALTER TABLE [dbo].[Cargos]  WITH CHECK ADD FOREIGN KEY([TipoAplicacionId])
REFERENCES [dbo].[TipoAplicacion] ([TipoAplicacionId])
GO
ALTER TABLE [dbo].[Cargos]  WITH CHECK ADD  CONSTRAINT [FKCargoTipoAsignacion] FOREIGN KEY([TipoAsignacionId])
REFERENCES [dbo].[TipoAsignacion] ([TipoAsignacionId])
GO
ALTER TABLE [dbo].[Cargos] CHECK CONSTRAINT [FKCargoTipoAsignacion]
GO
ALTER TABLE [dbo].[Carreras]  WITH CHECK ADD  CONSTRAINT [FK_Carreras_Estados] FOREIGN KEY([CarreraEstadoId])
REFERENCES [dbo].[Estados] ([EstadoId])
GO
ALTER TABLE [dbo].[Carreras] CHECK CONSTRAINT [FK_Carreras_Estados]
GO
ALTER TABLE [dbo].[Correlativas]  WITH CHECK ADD FOREIGN KEY([MateriaId])
REFERENCES [dbo].[Materias] ([MateriaId])
GO
ALTER TABLE [dbo].[Correlativas]  WITH CHECK ADD FOREIGN KEY([MateriaId])
REFERENCES [dbo].[Materias] ([MateriaId])
GO
ALTER TABLE [dbo].[Correlativas]  WITH CHECK ADD  CONSTRAINT [FKMateriaCorrelativaMateria] FOREIGN KEY([MateriaCorrelativaId])
REFERENCES [dbo].[Materias] ([MateriaId])
GO
ALTER TABLE [dbo].[Correlativas] CHECK CONSTRAINT [FKMateriaCorrelativaMateria]
GO
ALTER TABLE [dbo].[CursadaAlumnoCarreras]  WITH CHECK ADD FOREIGN KEY([AlumnoCarreraId])
REFERENCES [dbo].[AlumnosCarreras] ([AlumnoCarreraId])
GO
ALTER TABLE [dbo].[CursadaAlumnoCarreras]  WITH CHECK ADD FOREIGN KEY([AlumnoCarreraId])
REFERENCES [dbo].[AlumnosCarreras] ([AlumnoCarreraId])
GO
ALTER TABLE [dbo].[CursadaAlumnoCarreras]  WITH CHECK ADD FOREIGN KEY([CursadaId])
REFERENCES [dbo].[Cursadas] ([CursadaId])
GO
ALTER TABLE [dbo].[CursadaAlumnoCarreras]  WITH CHECK ADD FOREIGN KEY([CursadaId])
REFERENCES [dbo].[Cursadas] ([CursadaId])
GO
ALTER TABLE [dbo].[Cursadas]  WITH CHECK ADD FOREIGN KEY([CursoMateriaId])
REFERENCES [dbo].[CursoMaterias] ([CursoMateriaId])
GO
ALTER TABLE [dbo].[Cursadas]  WITH CHECK ADD FOREIGN KEY([CursoMateriaId])
REFERENCES [dbo].[CursoMaterias] ([CursoMateriaId])
GO
ALTER TABLE [dbo].[CursoMaterias]  WITH CHECK ADD FOREIGN KEY([CursoId])
REFERENCES [dbo].[Cursos] ([CursoId])
GO
ALTER TABLE [dbo].[CursoMaterias]  WITH CHECK ADD FOREIGN KEY([CursoId])
REFERENCES [dbo].[Cursos] ([CursoId])
GO
ALTER TABLE [dbo].[CursoMaterias]  WITH CHECK ADD FOREIGN KEY([MateriaId])
REFERENCES [dbo].[Materias] ([MateriaId])
GO
ALTER TABLE [dbo].[CursoMaterias]  WITH CHECK ADD FOREIGN KEY([MateriaId])
REFERENCES [dbo].[Materias] ([MateriaId])
GO
ALTER TABLE [dbo].[Cursos]  WITH CHECK ADD FOREIGN KEY([AnioCarreraId])
REFERENCES [dbo].[AniosCarreras] ([AnioCarreraId])
GO
ALTER TABLE [dbo].[Cursos]  WITH CHECK ADD FOREIGN KEY([AnioCarreraId])
REFERENCES [dbo].[AniosCarreras] ([AnioCarreraId])
GO
ALTER TABLE [dbo].[Equivalencias]  WITH CHECK ADD FOREIGN KEY([CarreraId])
REFERENCES [dbo].[Carreras] ([CarreraId])
GO
ALTER TABLE [dbo].[Equivalencias]  WITH CHECK ADD FOREIGN KEY([CarreraEquivalenciaId])
REFERENCES [dbo].[Carreras] ([CarreraId])
GO
ALTER TABLE [dbo].[Equivalencias]  WITH CHECK ADD FOREIGN KEY([MateriaId])
REFERENCES [dbo].[Materias] ([MateriaId])
GO
ALTER TABLE [dbo].[Equivalencias]  WITH CHECK ADD FOREIGN KEY([MateriaEquivalenciaId])
REFERENCES [dbo].[Materias] ([MateriaId])
GO
ALTER TABLE [dbo].[Horarios]  WITH CHECK ADD FOREIGN KEY([CursoMateriaId])
REFERENCES [dbo].[CursoMaterias] ([CursoMateriaId])
GO
ALTER TABLE [dbo].[Horarios]  WITH CHECK ADD FOREIGN KEY([CursoMateriaId])
REFERENCES [dbo].[CursoMaterias] ([CursoMateriaId])
GO
ALTER TABLE [dbo].[Horarios]  WITH CHECK ADD FOREIGN KEY([DiaId])
REFERENCES [dbo].[Dias] ([DiaId])
GO
ALTER TABLE [dbo].[Horarios]  WITH CHECK ADD FOREIGN KEY([DiaId])
REFERENCES [dbo].[Dias] ([DiaId])
GO
ALTER TABLE [dbo].[Horarios]  WITH CHECK ADD FOREIGN KEY([ModuloId])
REFERENCES [dbo].[Modulos] ([ModuloId])
GO
ALTER TABLE [dbo].[Horarios]  WITH CHECK ADD FOREIGN KEY([ModuloId])
REFERENCES [dbo].[Modulos] ([ModuloId])
GO
ALTER TABLE [dbo].[LibroActas]  WITH CHECK ADD FOREIGN KEY([TipoLibroId])
REFERENCES [dbo].[TipoLibros] ([TipoLibroId])
GO
ALTER TABLE [dbo].[LibroActas]  WITH CHECK ADD FOREIGN KEY([TipoLibroId])
REFERENCES [dbo].[TipoLibros] ([TipoLibroId])
GO
ALTER TABLE [dbo].[LibroActas]  WITH CHECK ADD  CONSTRAINT [FK_LibroActas_Carrera] FOREIGN KEY([CarreraID])
REFERENCES [dbo].[Carreras] ([CarreraId])
GO
ALTER TABLE [dbo].[LibroActas] CHECK CONSTRAINT [FK_LibroActas_Carrera]
GO
ALTER TABLE [dbo].[Licencias]  WITH CHECK ADD FOREIGN KEY([PersonalId])
REFERENCES [dbo].[Personal] ([PersonalId])
GO
ALTER TABLE [dbo].[Licencias]  WITH CHECK ADD FOREIGN KEY([PersonalId])
REFERENCES [dbo].[Personal] ([PersonalId])
GO
ALTER TABLE [dbo].[Licencias]  WITH CHECK ADD  CONSTRAINT [FK__Licencias__TipoL] FOREIGN KEY([TipoLicenciaId])
REFERENCES [dbo].[TipoLicencias] ([TipoLicenciaId])
GO
ALTER TABLE [dbo].[Licencias] CHECK CONSTRAINT [FK__Licencias__TipoL]
GO
ALTER TABLE [dbo].[Licencias]  WITH CHECK ADD FOREIGN KEY([TipoLicenciaId])
REFERENCES [dbo].[TipoLicencias] ([TipoLicenciaId])
GO
ALTER TABLE [dbo].[LicenciaServicios]  WITH CHECK ADD FOREIGN KEY([LicenciaId])
REFERENCES [dbo].[Licencias] ([LicenciaId])
GO
ALTER TABLE [dbo].[LicenciaServicios]  WITH CHECK ADD FOREIGN KEY([LicenciaId])
REFERENCES [dbo].[Licencias] ([LicenciaId])
GO
ALTER TABLE [dbo].[LicenciaServicios]  WITH CHECK ADD FOREIGN KEY([ServicioId])
REFERENCES [dbo].[Servicios] ([ServicioId])
GO
ALTER TABLE [dbo].[LicenciaServicios]  WITH CHECK ADD FOREIGN KEY([ServicioId])
REFERENCES [dbo].[Servicios] ([ServicioId])
GO
ALTER TABLE [dbo].[Materias]  WITH CHECK ADD FOREIGN KEY([AnioCarreraId])
REFERENCES [dbo].[AniosCarreras] ([AnioCarreraId])
GO
ALTER TABLE [dbo].[Materias]  WITH CHECK ADD FOREIGN KEY([AnioCarreraId])
REFERENCES [dbo].[AniosCarreras] ([AnioCarreraId])
GO
ALTER TABLE [dbo].[Materias]  WITH CHECK ADD FOREIGN KEY([EspacioId])
REFERENCES [dbo].[Espacios] ([EspacioId])
GO
ALTER TABLE [dbo].[Materias]  WITH CHECK ADD FOREIGN KEY([EspacioId])
REFERENCES [dbo].[Espacios] ([EspacioId])
GO
ALTER TABLE [dbo].[Materias]  WITH CHECK ADD  CONSTRAINT [FK_Materias_Carreras] FOREIGN KEY([CarreraId])
REFERENCES [dbo].[Carreras] ([CarreraId])
GO
ALTER TABLE [dbo].[Materias] CHECK CONSTRAINT [FK_Materias_Carreras]
GO
ALTER TABLE [dbo].[MesasFinales]  WITH CHECK ADD FOREIGN KEY([CarreraId])
REFERENCES [dbo].[Carreras] ([CarreraId])
GO
ALTER TABLE [dbo].[MesasFinales]  WITH CHECK ADD FOREIGN KEY([CicloLectivoId])
REFERENCES [dbo].[CicloLectivo] ([AnioLectivo])
GO
ALTER TABLE [dbo].[MesasFinales]  WITH CHECK ADD FOREIGN KEY([LlamadoId])
REFERENCES [dbo].[Llamados] ([LlamadoId])
GO
ALTER TABLE [dbo].[MesasFinales]  WITH CHECK ADD FOREIGN KEY([MateriaId])
REFERENCES [dbo].[Materias] ([MateriaId])
GO
ALTER TABLE [dbo].[MesasFinales]  WITH CHECK ADD FOREIGN KEY([PresidenteId])
REFERENCES [dbo].[Personal] ([PersonalId])
GO
ALTER TABLE [dbo].[MesasFinales]  WITH CHECK ADD FOREIGN KEY([TurnoId])
REFERENCES [dbo].[Turnos] ([TurnoId])
GO
ALTER TABLE [dbo].[MesasFinales]  WITH CHECK ADD FOREIGN KEY([VocalId])
REFERENCES [dbo].[Personal] ([PersonalId])
GO
ALTER TABLE [dbo].[MesasFinales]  WITH CHECK ADD  CONSTRAINT [FK_MesasFinales_Estados] FOREIGN KEY([FinalEstadoId])
REFERENCES [dbo].[Estados] ([EstadoId])
GO
ALTER TABLE [dbo].[MesasFinales] CHECK CONSTRAINT [FK_MesasFinales_Estados]
GO
ALTER TABLE [dbo].[Parametros]  WITH CHECK ADD FOREIGN KEY([TipoId])
REFERENCES [dbo].[TipoParametro] ([TipoParametroId])
GO
ALTER TABLE [dbo].[Parametros]  WITH CHECK ADD FOREIGN KEY([TipoId])
REFERENCES [dbo].[TipoParametro] ([TipoParametroId])
GO
ALTER TABLE [dbo].[Personal]  WITH CHECK ADD  CONSTRAINT [FK_Personal_Estados] FOREIGN KEY([PersonalEstadoId])
REFERENCES [dbo].[Estados] ([EstadoId])
GO
ALTER TABLE [dbo].[Personal] CHECK CONSTRAINT [FK_Personal_Estados]
GO
ALTER TABLE [dbo].[Servicios]  WITH CHECK ADD FOREIGN KEY([CargoId])
REFERENCES [dbo].[Cargos] ([CargoId])
GO
ALTER TABLE [dbo].[Servicios]  WITH CHECK ADD FOREIGN KEY([CargoId])
REFERENCES [dbo].[Cargos] ([CargoId])
GO
ALTER TABLE [dbo].[Servicios]  WITH CHECK ADD FOREIGN KEY([CursoMateriaId])
REFERENCES [dbo].[CursoMaterias] ([CursoMateriaId])
GO
ALTER TABLE [dbo].[Servicios]  WITH CHECK ADD FOREIGN KEY([CursoMateriaId])
REFERENCES [dbo].[CursoMaterias] ([CursoMateriaId])
GO
ALTER TABLE [dbo].[Servicios]  WITH CHECK ADD FOREIGN KEY([LibroActaId])
REFERENCES [dbo].[LibroActas] ([LibroActaId])
GO
ALTER TABLE [dbo].[Servicios]  WITH CHECK ADD FOREIGN KEY([LibroActaId])
REFERENCES [dbo].[LibroActas] ([LibroActaId])
GO
ALTER TABLE [dbo].[Servicios]  WITH CHECK ADD FOREIGN KEY([PersonalId])
REFERENCES [dbo].[Personal] ([PersonalId])
GO
ALTER TABLE [dbo].[Servicios]  WITH CHECK ADD FOREIGN KEY([PersonalId])
REFERENCES [dbo].[Personal] ([PersonalId])
GO
ALTER TABLE [dbo].[Servicios]  WITH CHECK ADD FOREIGN KEY([SituacionRevistaId])
REFERENCES [dbo].[SituacionRevistas] ([SituacionRevistaId])
GO
ALTER TABLE [dbo].[Servicios]  WITH CHECK ADD FOREIGN KEY([SituacionRevistaId])
REFERENCES [dbo].[SituacionRevistas] ([SituacionRevistaId])
GO
ALTER TABLE [dbo].[Servicios]  WITH CHECK ADD  CONSTRAINT [FKServicioCarrera] FOREIGN KEY([CarreraId])
REFERENCES [dbo].[Carreras] ([CarreraId])
GO
ALTER TABLE [dbo].[Servicios] CHECK CONSTRAINT [FKServicioCarrera]
GO
ALTER TABLE [dbo].[LibroActas]  WITH CHECK ADD  CONSTRAINT [LimitarFolios] CHECK  (([FolioNumero]<=[FolioMaximo]))
GO
ALTER TABLE [dbo].[LibroActas] CHECK CONSTRAINT [LimitarFolios]
GO
ALTER TABLE [dbo].[Materias]  WITH CHECK ADD  CONSTRAINT [CK_Materias_FinalPromocion] CHECK  (([FinalPromocion]='P' OR [FinalPromocion]='F'))
GO
ALTER TABLE [dbo].[Materias] CHECK CONSTRAINT [CK_Materias_FinalPromocion]
GO
/****** Object:  StoredProcedure [dbo].[AgregarNumeroLibro]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AgregarNumeroLibro]
    @TipoLibroID INT,
    @LibroNumero INT,
	@CarreraID INT,
	@FolioMaximo INT,
	@FechaAlta DATE
AS
BEGIN
    INSERT INTO LibroActas 
	(TipoLibroId, LibroNumero, CarreraID, FolioNumero, FolioMaximo, FechaAlta, Activo) 
	VALUES 
	(@TipoLibroID, @LibroNumero, @CarreraID, 0, @FolioMaximo, @FechaAlta, 1)
END
GO
/****** Object:  StoredProcedure [dbo].[ListaLocalidades]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[ListaLocalidades]
AS
BEGIN
create table #localidades ( LOCALIDAD varchar(50) );

INSERT INTO #localidades
SELECT  Localidad AS LOCALIDAD FROM Alumnos
UNION
SELECT  LocalidadNacimiento AS LOCALIDAD FROM Alumnos

select DISTINCT LOCALIDAD FROM #localidades

DROP TABLE #localidades
END
GO
/****** Object:  StoredProcedure [dbo].[SP_ActualizarPorcentajeAsistencia]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE procedure [dbo].[SP_ActualizarPorcentajeAsistencia](@HorascursadasCa int, @PorcentajeAsistenciaCa decimal, @UltimoPresentismoCa date, @FechaAsistencia date, 
  @HoraCatedraC int, @PorcentajeAsistenciaC decimal, @CursadaId int, @AlumnoCarreraId int, @Asistencia char, @CursadaAlumnoCarreraId int)
 as 
 begin
 update CursadaAlumnoCarreras set PorcentajeAsistencia = @PorcentajeAsistenciaCa, UltimoPresentismo = @UltimoPresentismoCa, HorasCursadas = @HorascursadasCa
 where AlumnoCarreraId = @AlumnoCarreraId;
 update Cursadas set HoraCatedra = @HoraCatedraC, PorcentajeAsistencia = @PorcentajeAsistenciaC, FechaAsistencia = @FechaAsistencia
 where CursadaId = @CursadaId;
 update Asistencias set Asistencia = @Asistencia where CursadaAlumnoCarreraId = @CursadaAlumnoCarreraId;
 end
GO
/****** Object:  StoredProcedure [dbo].[SP_CargaCursadas]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SP_CargaCursadas] (@AnioLectivoId INT)
AS
CREATE TABLE #CursoMaterias(CursoMateriaId INT);
CREATE TABLE #Cursadas(CursoMateriaId INT);

INSERT INTO #Cursadas
SELECT c.CursoMateriaId FROM Cursadas c
WHERE c.AnioLectivo = @AnioLectivoId;

INSERT INTO #CursoMaterias
SELECT cm.CursoMateriaId FROM CursoMaterias cm
INNER JOIN Cursos cur ON cm.CursoId = cur.CursoId
INNER JOIN AniosCarreras ac ON cur.AnioCarreraId = ac.AnioCarreraId
INNER JOIN Carreras car ON ac.CarreraId = car.CarreraId
WHERE 
	car.CarreraEstadoId = 1 AND 
	cur.Activo = 1 AND
	cm.CursoMateriaId NOT IN (SELECT CursoMateriaId FROM #Cursadas);

INSERT INTO Cursadas (CursoMateriaId, AnioLectivo)
SELECT CursoMateriaId, @AnioLectivoId FROM #CursoMaterias;

DROP TABLE #CursoMaterias
GO
/****** Object:  StoredProcedure [dbo].[SP_CargosLibres]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_CargosLibres]
@PersonalId int
AS
BEGIN

	--Carga los Cargos Unicos
	Create table #Unico(
	CargoId int
	)
	INSERT INTO #Unico 
	SELECT CargoId FROM Cargos WHERE TipoAplicacionId = 1

	--Carga los Cargos Unicos y Repetibles
	Create table #UnicoRepetible(
	CargoId int
	)
	INSERT INTO #UnicoRepetible 
	SELECT CargoId FROM Cargos WHERE TipoAplicacionId = 2

	--Carga los Cargos Repetibles
	Create table #Repetible(
	CargoId int
	)
	INSERT INTO #Repetible 
	SELECT CargoId FROM Cargos WHERE TipoAplicacionId = 3

	--Carga los Cargos Exclusivos y Repetibles
	Create table #EsclusivoRepetible(
	CargoId int
	)
	INSERT INTO #EsclusivoRepetible 
	SELECT CargoId FROM Cargos WHERE TipoAplicacionId = 4

	--Cargos que dispone la persona
	Create Table #CargosTomados(
	CargoId int
	)
	INSERT INTO #CargosTomados
	SELECT CargoId From Servicios WHERE PersonalId = @PersonalId AND Activo = 'true';

	Create Table #CargosDisponibles(
	CargoId int
	)

	--Carga los Cargos que no sean unicos
	INSERT INTO #CargosDisponibles 
	SELECT CargoId FROM Cargos 
	WHERE CargoId NOT IN (SELECT CargoId FROM Servicios WHERE CargoId IN (SELECT CargoId FROM #Unico) AND Activo = 'True') 
	AND Activo = 'True'

	IF EXISTS (SELECT CargoId From #CargosTomados WHERE CargoId IN (SELECT CargoId FROM #Unico) OR CargoId IN (SELECT CargoId FROM #UnicoRepetible))
		BEGIN
			DELETE FROM #CargosDisponibles
		END
	ELSE IF EXISTS (SELECT CargoId From #CargosTomados WHERE CargoId IN (SELECT CargoId FROM #EsclusivoRepetible))
		BEGIN
			DELETE FROM #CargosDisponibles WHERE CargoId IN (SELECT CargoId From #CargosTomados WHERE CargoId IN (SELECT CargoId FROM #EsclusivoRepetible))				
		END

	SELECT * From Cargos WHERE CargoId IN (SELECT CargoId FROM #CargosDisponibles)

	drop table #Unico
	drop table #UnicoRepetible
	drop table #CargosDisponibles
	drop table #CargosTomados
	drop table #EsclusivoRepetible
	drop table #Repetible
END

GO
/****** Object:  StoredProcedure [dbo].[SP_CarrerasDisponibles]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_CarrerasDisponibles](
@TipoAsignacionId as int,
@SituacionRevistaId as int
)
AS
BEGIN 
	IF(@TipoAsignacionId = 3 AND (@SituacionRevistaId = 2 OR @SituacionRevistaId = 1))
		BEGIN		
			SELECT CarreraId, DescripcionCorta FROM Carreras WHERE Activo= 'True' AND CarreraId NOT IN (SELECT CarreraId From Servicios WHERE Activo = 'True')			
		END
	ELSE IF(@TipoAsignacionId = 3 AND @SituacionRevistaId = 3)
		BEGIN
			SELECT CarreraId, DescripcionCorta FROM Carreras WHERE Activo= 'True' AND CarreraId IN (SELECT CarreraId From Servicios WHERE Activo = 'True')
			AND (CarreraId IN (SELECT CarreraId FROM Servicios se INNER JOIN LicenciaServicios ls ON se.ServicioId = ls.ServicioId
			INNER JOIN Licencias li ON li.LicenciaId = ls.LicenciaId WHERE li.FechaBaja IS NULL))
		END
	ELSE
		BEGIN
			SELECT * FROM Carreras WHERE  Activo= 'True'
		END
END
GO
/****** Object:  StoredProcedure [dbo].[SP_CrearMesasFinales]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_CrearMesasFinales]
    @anioLectivo INT,
    @turnoId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Llamado INT = 0;
    DECLARE @LlamadoMax INT;

    SELECT @LlamadoMax = MAX(LlamadoId) 
    FROM MesasFinales 
    WHERE CicloLectivoId = @anioLectivo;

    IF @LlamadoMax IS NULL
        SET @LlamadoMax = 2;

    IF NOT EXISTS (
        SELECT 1 FROM MesasFinales 
        WHERE CicloLectivoId = @anioLectivo AND TurnoId = @turnoId
    )
    BEGIN
        IF @turnoId != 3
        BEGIN
            INSERT INTO MesasFinales (CarreraId, TurnoId, LlamadoId, MateriaId, CicloLectivoId, FinalEstadoId) 
            SELECT C.CarreraId, @turnoId, 3, M.MateriaId, @anioLectivo, 3 
            FROM Materias M 
            INNER JOIN AniosCarreras AC ON M.AnioCarreraId = AC.AnioCarreraId
            INNER JOIN Carreras C ON AC.CarreraId = C.CarreraId;
        END
        ELSE
        BEGIN
            WHILE (@Llamado < @LlamadoMax)
            BEGIN
                IF @Llamado = 0 OR @Llamado = 2
                    SET @Llamado = 1;
                ELSE
                    SET @Llamado = @Llamado + 1;

                INSERT INTO MesasFinales (CarreraId, TurnoId, LlamadoId, MateriaId, CicloLectivoId, FinalEstadoId) 
                SELECT C.CarreraId, @turnoId, @Llamado, M.MateriaId, @anioLectivo, 3 
                FROM Materias M 
                INNER JOIN AniosCarreras AC ON M.AnioCarreraId = AC.AnioCarreraId
                INNER JOIN Carreras C ON AC.CarreraId = C.CarreraId;

                IF @Llamado = 2
                    BREAK;
            END
        END
    END
    ELSE
    BEGIN
        -- RAISERROR para SQL Server 2008 en lugar de THROW
        RAISERROR ('Ya hay mesas finales para dicho Turno y Ciclo Lectivo', 16, 1);
        RETURN;
    END
END
GO
/****** Object:  StoredProcedure [dbo].[SP_CursoMateriasLibres]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_CursoMateriasLibres](
@CursoId as int,
@TipoAsignacionId as int,
@SituacionRevistaId as int
)
AS
BEGIN 
	IF(@TipoAsignacionId = 2 AND (@SituacionRevistaId = 2 OR @SituacionRevistaId = 1))
		BEGIN		
			SELECT cm.CursoMateriaId, ma.Nombre FROM CursoMaterias cm FULL JOIN materias ma ON cm.MateriaId=ma.MateriaId 
			WHERE cursoid=@CursoId AND cm.Activo= 'True' AND (cm.CursoMateriaId NOT IN (SELECT CursoMateriaId FROM Servicios WHERE Activo = 'True') 
			OR cm.CursoMateriaId IN (SELECT CursoMateriaId FROM CursoMaterias AS CM INNER JOIN Materias AS MA ON CM.MateriaId = MA.MateriaId
			WHERE MA.Multiple = 'True'))			
		END
	ELSE IF(@TipoAsignacionId = 2 AND @SituacionRevistaId = 3)
		BEGIN
			SELECT cm.CursoMateriaId, ma.Nombre FROM CursoMaterias cm FULL JOIN materias ma ON cm.MateriaId=ma.MateriaId 
			WHERE cursoid=@CursoId AND cm.Activo= 'True' AND (cm.CursoMateriaId IN (SELECT CursoMateriaId FROM Servicios WHERE Activo = 'True')
			AND (cm.CursoMateriaId IN (SELECT se.CursoMateriaId FROM Servicios se INNER JOIN LicenciaServicios ls ON se.ServicioId = ls.ServicioId
			INNER JOIN Licencias li ON li.LicenciaId = ls.LicenciaId WHERE li.FechaBaja IS NULL)))
		END
END
GO
/****** Object:  StoredProcedure [dbo].[SP_CursoMateriasLibres2]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_CursoMateriasLibres2](
@CursoId as int,
@TipoAsignacionId as int,
@SituacionRevistaId as int
)
AS
BEGIN 
	IF(@TipoAsignacionId = 2 AND (@SituacionRevistaId = 2 OR @SituacionRevistaId = 1))
		BEGIN		
			SELECT cm.CursoMateriaId, ma.Nombre FROM CursoMaterias cm FULL JOIN materias ma ON cm.MateriaId=ma.MateriaId 
			WHERE cursoid=@CursoId AND cm.Activo= 'True' AND (cm.CursoMateriaId NOT IN (SELECT CursoMateriaId FROM Servicios WHERE Activo = 'True') 
			OR cm.CursoMateriaId IN (SELECT CursoMateriaId FROM CursoMaterias AS CM INNER JOIN Materias AS MA ON CM.MateriaId = MA.MateriaId
			WHERE MA.Multiple = 'True'))			
		END
	ELSE IF(@TipoAsignacionId = 2 AND @SituacionRevistaId = 3)
		BEGIN
			SELECT cm.CursoMateriaId, ma.Nombre FROM CursoMaterias cm FULL JOIN materias ma ON cm.MateriaId=ma.MateriaId 
			WHERE cursoid=@CursoId AND cm.Activo= 'True' AND (cm.CursoMateriaId IN (SELECT CursoMateriaId FROM Servicios WHERE Activo = 'True')
			AND (cm.CursoMateriaId IN (SELECT se.CursoMateriaId FROM Servicios se INNER JOIN LicenciaServicios ls ON se.ServicioId = ls.ServicioId
			INNER JOIN Licencias li ON li.LicenciaId = ls.LicenciaId WHERE li.FechaBaja IS NULL)))
		END
END
GO
/****** Object:  StoredProcedure [dbo].[SP_ExistenFechasFinales]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SP_ExistenFechasFinales] (@anioLectivo int)
as
begin
declare @fecha1 date, @fecha2 date, @fecha3 date, @fecha4 date, @fecha5 date, @fecha6 date, @resultadoMarzo bit, @resultadoJulio bit, @resultadoDiciembre bit
set @fecha1 = (select FechaMarzoInicio from CicloLectivo where AnioLectivo = @anioLectivo)
set @fecha2 = (select FechaMarzoFinal from CicloLectivo where AnioLectivo = @anioLectivo)
set @fecha3 = (select FechaJunioInicio from CicloLectivo where AnioLectivo = @anioLectivo)
set @fecha4 = (select FechaJunioFinal from CicloLectivo where AnioLectivo = @anioLectivo)
set @fecha5 = (select FechaDiciembreInicio from CicloLectivo where AnioLectivo = @anioLectivo)
set @fecha6 = (select FechaDiciembreFinal from CicloLectivo where AnioLectivo = @anioLectivo)
begin
if (@fecha1 is not null and @fecha2 is not null)
set @resultadoMarzo = 1
else
set @resultadoMarzo = 0
end
begin
if (@fecha3 is not null and @fecha4 is not null)
set @resultadoJulio = 1
else
set @resultadoJulio = 0
end
begin
if (@fecha5 is not null and @fecha6 is not null)
set @resultadoDiciembre = 1
else
set @resultadoDiciembre = 0
end
select @resultadoMarzo as 'Marzo', @resultadoJulio as 'Julio', @resultadoDiciembre as 'Diciembre'
end
GO
/****** Object:  StoredProcedure [dbo].[SP_IngresoCursadaPrimero]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE  [dbo].[SP_IngresoCursadaPrimero] (@AnioLectivoId int)
 AS
 BEGIN   

-- Tabla para almacenar la cantidad de cursos por carrera
CREATE TABLE #CursosTotales
(
	CarreraId INT,
	Cursos INT
);

-- Tabla que contiene los cursos
-- y define un código dependiendo de la cantidad de cursos por carrera
CREATE TABLE #CursosPrimero
(
	CarreraId INT,
	CursoId INT,
	Curso VARCHAR(10),
	Codigo INT
);

-- Tabla que contiene los alumnos no inicializado 
-- y define un código dependiendo de la cantidad de cursos por carrera
CREATE TABlE #AlumnosNoInicializados
(
	Codigo INT,
	AlumnoCarreraId INT,
	CarreraId INT
);

-- Tabla para almacenar los alumnos que ya están inscriptos
-- y así avitar duplicidad
CREATE TABLE #AlumnosInscriptos
(
	AlumnoCarreraId INT,
);

-- Busco los cuantos cursos de primero existe por carrera
INSERT INTO #CursosTotales
SELECT CarreraId, COUNT(CursoId) Cursos
FROM Cursos c
INNER JOIN AniosCarreras ac ON c.AnioCarreraId = ac.AnioCarreraId AND ac.AnioCarrera = 1
GROUP BY ac.CarreraId;

-- Le asigno un codigo al curso dependiendo la cantidad de cursos de primero de la carrera
INSERT INTO #CursosPrimero
SELECT
  ac.CarreraId,
  c.CursoId,
  c.NombreCurso AS Curso,
  CASE 
    WHEN (ROW_NUMBER() OVER (ORDER BY c.CursoId ASC) % #CursosTotales.Cursos) = 0 
      THEN #CursosTotales.Cursos
    ELSE (ROW_NUMBER() OVER (ORDER BY c.CursoId ASC) % #CursosTotales.Cursos)
  END AS Codigo
FROM Cursos c
INNER JOIN AniosCarreras ac ON c.AnioCarreraId = ac.AnioCarreraId AND ac.AnioCarrera = 1
INNER JOIN #CursosTotales ON ac.CarreraId = #CursosTotales.CarreraId;

-- Le asigno un codigo al alumno dependiendo la cantidad de cursos de primero de la carrera
INSERT INTO #AlumnosNoInicializados
SELECT
  CASE 
    WHEN (ROW_NUMBER() OVER(ORDER BY AlumnoCarreraId ASC) % #CursosTotales.Cursos) = 0
      THEN #CursosTotales.Cursos
    ELSE (ROW_NUMBER() OVER(ORDER BY AlumnoCarreraId ASC) % #CursosTotales.Cursos)
  END AS Codigo,
  AlumnoCarreraId,
  alc.CarreraId
FROM AlumnosCarreras alc
INNER JOIN #CursosTotales ON alc.CarreraId = #CursosTotales.CarreraId
WHERE alc.Inicializado = 0;

INSERT INTO #AlumnosInscriptos
SELECT car.AlumnoCarreraId FROM CursadaAlumnoCarreras car
INNER JOIN #AlumnosNoInicializados ani ON car.AlumnoCarreraId = ani.AlumnoCarreraId;

-- Se distribuye los alumnos en los distintos cursos de 1ro 
-- respetando el código asignado al alumno y al curso
INSERT INTO CursadaAlumnoCarreras
SELECT
	ani.AlumnoCarreraId,
	curs.CursadaId,
	@AnioLectivoId,
	'CU',
	0,
	NULL,
	0,
	NULL,
	1
FROM #AlumnosNoInicializados ani
INNER JOIN Carreras c ON ani.CarreraId = c.CarreraId
INNER JOIN AniosCarreras anc ON c.CarreraId = anc.CarreraId AND anc.AnioCarrera = 1
INNER JOIN #CursosPrimero cp ON ani.Codigo = cp.Codigo
INNER JOIN CursoMaterias cuma ON cp.CursoId = cuma.CursoId
INNER JOIN Materias m ON cuma.MateriaId = m.MateriaId
INNER JOIN Cursadas curs ON cuma.CursoMateriaId = curs.CursoMateriaId AND curs.AnioLectivo = 2022
WHERE ani.AlumnoCarreraId NOT IN (SELECT AlumnoCarreraId FROM #AlumnosInscriptos);

-- Establezco que los alumnos ya se encuentran inicializados
UPDATE AlumnosCarreras SET Inicializado = 1
WHERE AlumnoCarreraId IN (SELECT AlumnoCarreraId FROM #AlumnosNoInicializados);

-- Elimino todas las tablas temporales
DROP TABLE #AlumnosNoInicializados;
DROP TABLE #AlumnosInscriptos;
DROP TABLE #CursosPrimero;
DROP TABLE #CursosTotales;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_InscripcionAlumnos]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_InscripcionAlumnos]  ( @AnioLectivoId As int)
 AS
 BEGIN

 -- Tabla que almacena alumnos no inicializados
 CREATE TABLE #AlumnosNoInicializados
 (
	CarreraId INT,
	Alumnos INT
 );

 -- Tabla que almacena alumnos ya inscriptos
CREATE TABLE #AlumnosInscriptos
(
	CursoId INT,
	Alumnos INT
);

 -- Tabla que almacena cantidad de materias por curso
CREATE TABLE #MateriasTotales
(
	CursoId INT,
	Materias INT
);

 -- Obtiene los alumnos no inicializados
 INSERT INTO #AlumnosNoInicializados
 SELECT CarreraId, COUNT(AlumnoCarreraId) AS Alumnos FROM AlumnosCarreras
 WHERE Inicializado = 0
 GROUP BY CarreraId

 -- Obtiene la cantidad de materias por curso
INSERT INTO #MateriasTotales
SELECT CursoId, COUNT(MateriaId) AS Materias FROM CursoMaterias
GROUP BY CursoId;

 -- Obtiene la distribución de alumnos por curso
 SELECT 
 c.DescripcionCorta As Carrera,
 ani.Alumnos As [Alumnos Nuevos],
 CAST(ac.AnioCarrera AS varchar(10)) + ISNULL(cu.NombreCurso, '') AS Curso,
 COUNT(car.AlumnoCarreraId) / mt.Materias AS [Alumnos Inscriptos]
 FROM Cursadas cur
 INNER JOIN CursoMaterias cuma ON cur.CursoMateriaId = cuma.CursoMateriaId
 INNER JOIN #MateriasTotales mt ON cuma.CursoId = mt.CursoId
 INNER JOIN Cursos cu ON cuma.CursoId = cu.CursoId AND cu.Activo = 1
 INNER JOIN AniosCarreras ac ON cu.AnioCarreraId = ac.AnioCarreraId
 INNER JOIN Carreras c ON ac.CarreraId = c.CarreraId AND c.CarreraEstadoId = 1
 LEFT JOIN #AlumnosNoInicializados ani ON c.CarreraId = ani.CarreraId
 LEFT JOIN CursadaAlumnoCarreras car ON cur.CursadaId = car.CursadaId
 GROUP BY cu.NombreCurso, ac.AnioCarrera, c.DescripcionCorta, ani.Alumnos, mt.Materias
 ORDER BY ac.AnioCarrera
 END
GO
/****** Object:  StoredProcedure [dbo].[sp_insertCursosMaterias]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_insertCursosMaterias]
	@NombreCurso varchar(10),
	@AnioCarreraId int
AS
	INSERT INTO Cursos(NombreCurso, AnioCarreraId, Activo,AdmiteCurso)
	VALUES(@NombreCurso,@AnioCarreraId,1,1)
	
	DECLARE @CursoId int
    select @CursoId = @@IDENTITY
	DECLARE @fecha date
	set @fecha = GETDATE()

	INSERT INTO CursoMaterias(MateriaId, CursoId,FechaAlta,Activo)
	SELECT MateriaId, @CursoId,@fecha,1 from Materias where AnioCarreraId=@AnioCarreraId
GO
/****** Object:  StoredProcedure [dbo].[SP_libroActas]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[SP_libroActas]
@LibroTipoId int
AS
BEGIN

	IF(SELECT count(*) FROM LibroActas WHERE TipoLibroId=@LibroTipoId)=0
	BEGIN 
		INSERT INTO LibroActas(TipoLibroId, LibroNumero, FolioNumero, FolioMaximo,FechaAlta,Activo) VALUES(@LibroTipoId,1,1,60,GETDATE(),1) 
	END
	ELSE
	BEGIN 
		DECLARE @LibroNumero INT
		DECLARE @FolioNumero INT
		
		SELECT @LibroNumero=MAX(LibroNumero), @FolioNumero=MAX(FolioNumero) FROM LibroActas WHERE TipoLibroId=@LibroTipoId AND LibroNumero=(SELECT MAX(LibroNumero) FROM LibroActas WHERE TipoLibroId=@LibroTipoId)
		IF (@FolioNumero<60)
		BEGIN 
			INSERT INTO LibroActas(TipoLibroId, LibroNumero, FolioNumero, FolioMaximo,FechaAlta,Activo) VALUES(@LibroTipoId,@LibroNumero,(@FolioNumero + 1),60,GETDATE(),1)
		END
		ELSE
		BEGIN
			UPDATE LibroActas SET FechaBaja=GETDATE(), Activo=0 WHERE LibroNumero=@LibroNumero
			INSERT INTO LibroActas(TipoLibroId, LibroNumero, FolioNumero, FolioMaximo,FechaAlta,Activo) VALUES(@LibroTipoId,(@LibroNumero + 1),1,60,GETDATE(),1)
		END
	END
END
 
 
GO
/****** Object:  StoredProcedure [dbo].[SP_ListaMaterias]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_ListaMaterias](
@MateriaID as int,
@CarreraID as int,
@msj as varchar(50) output
)
AS
BEGIN
BEGIN TRY
   -- Variable interna
	Declare @AnioCarrera As int

	create table #Temporal (
	MateriaCorrelativaId int)

	insert into #Temporal
	select MateriaCorrelativaId from Correlativas
	where MateriaId = @MateriaID


	SELECT @AnioCarrera = AnioCarrera FROM Materias as Mat
	INNER join AniosCarreras AS Ani on Mat.AnioCarreraId = Ani.AnioCarreraId
	WHERE Mat.MateriaId = @MateriaID

	select Mat.MateriaId, CONCAT(Mat.MateriasCodigoBloque, ' - ', Mat.Nombre) AS Materia from Materias as Mat
	INNER join AniosCarreras AS Ani on Mat.AnioCarreraId = Ani.AnioCarreraId INNER JOIN Carreras as C
	ON Ani.CarreraId = C.CarreraId
	WHERE Ani.AnioCarrera < @AnioCarrera AND Ani.CarreraId = @CarreraID AND
	Mat.MateriaId NOT IN (select MateriaCorrelativaId FROM #Temporal)
	ORDER BY Ani.AnioCarrera, Mat.Nombre

	drop table #Temporal
	SET @msj = 'La lista se genero correctamente.'
END TRY

BEGIN CATCH
	SET @msj = 'Ocurrio un Error: ' + ERROR_MESSAGE() + ' en la linea ' + CONVERT(nvarchar(255), ERROR_LINE()) + '.'
END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_ListaMateriasAlumno]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[SP_ListaMateriasAlumno](
@AlumnoID as int,
@AnioCarreraID as int,
@msj as varchar(50) output
)
AS
BEGIN
BEGIN TRY
	create table #Temporal (
	MateriaId int)

	insert into #Temporal
	select MateriaId from AlumnoCicloLectivoMaterias
	where AlumnoId = @AlumnoID

	IF @AnioCarreraID = 0
	BEGIN
		SELECT @AnioCarreraID = AC.AnioCarreraId FROM AlumnosCarreras AlumCar
        INNER JOIN AniosCarreras AC ON AlumCar.CarreraId = AC.CarreraId
        WHERE AlumCar.AlumnoId = @AlumnoID AND AlumCar.Activo = 1 AND AC.AnioCarrera = 1
	END

	select * from Materias as Mat
	INNER join AniosCarreras AS Ani on Mat.AnioCarreraId = Ani.AnioCarreraId
	WHERE Ani.AnioCarreraId = @AnioCarreraID AND
	Mat.MateriaId NOT IN (select MateriaId FROM #Temporal)
	ORDER BY Mat.Nombre 

	drop table #Temporal
	SET @msj = 'La lista se genero correctamente.'
END TRY

BEGIN CATCH
	SET @msj = 'Ocurrio un Error: ' + ERROR_MESSAGE() + ' en la linea ' + CONVERT(nvarchar(255), ERROR_LINE()) + '.'
END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_ListaServiciosCursoMaterias]    Script Date: 19/6/2026 17:21:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[SP_ListaServiciosCursoMaterias](
@PersonalId as int,
@CursoId as int,
@CargoId as int
)
AS
BEGIN  
	create table #Temporal (
	CursoMateriaId int)
	 
	insert into #Temporal
	SELECT cm.CursoMateriaId from CursoMaterias cm inner join Servicios se on cm.CursoMateriaId=se.CursoMateriaId where se.PersonalId = @PersonalId and SE.Activo=1 AND se.CargoId=@CargoId

	select DISTINCT cm.CursoMateriaId, ma.Nombre from Servicios se full join CursoMaterias cm on se.CursoMateriaId=cm.CursoMateriaId INNER join Materias ma on cm.MateriaId=ma.MateriaId
	where cm.CursoMateriaId is not null AND cm.CursoMateriaId NOT IN (select CursoMateriaId from #temporal) AND cm.CursoId=@CursoId
	
	drop table #Temporal
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[73] 4[12] 2[11] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Alumnos"
            Begin Extent = 
               Top = 203
               Left = 950
               Bottom = 366
               Right = 1236
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "AlumnosCarreras"
            Begin Extent = 
               Top = 27
               Left = 889
               Bottom = 191
               Right = 1133
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Carreras"
            Begin Extent = 
               Top = 16
               Left = 965
               Bottom = 179
               Right = 1209
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CursadaAlumnoCarreras"
            Begin Extent = 
               Top = 25
               Left = 591
               Bottom = 220
               Right = 853
            End
            DisplayFlags = 280
            TopColumn = 3
         End
         Begin Table = "Cursadas"
            Begin Extent = 
               Top = 247
               Left = 649
               Bottom = 486
               Right = 896
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CursoMaterias"
            Begin Extent = 
               Top = 413
               Left = 291
               Bottom = 576
               Right = 555
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "Materias"
            Begin Extent = 
               Top = 449
               Left = 0
               Bottom = 612
               Right = 244
            ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'AlumnoMateriaCursoAnioCarrera'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Cursos"
            Begin Extent = 
               Top = 462
               Left = 618
               Bottom = 625
               Right = 862
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "AniosCarreras"
            Begin Extent = 
               Top = 407
               Left = 1028
               Bottom = 570
               Right = 1277
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CursadaAlumnoExamen"
            Begin Extent = 
               Top = 0
               Left = 245
               Bottom = 163
               Right = 480
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TipoExamen"
            Begin Extent = 
               Top = 221
               Left = 18
               Bottom = 431
               Right = 205
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "InstanciaExamenCursada"
            Begin Extent = 
               Top = 210
               Left = 331
               Bottom = 373
               Right = 583
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Examen"
            Begin Extent = 
               Top = 40
               Left = 11
               Bottom = 196
               Right = 195
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'AlumnoMateriaCursoAnioCarrera'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'AlumnoMateriaCursoAnioCarrera'
GO
USE [master]
GO
ALTER DATABASE [instituto_db] SET  READ_WRITE 
GO
