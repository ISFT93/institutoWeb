using System.Text.Json;
using instituto93.Web.Models;

namespace instituto93.Web.Mocks;

/// <summary>
/// Datos de prueba leídos de los JSON de la carpeta Mocks. Solo se registra en Development y vive en memoria:
/// los cambios (evaluaciones, notas) se pierden al reiniciar y nunca llegan a la base de datos.
/// Los ids de los mocks empiezan en 9000 para no chocar con los reales.
/// </summary>
public sealed class MockDataStore
{
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);

    private readonly object _gate = new();
    private readonly List<CarrerasLookupModel> _carreras;
    private readonly List<TipoEvaluacionModel> _tipos;
    private readonly List<MateriaDocenteDetalleModel> _materias;
    private readonly List<EvaluacionMock> _evaluaciones;
    private readonly List<MesaFinalDetalleModel> _mesasDocente;
    private readonly List<MateriaInscripcionModel> _materiasInscripcion;
    private readonly List<MesaFinalInscripcionModel> _mesasAlumno;
    private int _nextEvaluacionId;

    public MockDataStore(IWebHostEnvironment environment)
    {
        var dir = Path.Combine(environment.ContentRootPath, "Mocks");
        List<T> Load<T>(string file) =>
            JsonSerializer.Deserialize<List<T>>(File.ReadAllText(Path.Combine(dir, file)), Json) ?? [];

        _carreras = Load<CarrerasLookupModel>("carreras.json");
        _tipos = Load<TipoEvaluacionModel>("tipos-evaluacion.json");
        _materias = Load<MateriaDocenteDetalleModel>("materias-docente.json");
        _evaluaciones = Load<EvaluacionMock>("evaluaciones-docente.json");
        _mesasDocente = Load<MesaFinalDetalleModel>("mesas-finales-docente.json");
        _materiasInscripcion = Load<MateriaInscripcionModel>("inscripcion-materias.json");
        _mesasAlumno = Load<MesaFinalInscripcionModel>("mesas-finales-alumno.json");
        _nextEvaluacionId = _evaluaciones.Select(e => e.EvaluacionId).DefaultIfEmpty(9100).Max() + 1;
    }

    public List<CarrerasLookupModel> Carreras() => [.. _carreras];

    public List<MateriaInscripcionModel> MateriasInscripcion() => [.. _materiasInscripcion];

    public List<MesaFinalInscripcionModel> MesasAlumno() => [.. _mesasAlumno];

    public List<TipoEvaluacionModel> Tipos() => [.. _tipos];

    public bool TieneMateria(int cursadaId) => _materias.Any(m => m.CursadaId == cursadaId);

    public bool TieneEvaluacion(int evaluacionId)
    {
        lock (_gate)
            return _evaluaciones.Any(e => e.EvaluacionId == evaluacionId);
    }

    public bool TieneMesa(int mesaId) => _mesasDocente.Any(m => m.MesaId == mesaId);

    public List<MateriaDocenteModel> MateriasDocente()
    {
        lock (_gate)
            return _materias
                .Select(m => new MateriaDocenteModel(
                    m.CursadaId, m.Materia, m.Carrera, m.Curso, m.AnioLectivo, m.Estudiantes.Count,
                    _evaluaciones.Count(e => e.CursadaId == m.CursadaId)))
                .ToList();
    }

    public MateriaDocenteDetalleModel? MateriaDocente(int cursadaId) =>
        _materias.FirstOrDefault(m => m.CursadaId == cursadaId);

    public List<EvaluacionDocenteModel> Evaluaciones(int cursadaId)
    {
        lock (_gate)
        {
            var total = MateriaDocente(cursadaId)?.Estudiantes.Count ?? 0;
            return _evaluaciones
                .Where(e => e.CursadaId == cursadaId)
                .OrderBy(e => e.Fecha)
                .Select(e => new EvaluacionDocenteModel(
                    e.EvaluacionId, e.TipoEvaluacionId, e.Nombre, DescripcionTipo(e.TipoEvaluacionId), e.Fecha,
                    e.Cuatrimestre, e.Estado, e.Ponderacion, e.Version.ToString(),
                    e.Notas.Count(n => n.Nota is not null || n.Ausente), total))
                .ToList();
        }
    }

    public void CrearEvaluacion(int cursadaId, CrearEvaluacionRequest request)
    {
        lock (_gate)
            _evaluaciones.Add(new EvaluacionMock
            {
                EvaluacionId = _nextEvaluacionId++,
                CursadaId = cursadaId,
                TipoEvaluacionId = request.TipoEvaluacionId,
                Nombre = request.Nombre,
                Fecha = request.Fecha,
                Cuatrimestre = request.Cuatrimestre,
                Estado = "Borrador",
            });
    }

    public void ActualizarEvaluacion(int evaluacionId, ActualizarEvaluacionRequest request)
    {
        lock (_gate)
        {
            var e = _evaluaciones.First(x => x.EvaluacionId == evaluacionId);
            e.TipoEvaluacionId = request.TipoEvaluacionId;
            e.Nombre = request.Nombre;
            e.Fecha = request.Fecha;
            e.Cuatrimestre = request.Cuatrimestre;
            e.Estado = request.Estado;
            e.Ponderacion = request.Ponderacion;
            e.Version++;
        }
    }

    public EvaluacionNotasModel? Notas(int evaluacionId)
    {
        lock (_gate)
        {
            var e = _evaluaciones.FirstOrDefault(x => x.EvaluacionId == evaluacionId);
            var materia = e is null ? null : MateriaDocente(e.CursadaId);
            if (e is null || materia is null)
                return null;

            var estudiantes = materia.Estudiantes.Select(s =>
            {
                var nota = e.Notas.FirstOrDefault(n => n.CursadaAlumnoCarreraId == s.CursadaAlumnoCarreraId);
                return new NotaAlumnoModel
                {
                    CursadaAlumnoCarreraId = s.CursadaAlumnoCarreraId,
                    AlumnoId = s.AlumnoId,
                    NombreCompleto = s.NombreCompleto,
                    Dni = s.Dni,
                    Nota = nota?.Nota,
                    Ausente = nota?.Ausente ?? false,
                    Observaciones = nota?.Observaciones,
                    Version = e.Version.ToString(),
                };
            }).ToList();

            return new EvaluacionNotasModel(
                e.EvaluacionId, e.CursadaId, e.Nombre, DescripcionTipo(e.TipoEvaluacionId), e.Fecha, e.Cuatrimestre,
                e.Estado, 1, 10, 6, estudiantes);
        }
    }

    public void GuardarNotas(int evaluacionId, GuardarNotasEvaluacionRequest request)
    {
        lock (_gate)
        {
            var e = _evaluaciones.First(x => x.EvaluacionId == evaluacionId);
            foreach (var n in request.Notas)
            {
                e.Notas.RemoveAll(x => x.CursadaAlumnoCarreraId == n.CursadaAlumnoCarreraId);
                e.Notas.Add(new NotaMock
                {
                    CursadaAlumnoCarreraId = n.CursadaAlumnoCarreraId,
                    Nota = n.Nota,
                    Ausente = n.Ausente,
                    Observaciones = n.Observaciones,
                });
            }
            e.Version++;
        }
    }

    public List<MesaFinalDocenteModel> MesasDocente()
    {
        lock (_gate)
            return _mesasDocente
                .Select(m => new MesaFinalDocenteModel(
                    m.MesaId, m.Materia, m.Carrera, m.FechaHora, m.Turno, m.Estado, m.Estudiantes.Count, m.Aula))
                .ToList();
    }

    public MesaFinalDetalleModel? MesaDocente(int mesaId)
    {
        lock (_gate)
        {
            var m = _mesasDocente.FirstOrDefault(x => x.MesaId == mesaId);
            return m is null
                ? null
                : m with
                {
                    // La página edita Nota en cada estudiante: se entregan copias.
                    Estudiantes = m.Estudiantes.Select(e => new EstudianteNotaFinalModel
                    {
                        MesaAlumnoId = e.MesaAlumnoId,
                        AlumnoId = e.AlumnoId,
                        NombreCompleto = e.NombreCompleto,
                        Dni = e.Dni,
                        Nota = e.Nota,
                        UltimaModificacion = e.UltimaModificacion,
                    }).ToList(),
                };
        }
    }

    public void GuardarNotasFinales(int mesaId, GuardarNotasFinalesRequest request)
    {
        lock (_gate)
        {
            var mesa = _mesasDocente.First(x => x.MesaId == mesaId);
            foreach (var n in request.Notas)
                if (mesa.Estudiantes.FirstOrDefault(e => e.MesaAlumnoId == n.MesaAlumnoId) is { } estudiante)
                    estudiante.Nota = n.Nota;
        }
    }

    private string DescripcionTipo(int tipoId) =>
        _tipos.FirstOrDefault(t => t.TipoEvaluacionId == tipoId)?.Descripcion ?? "Evaluación";

    private sealed class EvaluacionMock
    {
        public int EvaluacionId { get; set; }
        public int CursadaId { get; set; }
        public int TipoEvaluacionId { get; set; }
        public string Nombre { get; set; } = "";
        public DateTime Fecha { get; set; }
        public byte Cuatrimestre { get; set; }
        public string Estado { get; set; } = "Borrador";
        public decimal? Ponderacion { get; set; }
        public int Version { get; set; } = 1;
        public List<NotaMock> Notas { get; set; } = [];
    }

    private sealed class NotaMock
    {
        public int CursadaAlumnoCarreraId { get; set; }
        public decimal? Nota { get; set; }
        public bool Ausente { get; set; }
        public string? Observaciones { get; set; }
    }
}
