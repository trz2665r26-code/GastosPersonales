using System.Data;
using Microsoft.Data.SqlClient;

namespace CapaDatos
{
    public class GastoDatos
    {
        private const string SelectBase =
            "SELECT g.IdGasto, g.Descripcion, g.Monto, g.Fecha, g.IdCategoria, c.Nombre AS Categoria " +
            "FROM Gasto g INNER JOIN Categoria c ON c.IdCategoria = g.IdCategoria ";

        private SqlParameter ParametroFecha(DateTime fecha)
        {
            SqlParameter p = new SqlParameter("@Fecha", SqlDbType.Date);
            p.Value = fecha;
            return p;
        }

        public int Insertar(string descripcion, decimal monto, DateTime fecha, int idCategoria)
        {
            return Conexion.Ejecutar(
                "INSERT INTO Gasto (Descripcion, Monto, Fecha, IdCategoria) " +
                "VALUES (@Descripcion, @Monto, @Fecha, @IdCategoria)",
                new SqlParameter("@Descripcion", descripcion),
                new SqlParameter("@Monto", monto),
                ParametroFecha(fecha),
                new SqlParameter("@IdCategoria", idCategoria));
        }

        public int Actualizar(int idGasto, string descripcion, decimal monto, DateTime fecha, int idCategoria)
        {
            return Conexion.Ejecutar(
                "UPDATE Gasto SET Descripcion = @Descripcion, Monto = @Monto, " +
                "Fecha = @Fecha, IdCategoria = @IdCategoria WHERE IdGasto = @IdGasto",
                new SqlParameter("@Descripcion", descripcion),
                new SqlParameter("@Monto", monto),
                ParametroFecha(fecha),
                new SqlParameter("@IdCategoria", idCategoria),
                new SqlParameter("@IdGasto", idGasto));
        }

        public int Eliminar(int idGasto)
        {
            return Conexion.Ejecutar("DELETE FROM Gasto WHERE IdGasto = @IdGasto",
                new SqlParameter("@IdGasto", idGasto));
        }

        public DataTable Listar()
        {
            return Conexion.Consultar(SelectBase + "ORDER BY g.Fecha DESC, g.IdGasto DESC");
        }

        public DataTable ListarPorMesAnio(int mes, int anio)
        {
            return Conexion.Consultar(
                SelectBase + "WHERE MONTH(g.Fecha) = @Mes AND YEAR(g.Fecha) = @Anio ORDER BY g.Fecha DESC",
                new SqlParameter("@Mes", mes),
                new SqlParameter("@Anio", anio));
        }

        public DataTable TotalPorCategoria(int mes, int anio)
        {
            return Conexion.Consultar(
                "SELECT c.Nombre AS Categoria, SUM(g.Monto) AS Total " +
                "FROM Gasto g INNER JOIN Categoria c ON c.IdCategoria = g.IdCategoria " +
                "WHERE MONTH(g.Fecha) = @Mes AND YEAR(g.Fecha) = @Anio " +
                "GROUP BY c.Nombre ORDER BY Total DESC",
                new SqlParameter("@Mes", mes),
                new SqlParameter("@Anio", anio));
        }

        public DataTable MayorGastoDelMes(int mes, int anio)
        {
            return Conexion.Consultar(
                "SELECT TOP 1 g.Descripcion, g.Monto, g.Fecha, c.Nombre AS Categoria " +
                "FROM Gasto g INNER JOIN Categoria c ON c.IdCategoria = g.IdCategoria " +
                "WHERE MONTH(g.Fecha) = @Mes AND YEAR(g.Fecha) = @Anio ORDER BY g.Monto DESC",
                new SqlParameter("@Mes", mes),
                new SqlParameter("@Anio", anio));
        }
    }
}
