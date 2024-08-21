# Sistema de Cambio de Divisas (SCD)

El **Sistema de Cambio de Divisas (SCD)** es una aplicación de escritorio desarrollada en C# utilizando la tecnología Windows Forms y Sql Server, formando parte del entorno .NET Framework. 
Proporciona una solución integral para la gestión de operaciones de cambio de divisas, ofreciendo características avanzadas y una interfaz de usuario intuitiva.

## Características Destacadas

### Arquitectura de Tres Capas
- **Capa de Datos:**
  - Operaciones CRUD mediante ADO.NET.
  - Uso de stored procedures para operaciones complejas.
  - Manejo robusto de conexiones y excepciones.
- **Capa de Entidad:**
  - Clases que representan entidades con propiedades y relaciones.
  - Escrita en C# y utiliza .NET Framework.
- **Capa de Negocio:**
  - Implementa la lógica de negocio.
  - Interactúa con la capa de datos para presentar operaciones de negocio.
- **Capa de Presentación:**
  - Utiliza Windows Forms para la interfaz de usuario.
  - Implementa funcionalidades como filtros, listados y selección de registros.

### Funcionalidades Específicas
- Uso eficiente de DataGridView para mostrar datos tabulares.
- Generación de documentos PDF mediante iTextSharp para facilitar la documentación y registro.
- Integración de iconos de FontAwesome para mejorar la estética de la interfaz de usuario.
- Manejo avanzado de eventos y acciones del usuario.
- Uso de formularios modales para operaciones específicas, mejorando la experiencia del usuario.
- Validación exhaustiva de datos en la capa de presentación.
- Gestión de roles y permisos de usuario para garantizar la seguridad.

## Librerías y Tecnologías Utilizadas
- .NET Framework (System.Windows.Forms, System.Linq, System.Data para ADO.NET).
- ADO.NET para la conexión y manipulación de datos en la base de datos SQL Server.
- iTextSharp para generación de documentos PDF.
- FontAwesome.Sharp para la integración de iconos.
