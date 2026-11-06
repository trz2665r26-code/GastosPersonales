using System.Data;

namespace CapaDatos
{
    public class CategoriaDatos
    {
        public DataTable Listar()
        {
            return Conexion.Consultar("SELECT IdCategoria, Nombre FROM Categoria ORDER BY Nombre");
        }
    }
}
