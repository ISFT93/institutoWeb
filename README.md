# institutoWeb

Proyecto de desarrollo web para el instituto, con el objetivo de crear una plataforma interactiva para estudiantes y profesores.

## Levantar el proyecto

Para levantar el proyecto, sigue los siguientes pasos:

1. Clona el repositorio en tu máquina local:
    ```bash
    git clone https://github.com/ISFT93/institutoWeb.git
    ```
2. Navega al directorio del proyecto:
    ```bash
    cd institutoWeb
    ```
3. Copia el archivo de configuración de ejemplo:
    ```bash
    cp .env.example .env
    ```
4. Modifica el archivo `.env` con tus configuraciones locales (base de datos, puerto, etc.).
    Lo primordial es establecer la conexión a la base de datos y el puerto en el que correrá la aplicación.
    ```properties
    ConnectionStrings__InstiDb="Server=localhost,1433;Database=instituto_db;User Id=sa;Password=Contraseña;TrustServerCertificate=True;"
    ```
    o instancia local de SQL Server:
    ```properties
    ConnectionStrings__InstiDb="Server=MIPC\SQLEXPRESS;Database=instituto_db;User Id=sa;Password=Contraseña;TrustServerCertificate=True;"
    ```
    La base de datos **debe** estar creada con los scripts de la carpeta `./Base de Datos` del proyecto original, **[InstitutoNET](https://github.com/ISFT93/InstitutoNET)**.

    Debes ejecutar los scripts de esa carpeta **en orden** para crear la base de datos y sus tablas correctamente antes de levantar el proyecto.

    Una vez creada la base de datos, **es necesario ejecutar los scripts de `./instituto93.Data/Queries` de este proyecto** para actualizar las tablas con los datos necesarios para el funcionamiento de la aplicación.

5. Instala las dependencias del proyecto:
    ```bash
    dotnet restore
    ```
6. Ejecuta el proyecto de la API:
    ```bash
    cd ./instituto93.Controller;
    dotnet watch
    ```
7. Ejecuta el proyecto del Frontend:
    ```bash
    cd ./instituto93.Web;
    dotnet watch
    ```

En caso de usar Visual Studio Code, se puede ejecutar el script `./start-dev.sh` (Linux con Bash) o `./start-dev.ps1` (Windows con PowerShell) para levantar ambos proyectos de manera simultánea.

Alternativamente, en Visual Studio, se pueden establecer como proyecto de inicio `instituto93.Controller` y `instituto93.Web`.

## Datos de prueba

Al levantar el proyecto de esta manera, el entorno de .NET establece `ASPNETCORE_ENVIRONMENT` como `Development`. Esto causa que se ejecuten los `seeds` (datos de prueba) de la base de datos, los cuales crean un usuario administrador con las siguientes credenciales:

- DNI: `99999999`
- Email: `usuario.prueba@instituto93.local`
- Contraseña: `PruebaInstituto93!`

---

