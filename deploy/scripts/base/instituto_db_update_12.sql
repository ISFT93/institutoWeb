-- Script para crear la tabla CantAlumnosPreInscriptos en la base de datos instituto_db

USE instituto_db;

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='CantAlumnosPreInscriptos' AND xtype='U')
BEGIN
    CREATE TABLE CantAlumnosPreInscriptos (
        id INT PRIMARY KEY IDENTITY(1,1),
        carrera_id INT NOT NULL,
        anio_lectivo INT NOT NULL,
        anio INT NOT NULL,
        cantidad INT NOT NULL,
        FOREIGN KEY (carrera_id) REFERENCES Carreras(CarreraId),
        FOREIGN KEY (anio_lectivo) REFERENCES CicloLectivo(AnioLectivo)
    );
END;

---

-- Actualización para modificar el Máximo de caracteres de TipoLicencias -> Descripción

ALTER TABLE TipoLicencias
ALTER COLUMN Descripcion VARCHAR(60);
