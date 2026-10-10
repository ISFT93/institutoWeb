# Instituto93

## Structure

- `Instituto93.sln`: `instituto93.Controller` (ASP.NET Core API, the only DI composition root in `Program.cs`), `instituto93.Application` (services), `instituto93.Data` (raw ADO.NET/SQL Server repositories), `instituto93.Domain` (models/interfaces), `instituto93.Web` (.NET 8 Blazor Server + MudBlazor frontend).
- Flow is controller -> application service -> data repository. Register each new repository/service pair in `instituto93.Controller/Program.cs`. Many existing controllers/services are not registered there (e.g. `AlumnoController` + `IAlumnoService`), so their endpoints fail at runtime with a DI error.
- `instituto93.Application` references `instituto93.Data` (not clean-architecture direction). Repository interfaces live under `Data/Repositories/Interfaces`.
- `instituto93.Web` is a separate process from the API. It calls the API through typed `HttpClient`s registered in `Web/Program.cs` (`AuthApiClient`), base URL from `Api:BaseUrl` (default `http://localhost:8000`). Never hardcode URLs in Razor components.
- The database schema is not in the repo except the numbered scripts in `Data/Queries/`; `Alumnos`, `Personal` (docentes) and `Localidades` are assumed to exist. Repository SQL does not always match the real DB (e.g. `AlumnoRepository.GetByIdAsync`/`UpdateAsync` reference a `Carrera` column that doesn't exist in `Alumnos`). Check the real schema before trusting a repository.
- Run the `Data/Queries` scripts manually, in order (all idempotent; nothing applies migrations automatically): `01-CreateRolesTable.sql` (roles `Alumno`, `Docente`), `02-CreateUsuariosTable.sql` (also migrates the old schema with personal-data columns), `03-CreateRefreshTokensTable.sql`. For an existing `Usuarios` without `RolId`, also run `04-MigrateUsuariosRoles.sql` (existing users become `Alumno`).

## Commands

- Build (only verification available; no tests, lint, or formatter config): `dotnet build Instituto93.sln`. It emits ~40 pre-existing nullable warnings; only worry about errors and new warnings in files you touched.
- Run both services with hot reload: `./start-dev.sh` (API on `:8000`, Web on `:8080`; Ctrl+C stops both). `start-dev.ps1` is the Windows equivalent.
- Run one: `dotnet run --project instituto93.Controller --launch-profile http` (Swagger at `http://localhost:8000/swagger`), `dotnet run --project instituto93.Web --launch-profile http` (`http://localhost:8080`).

## Runtime gotchas

- DB connection: `ConnectionStrings:InstiDb`. `appsettings.json` has `Password=CHANGE_ME`. Locally copy `.env.example` to `.env` (gitignored, loaded by `DotNetEnv` with `TraversePath`) or set `ConnectionStrings__InstiDb`.
- In Development the API runs `DevelopmentUserSeed` at startup, so it crashes on boot if the DB is unreachable or `Usuarios` still has the old schema. The seed creates an `Alumnos` row (DNI `99999999`) and a `Personal` row (docente, DNI `88888888`), each with its `Usuarios` row, password `PruebaInstituto93!`.
- `start-dev.sh` uses `dotnet watch`: saving code recompiles and reruns the seed against the DB immediately. If one service exits, the script stops both.
- API auth uses standard `AddJwtBearer` (`MapInboundClaims = false`, so claims are `sub`, `name`, `email`, `alumnoId`). `Jwt:Secret` (>= 32 bytes, env `Jwt__Secret`) is required outside Development; Development falls back to a constant in `Controller/Program.cs`. Lifetimes: `Jwt:AccessTokenMinutes` (15), `Jwt:RefreshTokenDays` (14, sliding), `Jwt:RefreshTokenAbsoluteDays` (30).
- `03-CreateRefreshTokensTable.sql` must be applied manually after the `Usuarios` scripts, or login fails.
- Routes are inconsistent: `AuthController` and `CursadaController` use `api/[controller]`; the rest use `[controller]` without the `api/` prefix.
- No CORS policy. Blazor Server's server-side `HttpClient` doesn't need one; a browser client from another origin would.

## Login/auth flow

- `Alumnos` (rol `Alumno`) and `Personal` (rol `Docente`, active = `FechaBaja IS NULL`; `Usuarios.ProfesorId` references `Personal.PersonalId`) are the source of truth for identity. `Usuarios` only stores credentials and role: `Id, RolId (FK to Roles), AlumnoId` or `ProfesorId` (exactly one, each unique), `Password, Activo`. Never add personal data to `Usuarios` or `Usuario`; read it through the joins (`Usuario.Alumno` / `Usuario.Profesor`). If a DNI exists in both tables, the docente wins in `dni-status`/`create-password`. The JWT carries `rol` plus `alumnoId` or `profesorId`.
- `Login.razor` is a step flow: DNI -> (password login | create password) -> success. Endpoints: `POST api/Auth/dni-status`, `POST api/Auth/create-password`, `POST api/Auth/login` (body `{ dni, password }`, DNI only). There is no register endpoint; `create-password` is the only way to create a user.
- DNI lookup matches `Alumnos.NumeroDocumento` with dots, spaces and hyphens stripped on both sides (`AlumnoAccesoService.NormalizarDni` in C#, `AlumnoRepository.NumeroDocumentoNormalizadoSql` in SQL, and again in `Login.razor`). "Has a password" means a `Usuarios` row exists for that `AlumnoId`.
- The DNI field is formatted as `12.345.678` in the browser by `wwwroot/js/dni-input.js` (applies to any input inside a `.dni-input` container). Don't use MudBlazor `Mask`/`PatternMask` for it: in Blazor Server every keystroke round-trips to the server and the caret races, scrambling digits when typing fast or on mobile.
- Password rule (min 8 chars, must match confirmation) is duplicated in `AlumnoAccesoService` and `Web/Models/CreatePasswordModel.cs`; change both. `UsuarioRepository` hashes with BCrypt, so pass plain text into `AddAsync`.
- Tokens (RFC 9700 / BFF pattern): `AuthTokenService` issues a 15-min JWT access token plus an opaque refresh token (only its SHA-256 is stored in `RefreshTokens`). Every `POST api/Auth/refresh` rotates it; reusing an already-rotated token revokes the whole family (every token from that login). `POST api/Auth/logout` revokes the family; `GET api/Auth/me` needs Bearer.
- Tokens never reach the browser. Web keeps them server-side (`TokenSessionStore`, `IDistributedCache`, in-memory so a Web restart logs everyone out) and the browser only gets an HttpOnly cookie whose `sid` claim points to that session.
- The cookie can't be written from the interactive circuit, so `Login.razor` logs in against the API, stores a one-minute single-use `LoginTicket`, and `js/auth.js` POSTs a hidden form (with antiforgery) to `/account/complete-login`, which signs in. Logout is `<LogoutForm />` -> `POST /account/logout`. Both endpoints live in `Web/Auth/AccountEndpoints.cs`.
- Renewal happens in `TokenSessionManager` (per-session lock, required because of rotation): on every HTTP request (`CookieSessionValidator`), every minute inside the circuit (`TokenRevalidatingAuthenticationStateProvider`), and on each API call (`AccessTokenProvider`).
- Pages require login by default (`Components/Pages/_Imports.razor` has `[Authorize]`); public pages need `@attribute [AllowAnonymous]`. For protected API endpoints, inherit from `AuthorizedApiClient` (see `AccountApiClient`): it adds the Bearer token and retries once after a 401. Don't use a `DelegatingHandler`, because it runs in a different DI scope than the circuit.
